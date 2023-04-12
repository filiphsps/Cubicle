using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Cubicle.NET.Engine;
using Silk.NET.Input;
using Cubicle.NET.Util;
using Cubicle.NET.Engine.Rendering;

namespace Cubicle.NET
{
    public class Cubicle
    {
        private Input input;
        private Renderer? renderer;
        private Steam steam;

        private IWindow window;
        private GL? gl;

        public Cubicle()
        {
            window = Window.Create(WindowOptions.Default with {
                Position = new Vector2D<int>(800, 600),
                Title = "Cubicle"
            });

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;

            window.Initialize();

            input = new Input(window.CreateInput());
            steam = new Steam();
        }

        public void Run()
        {
            window.Run();
            window.Dispose();
        }

        private void OnLoad()
        {
            gl = window.CreateOpenGL();
            renderer = new Renderer(gl);
        }

        private void OnUpdate(double a)
        {
            input.Update();
            renderer?.Update();
        }

        private void OnRender(double a)
        {
            renderer?.Render();
        }
    }
}
