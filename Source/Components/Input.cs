using System.Numerics;

namespace Cubicle.Components {
    public enum KeyState {
        Released,
        Pressed
    }

    public class Input {
        public bool Enabled = true;
        public float Sensitivity = 2.0f;

        public KeyState Forward;
        public KeyState Backward;
        public KeyState Left;
        public KeyState Right;

        public KeyState Up;
        public KeyState Down;

        public Vector2 MousePointer = Vector2.Zero;
        public Vector2 LastMousePointer = Vector2.Zero;
        public Vector2 MouseDelta;
    }
}
