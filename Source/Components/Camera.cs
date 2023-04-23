

using Microsoft.Xna.Framework;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Components {
    public class Camera {
        public Matrix _world;

        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 Forward = new Vector3(0, 0, -1);
        public Vector3 Up = Vector3.UnitY;

        public Matrix View = Matrix.Identity;
        public Matrix Projection = Matrix.Identity;
    }
}
