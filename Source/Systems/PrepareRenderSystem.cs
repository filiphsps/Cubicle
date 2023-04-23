using Cubicle.Components;
using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Runtime.InteropServices;

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
            }

            DebugManager.Text($"{Math.Floor(1.0f / gameTime.GetElapsedSeconds())} FPS");
            DebugManager.Div();

            DebugManager.Text($"Framework: {RuntimeInformation.FrameworkDescription}", true);
            DebugManager.Text($"Runtime: {typeof(string).Assembly.ImageRuntimeVersion}", true);
            DebugManager.Div(true);

            var memory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024 * 1024);
            var used_memory = GC.GetTotalMemory(false) / (1024 * 1024);
            var allocated = GC.GetTotalAllocatedBytes(true) / (1024 * 1024);
            DebugManager.Text($"Memory: {(used_memory / memory).ToString("0%")} {used_memory}/{memory}MB", true);
            DebugManager.Text($"Allocated: {allocated}MB", true);
            DebugManager.Div(true);

            DebugManager.Text($"{_graphics.Viewport.Width}x{_graphics.Viewport.Height} ({_graphics.Viewport.AspectRatio.ToString("0.00")})", true);
            DebugManager.Text(GraphicsAdapter.DefaultAdapter.Description, true);
            DebugManager.Div(true);

            DebugManager.Text($"{TexturesManager.BlockTextures.Count} texture(s), " +
                $"{TexturesManager.ChunkAtlas.Count} atlas(es)", true);
        }
    }
}
