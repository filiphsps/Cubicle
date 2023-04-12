using Cubicle.NET.Util;
using Silk.NET.Input;

namespace Cubicle.NET.Engine
{
    public class Input : Manager
    {
        IInputContext inputContext;

        public Input(IInputContext inputContext)
        {
            this.inputContext = inputContext;

            foreach (var keyboard in this.inputContext.Keyboards)
            {
                keyboard.KeyDown += KeyDown;
                keyboard.KeyUp += KeyUp;
            }
        }

        private void KeyDown(IKeyboard keyboard, Key key, int i)
        {
            Console.WriteLine(key);
        }

        private void KeyUp(IKeyboard keyboard, Key key, int i)
        {
        }

        public override void Update()
        {
        }
    }
}
