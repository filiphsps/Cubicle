using Cubicle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Numerics;
using Quaternion = Microsoft.Xna.Framework.Quaternion;

namespace Cubicle.Systems {
    public class ModelRenderSystem : EntityDrawSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Renderable> _renderableMapper;
        ComponentMapper<Model> _modelMapper;
        ComponentMapper<Transform3> _transformMapper;

        public ModelRenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Renderable), typeof(Model), typeof(Transform3))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _renderableMapper = mapperService.GetMapper<Renderable>();
            _modelMapper = mapperService.GetMapper<Model>();
            _transformMapper = mapperService.GetMapper<Transform3>();
        }

        public override void Draw(GameTime gameTime) {
            foreach (var entityId in ActiveEntities) {
                var renderable = _renderableMapper.Get(entityId);
                var model = _modelMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                // TODO: Remove
                float diff = MathHelper.ToRadians((float)gameTime.GetElapsedSeconds() * 50);
                transform.Rotation = transform.Rotation + Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationX(diff));

                var scale = Matrix.CreateScale(transform.Scale);
                var rotation = Matrix.CreateRotationY(transform.Rotation.X)
                    * Matrix.CreateRotationX(transform.Rotation.Y)
                    * Matrix.CreateRotationZ(transform.Rotation.Z);
                var pos = Matrix.CreateTranslation(transform.Position);

                model.Draw(renderable.World * scale * rotation * pos, renderable.View, renderable.Projection);
            }
        }
    }
}
