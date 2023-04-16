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
            for (int i = 0; i < this.inputContext.Mice.Count; i++)
            {
                this.inputContext.Mice[i].Cursor.CursorMode = CursorMode.Raw;
                this.inputContext.Mice[i].MouseMove += OnMouseMove;
            }
        }

        public override void Update(double delta)
        {
            var moveSpeed = 2.5f * (float)delta;

            var primaryKeyboard = inputContext.Keyboards.FirstOrDefault();
            if (primaryKeyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                Renderer.camera.Position += moveSpeed * Renderer.camera.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                Renderer.camera.Position -= moveSpeed * Renderer.camera.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.A))
            {
                //Move left
                Renderer.camera.Position -= Vector3.Normalize(Vector3.Cross(Renderer.camera.Front, Renderer.camera.Up)) * moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.D))
            {
                //Move right
                Renderer.camera.Position += Vector3.Normalize(Vector3.Cross(Renderer.camera.Front, Renderer.camera.Up)) * moveSpeed;
            }
        }

        private static Vector2 LastMousePosition;
        private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
        {
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

                Renderer.camera.Yaw += xOffset;
                Renderer.camera.Pitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                Renderer.camera.Pitch = Math.Clamp(Renderer.camera.Pitch, -89.0f, 89.0f);

                Renderer.camera.Direction.X = MathF.Cos(MathHelper.DegreesToRadians(Renderer.camera.Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Renderer.camera.Pitch));
                Renderer.camera.Direction.Y = MathF.Sin(MathHelper.DegreesToRadians(Renderer.camera.Pitch));
                Renderer.camera.Direction.Z = MathF.Sin(MathHelper.DegreesToRadians(Renderer.camera.Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Renderer.camera.Pitch));
                Renderer.camera.Front = Vector3.Normalize(Renderer.camera.Direction);
            }
        }
    }
}
