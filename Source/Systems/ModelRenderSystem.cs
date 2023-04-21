using Cubicle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Numerics;
using Quaternion = Microsoft.Xna.Framework.Quaternion;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class ModelRenderSystem : EntityDrawSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Renderable> _renderableMapper;
        ComponentMapper<Model> _modelMapper;
        ComponentMapper<Transform> _transformMapper;

        public ModelRenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Renderable), typeof(Model), typeof(Transform))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _renderableMapper = mapperService.GetMapper<Renderable>();
            _modelMapper = mapperService.GetMapper<Model>();
            _transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Draw(GameTime gameTime) {
            foreach (var entityId in ActiveEntities) {
                var renderable = _renderableMapper.Get(entityId);
                var model = _modelMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                var scale = Matrix.CreateScale(transform.Scale * 0.01f);
                var rotation = Matrix.Identity;
                var pos = Matrix.CreateTranslation(transform.Position);

                model.Draw(renderable.World * scale * rotation * pos, renderable.View, renderable.Projection);
            }
        }
    }
}
