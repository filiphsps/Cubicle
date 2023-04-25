using Cubicle.Components;
using Cubicle.Gearset;
using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class PrepareRenderSystem : EntityUpdateSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Camera> _cameraMapper;
        ComponentMapper<Renderable> _renderableMapper;

        public PrepareRenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Camera), typeof(Renderable))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _cameraMapper = mapperService.GetMapper<Camera>();
            _renderableMapper = mapperService.GetMapper<Renderable>();
        }

        public override void Update(GameTime gameTime) {
            _graphics.Clear(Color.Aqua);
            _graphics.SamplerStates[0] = SamplerState.PointClamp;
            _graphics.DepthStencilState = DepthStencilState.Default;

            foreach (var entityId in ActiveEntities) {
                var camera = _cameraMapper.Get(entityId);
                var renderable = _renderableMapper.Get(entityId);

                renderable.World = Matrix.Identity;
                renderable.View = camera.View;
                renderable.Projection = camera.Projection;

                renderable.CameraPosition = camera.Position;
                renderable.Direction = Vector3.Normalize(camera.Forward);

                GS.SetMatrices(ref renderable.World, ref renderable.View, ref renderable.Projection);
            }

            DebugManager.Text($"{Math.Floor(1.0f / gameTime.GetElapsedSeconds())} FPS");
            DebugManager.Div();

            DebugManager.Text($"Framework: {RuntimeInformation.FrameworkDescription}", true);
            DebugManager.Text($"Runtime: {typeof(string).Assembly.ImageRuntimeVersion}", true);
            DebugManager.Div(true);

            // TODO: Utility class to convert bytes to readable
            var memory = Math.Round((double)(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024 * 1024)), 2);
            Process process = Process.GetCurrentProcess();
            var used_memory = Math.Round((double)(process.WorkingSet64 / (1024 * 1024)), 2);
            var allocated = Math.Round((double)(process.PrivateMemorySize64 / (1024 * 1024)), 2);

            DebugManager.Text($"Memory: {used_memory}/{memory}MB {((used_memory / memory) * 100).ToString("0")}%", true);
            DebugManager.Text($"Allocated: {allocated}MB", true);
            DebugManager.Div(true);

            DebugManager.Text($"{_graphics.Viewport.Width}x{_graphics.Viewport.Height} ({_graphics.Viewport.AspectRatio.ToString("0.00")})", true);
            DebugManager.Text(GraphicsAdapter.DefaultAdapter.Description, true);
            DebugManager.Div(true);

            DebugManager.Text($"Blocks: {String.Join(", ", BlocksManager.BlockNameMap)}", true);
            DebugManager.Text($"{TexturesManager.BlockTextures.Count} texture(s), " +
                $"{TexturesManager.ChunkAtlas.Count} atlas(es)", true);
            DebugManager.Div(true);
        }
    }
}
