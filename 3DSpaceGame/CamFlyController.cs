﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace _3DSpaceGame {
    class CamFlyController : Component {

        public override void Start() {
            Input.FixedMouse(true);
        }

        public override void Update() {

            float speed = .4f;
            if (Input.IsKeyDown(OpenTK.Input.Key.LShift)) {
                speed = 1.6f;
            }

            var translation = new Vector3();
            var wasd = Input.Wasd;
            translation += transform.forward * wasd.Y;
            translation += transform.left * wasd.X;
            translation += transform.up * Input.KeyAxis(OpenTK.Input.Key.Space, OpenTK.Input.Key.LControl);
            transform.position += translation * speed;

            //transform.Rotate(transform.up, Input.MouseDelta.X / 100);
            //transform.Rotate(transform.right, Input.MouseDelta.Y / 100);
            //transform.Rotate(transform.forward, Input.KeyAxis(OpenTK.Input.Key.E, OpenTK.Input.Key.Q) / 15);
            transform.Rotate(new Vector3(Input.MouseDelta.Y / 100f, -Input.MouseDelta.X / 100f, -Input.KeyAxis(OpenTK.Input.Key.Q, OpenTK.Input.Key.E) / 10f));


        }
    }
}
