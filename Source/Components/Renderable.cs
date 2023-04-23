using Microsoft.Xna.Framework;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Components {
    public class Renderable {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;

        public Vector3 CameraPosition;
        public Vector3 Direction;
    }
}
