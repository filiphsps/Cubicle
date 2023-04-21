using Cubicle.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Cubicle.Systems {
    public class CameraSystem : EntityProcessingSystem {
        ComponentMapper<Camera> _cameraMapper;

        public CameraSystem()
            : base(Aspect.All(typeof(Camera))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _cameraMapper = mapperService.GetMapper<Camera>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            var camera = _cameraMapper.Get(entityId);

            camera.View = Matrix.CreateLookAt(camera.Position, camera.Position + camera.Direction, Vector3.Up);
        }
    }
}
