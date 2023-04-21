using Cubicle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

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
            _graphics.Clear(Color.Black);

            foreach (var entityId in ActiveEntities) {
                var camera = _cameraMapper.Get(entityId);
                var renderable = _renderableMapper.Get(entityId);

                renderable.World = Matrix.Identity;
                renderable.View = camera.View;
                renderable.Projection = camera.Projection;
            }
        }
    }
}
