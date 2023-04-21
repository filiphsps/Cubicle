using Cubicle.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Cubicle.Systems {
    public class CameraSystem : EntityProcessingSystem {
        ComponentMapper<Camera> _cameraMapper;
        ComponentMapper<Transform3> _transformMapper;

        public CameraSystem()
            : base(Aspect.All(typeof(Camera))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _cameraMapper = mapperService.GetMapper<Camera>();
            _transformMapper = mapperService.GetMapper<Transform3>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            var camera = _cameraMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);

            // Make sure the position for the camera and transform match
            //
            // TODO: Maybe just treat the camera position as an offset in
            // those cases?
            if (transform != null)
                camera.Position = transform.Position;

            camera.View = Matrix.CreateLookAt(camera.Position, camera.Position + camera.Forward, camera.Up);
        }
    }
}
