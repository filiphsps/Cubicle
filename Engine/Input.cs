using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Assimp;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Maths;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Input : Manager
    {
        IInputContext inputContext;

        public Input(IInputContext inputContext)
        {
            this.inputContext = inputContext;
            this.inputContext.Keyboards[0].KeyDown += KeyDown;

            for (int i = 0; i < this.inputContext.Mice.Count; i++)
            {
                this.inputContext.Mice[i].Cursor.CursorMode = CursorMode.Raw;
                this.inputContext.Mice[i].MouseMove += OnMouseMove;
            }
        }

        private void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Q)
            {
                Cubicle.Renderer.ToggleWireframe();
            }

            if (key == Key.Escape)
            {
                Cubicle.Quit();
            }

            if (key == Key.Tab)
            {
                if (this.inputContext.Mice[0].Cursor.CursorMode == CursorMode.Raw)
                    this.inputContext.Mice[0].Cursor.CursorMode = CursorMode.Normal;
                else
                    this.inputContext.Mice[0].Cursor.CursorMode = CursorMode.Raw;
            }

            if (key == Key.Up)
            {
                Renderer.Camera.Speed += 2.5f;
            }
            if (key == Key.Down)
            {
                Renderer.Camera.Speed -= 2.5f;
            }
        }

        public override void Update(double delta)
        {
            var moveSpeed = Renderer.Camera.Speed * (float)delta;

            var primaryKeyboard = inputContext.Keyboards.FirstOrDefault();
            if (primaryKeyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                Renderer.Camera.Position += moveSpeed * Renderer.Camera.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                Renderer.Camera.Position -= moveSpeed * Renderer.Camera.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.A))
            {
                //Move left
                Renderer.Camera.Position -= Vector3.Normalize(Vector3.Cross(Renderer.Camera.Front, Renderer.Camera.Up)) * moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.D))
            {
                //Move right
                Renderer.Camera.Position += Vector3.Normalize(Vector3.Cross(Renderer.Camera.Front, Renderer.Camera.Up)) * moveSpeed;
            }

            if (primaryKeyboard.IsKeyPressed(Key.Space))
            {
                //Move right
                Renderer.Camera.Position.Y +=  moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.ShiftLeft))
            {
                //Move right
                Renderer.Camera.Position.Y -= moveSpeed;
            }
        }

        private static Vector2 LastMousePosition;
        private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
        {
            // We should be able to look around when we aren't caputring the cursor
            if (mouse.Cursor.CursorMode != CursorMode.Raw)
                return;

            var lookSensitivity = 0.1f;
            if (LastMousePosition == default)
            {
                LastMousePosition = position;
            }
            else
            {
                var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;

                Renderer.Camera.Yaw += xOffset;
                Renderer.Camera.Pitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                Renderer.Camera.Pitch = Math.Clamp(Renderer.Camera.Pitch, -89.0f, 89.0f);

                Renderer.Camera.Direction.X = MathF.Cos(MathHelper.DegreesToRadians(Renderer.Camera.Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Renderer.Camera.Pitch));
                Renderer.Camera.Direction.Y = MathF.Sin(MathHelper.DegreesToRadians(Renderer.Camera.Pitch));
                Renderer.Camera.Direction.Z = MathF.Sin(MathHelper.DegreesToRadians(Renderer.Camera.Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Renderer.Camera.Pitch));
                Renderer.Camera.Front = Vector3.Normalize(Renderer.Camera.Direction);
            }
        }
    }
}
