using Cubicle.Components;
using Cubicle.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Cubicle.Systems {
    public class RenderSystem : EntityDrawSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Renderable> _renderableMapper;
        ComponentMapper<Mesh> _meshMapper;
        ComponentMapper<Transform3> _transformMapper;

        // TODO: Make this dynamic
        DynamicVertexBuffer buffer;

        public RenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Renderable), typeof(Mesh), typeof(Transform3))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _renderableMapper = mapperService.GetMapper<Renderable>();
            _meshMapper = mapperService.GetMapper<Mesh>();
            _transformMapper = mapperService.GetMapper<Transform3>();

            buffer = new DynamicVertexBuffer(_graphics, typeof(VertexPositionTextureLight),
                        (int)2e4, BufferUsage.WriteOnly);
        }

        public override void Draw(GameTime gameTime) {
            foreach (var entityId in ActiveEntities) {
                var renderable = _renderableMapper.Get(entityId);
                var mesh = _meshMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                buffer.SetData(mesh.Vertices);
                _graphics.SetVertexBuffer(buffer);

                foreach (EffectPass pass in renderable.Effect.CurrentTechnique.Passes) {
                    pass.Apply();
                    _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.VertexList.Count / 3);
                }
            }
        }
    }
}
