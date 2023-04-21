﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubicle.Components {
    public class Camera {
        public Matrix _world;

        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Forward = Vector3.Forward;
        public Vector3 Up = Vector3.UnitY;

        public Matrix View = Matrix.Identity;
        public Matrix Projection = Matrix.Identity;
    }
}