using Cubicle.Components;
using Cubicle.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

namespace Cubicle.Systems {
    public class ChunkRenderSystem : EntityDrawSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Renderable> _renderableMapper;
        ComponentMapper<Chunks> _chunksMapper;

        DynamicVertexBuffer _buffer;

        public ChunkRenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Renderable), typeof(Chunks))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _renderableMapper = mapperService.GetMapper<Renderable>();
            _chunksMapper = mapperService.GetMapper<Chunks>();

            _buffer = new DynamicVertexBuffer(_graphics, typeof(VertexPositionTextureLight),
                        (int)2e5, BufferUsage.WriteOnly);
        }

        public override void Draw(GameTime gameTime) {
            foreach (var entityId in ActiveEntities) {
                var renderable = _renderableMapper.Get(entityId);
                var chunks = _chunksMapper.Get(entityId);

                foreach (var chunk in chunks.LoadedChunks.Values) {
                    var chunk_pos = chunk.Position;
                    var world_pos = new Vector3(chunk_pos.X * 16, chunk_pos.Y, chunk_pos.Z * 16);
                    var model_matrix = Matrix.CreateScale(1f)
                            * Matrix.CreateRotationX(0)
                            * Matrix.CreateRotationY(0)
                            * Matrix.CreateRotationZ(0)
                            * Matrix.CreateTranslation(world_pos + new Vector3(0.5f, 0.5f, 0.5f));

                    Cubicle.Effect.World = renderable.World * model_matrix;
                    Cubicle.Effect.View = renderable.View;
                    Cubicle.Effect.Projection = renderable.Projection;
                    Cubicle.Effect.LightingEnabled = false;
                    Cubicle.Effect.Alpha = 0.75f;
                    Cubicle.Effect.VertexColorEnabled = true;

                    _buffer.SetData(chunk.Vertices);
                    _graphics.SetVertexBuffer(_buffer);

                    foreach (EffectPass pass in Cubicle.Effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, chunk.VertexCount / 3);
                    }
                }
            }
        }
    }
}
