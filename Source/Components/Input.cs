namespace Cubicle.Components {
    public enum KeyState {
        Released,
        Pressed
    }

    public class Input {
        public KeyState Forward;
        public KeyState Backward;
        public KeyState Left;
        public KeyState Right;
    }
}
