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
            var font = new ImGuiFontConfig("res/test.ttf", 20);
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

            var pos = Renderer.player.Position;
            var vel = Renderer.player.Velocity;
            ImGui.GetWindowDrawList().AddText(new Vector2(5, 5),
                0xFFffFFff,
                $"Position - X: {pos.X.ToString("0.00")}, Y: {pos.Y.ToString("0.00")}, Z: {pos.Z.ToString("0.00")}");
            ImGui.GetWindowDrawList().AddText(new Vector2(5, 25),
                0xFFffFFff,
                $"Velocity - X: {vel.X.ToString("0.00")}, Y: {vel.Y.ToString("0.00")}, Z: {vel.Z.ToString("0.00")}");
        }

        public void Render(double delta)
        {
            controller.Render();
        }
    }
}
