using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Assimp;
using Silk.NET.Input;
using Silk.NET.Maths;
using System.Collections.Generic;
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
                this.inputContext.Mice[i].Click += OnMouseClick;
            }
        }

        private void OnMouseClick(IMouse mouse, MouseButton button, Vector2 arg3)
        {
            if (Cubicle.Player.Target == null)
                return;

            if (button == MouseButton.Left)
                Renderer.chunk.RemoveBlock((Vector3)Cubicle.Player.Target);
        }

        private void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
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

            if (Cubicle.Player == null || Cubicle.Renderer == null)
                return;

            if (key == Key.Q)
            {
                Cubicle.Renderer.ToggleWireframe();
            }

            if (key == Key.Up)
            {
                Cubicle.Player.Speed += 2.5f;
            }
            if (key == Key.Down)
            {
                Cubicle.Player.Speed -= 2.5f;
            }
        }

        public override void Update(double delta)
        {
            if (Cubicle.Player == null || Cubicle.Renderer == null)
                return;

            var moveSpeed = Cubicle.Player.Speed * (float)delta;

            var primaryKeyboard = inputContext.Keyboards.FirstOrDefault();
            if (primaryKeyboard == null)
                return;

            if (primaryKeyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                Cubicle.Player.Position += moveSpeed * Cubicle.Player.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                Cubicle.Player.Position -= moveSpeed * Cubicle.Player.Front;
            }
            if (primaryKeyboard.IsKeyPressed(Key.A))
            {
                //Move left
                Cubicle.Player.Position -= Vector3.Normalize(Vector3.Cross(Cubicle.Player.Front, Cubicle.Player.Up)) * moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.D))
            {
                //Move right
                Cubicle.Player.Position += Vector3.Normalize(Vector3.Cross(Cubicle.Player.Front, Cubicle.Player.Up)) * moveSpeed;
            }

            if (primaryKeyboard.IsKeyPressed(Key.Space))
            {
                Cubicle.Player.Position.Y +=  moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.ShiftLeft))
            {
                Cubicle.Player.Position.Y -= moveSpeed;
            }
        }

        private static Vector2 LastMousePosition;
        private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
        {
            // We should be able to look around when we aren't caputring the cursor
            if (mouse.Cursor.CursorMode != CursorMode.Raw || Cubicle.Player == null)
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

                Cubicle.Player.Yaw += xOffset;
                Cubicle.Player.Pitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                Cubicle.Player.Pitch = Math.Clamp(Cubicle.Player.Pitch, -89.0f, 89.0f);

                Cubicle.Player.Direction.X = MathF.Cos(MathHelper.DegreesToRadians(Cubicle.Player.Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Cubicle.Player.Pitch));
                Cubicle.Player.Direction.Y = MathF.Sin(MathHelper.DegreesToRadians(Cubicle.Player.Pitch));
                Cubicle.Player.Direction.Z = MathF.Sin(MathHelper.DegreesToRadians(Cubicle.Player.Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Cubicle.Player.Pitch));
                Cubicle.Player.Front = Vector3.Normalize(Cubicle.Player.Direction);

                // TODO: Move this into player
                Cubicle.Player.MinDistanceToTarget = 7.5f;
            }
        }
    }
}
