using Cubicle.Components;
using Cubicle.Gearset;
using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Runtime.InteropServices;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class PrepareRenderSystem : EntityUpdateSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Camera> _cameraMapper;
        ComponentMapper<Renderable> _renderableMapper;
        // FIXME: Do this properly.
        private static int _fps = 0;
        private static float _timeSinceFpsUpdate = 0;

        public PrepareRenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Camera), typeof(Renderable))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _cameraMapper = mapperService.GetMapper<Camera>();
            _renderableMapper = mapperService.GetMapper<Renderable>();
        }

        public override void Update(GameTime gameTime) {
            GS.BeginMark("PrepareRenderSystem", Color.CornflowerBlue);
            _graphics.Clear(Color.Aqua);
            _graphics.SamplerStates[0] = SamplerState.PointClamp;
            _graphics.DepthStencilState = DepthStencilState.Default;

            GS.BeginMark("PrepareRenderSystem.Matrix", Color.MediumTurquoise);
            foreach (var entityId in ActiveEntities) {
                var camera = _cameraMapper.Get(entityId);
                var renderable = _renderableMapper.Get(entityId);

                renderable.World = Matrix.Identity * Matrix.CreateScale(1f)
                            * Matrix.CreateRotationX(0)
                            * Matrix.CreateRotationY(0)
                            * Matrix.CreateRotationZ(0);
                renderable.View = camera.View;
                renderable.Projection = camera.Projection;

                renderable.CameraPosition = camera.Position;
                renderable.Direction = Vector3.Normalize(camera.Forward);

                GS.SetMatrices(ref renderable.World, ref renderable.View, ref renderable.Projection);
            }
            GS.EndMark("PrepareRenderSystem.Matrix");

            GS.BeginMark("PrepareRenderSystem.Debug", Color.Purple);
            _timeSinceFpsUpdate += gameTime.GetElapsedSeconds();
            if (_timeSinceFpsUpdate >= 1 || _fps == 0) {
                _timeSinceFpsUpdate = 0;
                _fps = (int)Math.Floor(1.0f / gameTime.GetElapsedSeconds());
            }

            if (_fps > 0) {
                DebugManager.Text($"{_fps} FPS");
                DebugManager.Div();
            }

            DebugManager.Text($"Framework: {RuntimeInformation.FrameworkDescription}", true);
            DebugManager.Text($"Runtime: {typeof(string).Assembly.ImageRuntimeVersion}", true);
            DebugManager.Div(true);
            GS.EndMark("PrepareRenderSystem.Debug");
            GS.EndMark("PrepareRenderSystem");
        }
    }
}
