using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubicle.Components {
    public class Camera {
        public Vector3 Position = new Vector3(10, 0, 10);
        public Vector3 Direction;

        public Matrix View;
        public Matrix Projection;
    }
}
