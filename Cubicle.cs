using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Cubicle.NET.Engine;
using Silk.NET.Input;
using Cubicle.NET.Util;
using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Debug;
using Silk.NET.Core;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Runtime.InteropServices;

namespace Cubicle.NET
{
    public class Cubicle
    {
        public static Engine.Rendering.Renderer? Renderer;
        public static Player? Player;

        private Input input;
        private Steam steam;
        private DebugScreen? debug;

        public static IWindow? Window;
        private IInputContext input_context;
        private GL? gl;

        public Cubicle()
        {
            Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with {
                Size = new Vector2D<int>(800, 600),
                Title = "Cubicle",
                WindowBorder = WindowBorder.Fixed,
                VSync = false,
            });

            Window.Load += OnLoad;
            Window.Update += OnUpdate;
            Window.Render += OnRender;
            Window.FocusChanged += OnFocusChanged;

            Window.Initialize();

            input_context = Window.CreateInput();
            input = new Input(input_context);
            steam = new Steam();
        }

        private void OnFocusChanged(bool focus)
        {
        }

        public void Run()
        {
            Window!.Run();
            Window!.Dispose();
        }

        public static void Quit()
        {
            Window!.Close();
        }

        private void OnLoad()
        {
            var img = Image.Load<Rgba32>("Res/Icon.png");
            var _IMemoryGroup = img.GetPixelMemoryGroup();
            var _MemoryGroup = _IMemoryGroup.ToArray()[0];
            var PixelData = MemoryMarshal.AsBytes(_MemoryGroup.Span).ToArray();

            var icon = new RawImage(64, 64, new Memory<byte>(PixelData));
            Window!.SetWindowIcon(ref icon);

            gl = GL.GetApi(Window);
            Player = new Player(gl);
            Renderer = new Renderer(gl);
        }

        private void OnUpdate(double delta)
        {
            if (debug == null && gl != null) {
                debug = new DebugScreen(gl, Window!, input_context);
            }

            input.Update(delta);
            Player?.Update(delta);
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
