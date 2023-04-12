using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Cubicle.NET.Engine;
using Silk.NET.Input;
using Cubicle.NET.Util;
using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Debug;

namespace Cubicle.NET
{
    public class Cubicle
    {
        private Input input;
        private Renderer? renderer;
        private Steam steam;
        private DebugScreen? debug;

        private IWindow window;
        private IInputContext input_context;
        private GL? gl;

        public Cubicle()
        {
            window = Window.Create(WindowOptions.Default with {
                Size = new Vector2D<int>(800, 600),
                Title = "Cubicle",
                WindowBorder = WindowBorder.Fixed
            });

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;

            window.Initialize();

            input_context = window.CreateInput();
            input = new Input(input_context);
            steam = new Steam();
        }

        public void Run()
        {
            window.Run();
            window.Dispose();
        }

        private void OnLoad()
        {
            gl = GL.GetApi(window);
            renderer = new Renderer(gl);
        }

        private void OnUpdate(double delta)
        {
            if (debug == null) {
                debug = new DebugScreen(gl, window, input_context);
            }

            input.Update(delta);
            renderer?.Update(delta);
            debug?.Update(delta);
        }

        private void OnRender(double delta)
        {
            renderer?.Render(delta);
            debug?.Render(delta);
        }
    }
}
