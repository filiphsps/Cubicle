using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System.Numerics;

namespace Cubicle.NET.Debug
{
    public class DebugScreen : Manager
    {
        private ImGuiController controller;
        public DebugScreen(GL gl, IWindow window, IInputContext input)
        {
            var font = new ImGuiFontConfig("Res/font.ttf", 20);
            controller = new ImGuiController(gl, window, input, font);
        }

        public override void Update(double delta)
        {
            controller.Update((float)delta);

            ImGui.Begin("",
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoInputs |
                ImGuiWindowFlags.NoDecoration |
                ImGuiWindowFlags.NoDocking |
                ImGuiWindowFlags.NoCollapse);

            ImGui.SetWindowSize(new Vector2(800, 600));
            ImGui.SetWindowPos(new Vector2(0, 0));

            var pos = Renderer.Camera.Position;
            ImGui.GetWindowDrawList().AddText(new Vector2(5, 5),
                0xFFffFFff,
                $"{(1 / delta).ToString("000.00")} FPS");

            ImGui.GetWindowDrawList().AddText(new Vector2(5, 30),
                0xFFffFFff,
                $"X: {pos.X.ToString("0.00")}, Y: {pos.Y.ToString("0.00")}, Z: {pos.Z.ToString("0.00")}");

            ImGui.GetWindowDrawList().AddText(new Vector2(5, 55),
                0xFFffFFff,
                $"Yaw: {Renderer.Camera.Yaw.ToString("0.00")}, Pitch: {Renderer.Camera.Pitch.ToString("0.00")}, Zoom: {Renderer.Camera.Zoom.ToString("0.00")}, Speed: {Renderer.Camera.Speed.ToString("0.00")}");


            ImGui.GetWindowDrawList().AddText(new Vector2(5, 535),
                0xFFffFFff,
                $"ESC - Quit\n" +
                $"Q   - Wireframes\n" +
                $"TAB - Toggle Captured Cursor");
        }

        public void Render(double delta)
        {
            controller.Render();
        }
    }
}
