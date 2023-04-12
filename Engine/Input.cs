using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace Cubicle.NET.Engine
{
    public class Input : Manager
    {
        IInputContext inputContext;

        private Vector2D<double> last_mouse;
        private Vector2D<double> delta_mouse;
        private Vector2D<double> normalized_delta_mouse;

        public Input(IInputContext inputContext)
        {
            this.inputContext = inputContext;
            this.inputContext.Mice[0].Cursor.CursorMode = CursorMode.Raw;
        }

        public override void Update(double delta)
        {
            var pos = this.inputContext.Mice[0].Position;

            delta_mouse = new Vector2D<double>(pos.X, pos.Y) - last_mouse;
            normalized_delta_mouse = delta_mouse / new Vector2D<double>(800, 600);
            last_mouse = new Vector2D<double>(pos.X, pos.Y);

            Console.Clear();
            Console.WriteLine("Pos       : " + Renderer.player.Position);
            Console.WriteLine("Vel       : " + Renderer.player.Velocity);
            Console.WriteLine("Delta     : " + delta_mouse);
            Console.WriteLine("Norm Delta: " + normalized_delta_mouse);

            Renderer.player.CameraRotationY -= (float)normalized_delta_mouse.X;
            Renderer.player.CameraRotationX -= (float)normalized_delta_mouse.Y;

            float moveF = 0.0f, moveL = 0.0f;
            if (this.inputContext.Keyboards[0].IsKeyPressed(Key.W))
                moveF += 1.0f;
            if (this.inputContext.Keyboards[0].IsKeyPressed(Key.S))
                moveF -= 1.0f;
            if (this.inputContext.Keyboards[0].IsKeyPressed(Key.A))
                moveL += 1.0f;
            if (this.inputContext.Keyboards[0].IsKeyPressed(Key.D))
                moveL -= 1.0f;

            if (this.inputContext.Keyboards[0].IsKeyPressed(Key.Space))
                Renderer.player.Velocity.Y += 0.05f;
            if (this.inputContext.Keyboards[0].IsKeyPressed(Key.ControlLeft))
                Renderer.player.Velocity.Y -= 0.05f;

            Renderer.player.Move(moveF, moveL, delta);
        }
    }
}
