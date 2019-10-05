﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {

    public class Particle {
        public readonly Transform transform = new Transform();
        public float time = 0;
        public Vector3 velocity;
        public bool enabled = false;

        
        public void Reset() {
            time = 0;
            enabled = false;
            transform.position = Vector3.Zero;
            transform.rotation = Quaternion.Identity;
            transform.scale = Vector3.One;
            velocity = Vector3.Zero;
        }
        
    }

    public abstract class ParticleSystem : Component {

        private readonly List<Particle> particles = new List<Particle>();

        protected IEnumerable<Particle> EnabledParticles => from p in particles
                                                          where p.enabled == true
                                                          select p;

        protected IEnumerable<Particle> DisabledParticles => from o in particles
                                                           where o.enabled == false
                                                           select o;

        protected bool LocalCoords;

        protected int ParticleCount {
            get => particles.Count;
            set {
                if (particles.Count < value) {
                    var num = value - particles.Count;
                    for (int i = 0; i < num; i++) {
                        particles.Add(new Particle());
                    }
                } else {
                    var num = particles.Count - value;
                    for (int i = 0; i < num; i++) {
                        particles.RemoveAt(0);
                    }
                }
            }
        }
        
        public ParticleSystem(int pcount, bool local = true) {
            ParticleCount = pcount;
            LocalCoords = local;
        }

        public override void EarlyUpdate() {

            // Update & kill current particles
            for (int i = 0; i < particles.Count; i++) {
                var p = particles[i];
                if (p.enabled) {

                    p.time += Program.DeltaTime;
                    p.transform.Translate(p.velocity * Program.DeltaTime);

                    UpdateParticle(p);

                    if (ParticleEndCondition(p)) {
                        p.Reset();
                    }
                }
            }
        }

        private float particlesQued = 0;
        public void Queue(float num) {
            particlesQued += num;
            for (int i = 0; i < MyMath.Floor(particlesQued); i++) {
                Spawn();
                particlesQued--;
            }
        }

        private void Spawn() {
            if (DisabledParticles.Count() > 0) {
                var p = DisabledParticles.ElementAt(0);
                p.enabled = true;
                StartParticle(p);
            }
        }

        public override void Render() {
            Material.WhitePlastic.Apply(Program.StandardShader);
            for (int i = 0; i < particles.Count; i++) {
                var p = particles[i];
                if (!p.enabled) {
                    continue;
                }
                var m = p.transform.matrix;
                if (LocalCoords) {
                    m *= gameObject.ModelMatrix;
                }
                Program.StandardShader.SetMat4("obj_transform", m);
                RenderParticle(p);
            }
        }

        protected abstract bool ParticleEndCondition(Particle p);
        protected abstract void StartParticle(Particle p);
        protected abstract void UpdateParticle(Particle p);
        protected abstract void RenderParticle(Particle p);

    }
}
