using System.Numerics;

namespace Cubicle.Components {
    public class Transform {
        public Vector3 Scale = Vector3.One;

        public Vector3 Position = Vector3.Zero;
        public Vector3 Velocity = Vector3.Zero;

        public Vector3 Forward = new Vector3(0f, 0f, -1f);
    }
}
