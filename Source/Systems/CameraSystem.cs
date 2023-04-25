using Cubicle.Components;
using Cubicle.Gearset;
using Cubicle.Singletons;
using Cubicle.Util;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Cubicle.Systems {
    public class CameraSystem : EntityProcessingSystem {
        ComponentMapper<Camera> _cameraMapper;
        ComponentMapper<Transform> _transformMapper;

        public CameraSystem()
            : base(Aspect.All(typeof(Camera))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _cameraMapper = mapperService.GetMapper<Camera>();
            _transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            GS.BeginMark("CameraSystem", Color.Coral);
            var camera = _cameraMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);

            // Make sure the position for the camera and transform match
            //
            // TODO: Maybe just treat the camera position as an offset in
            // those cases?
            if (transform != null) {
                camera.Position = transform.Position;
                camera.Forward = transform.Forward;
                camera.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75),
                    (float)Cubicle.Viewport.Width / Cubicle.Viewport.Height, 0.1f, 1000f);
            }

            camera.View = Matrix.CreateLookAt(camera.Position, camera.Position + camera.Forward, camera.Up);

            DebugManager.Text($"Direction: {StringHelpers.CardinalToString(camera.Forward.Cardinal())}", true);
            DebugManager.Div();
            GS.EndMark("CameraSystem");
        }
    }
}
