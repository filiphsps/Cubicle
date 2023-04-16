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
        public static Renderer? Renderer;
        private Steam steam;
        private DebugScreen? debug;

        public static IWindow Window;
        private IInputContext input_context;
        private GL? gl;

        public Cubicle()
        {
            Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with {
                Size = new Vector2D<int>(800, 600),
                Title = "Cubicle",
                WindowBorder = WindowBorder.Fixed,
                VSync = false
            });

            Window.Load += OnLoad;
            Window.Update += OnUpdate;
            Window.Render += OnRender;

            Window.Initialize();

            input_context = Window.CreateInput();
            input = new Input(input_context);
            steam = new Steam();
        }

        public void Run()
        {
            Window.Run();
            Window.Dispose();
        }

        public static void Quit()
        {
            Window.Close();
        }

        private void OnLoad()
        {
            gl = GL.GetApi(Window);
            Renderer = new Renderer(gl);
        }

        private void OnUpdate(double delta)
        {
            if (debug == null) {
                debug = new DebugScreen(gl, Window, input_context);
            }

            input.Update(delta);
            Renderer?.Update(delta);
            debug?.Update(delta);
        }

        private void OnRender(double delta)
        {
            Renderer?.Render(delta);
            debug?.Render(delta);
        }
    }
}
