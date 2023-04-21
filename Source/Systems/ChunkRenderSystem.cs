using Cubicle.Components;
using Cubicle.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;

namespace Cubicle.Systems {
    public class ChunkRenderSystem : EntityDrawSystem {
        GraphicsDevice _graphics;
        ComponentMapper<Renderable> _renderableMapper;
        ComponentMapper<Chunks> _chunksMapper;

        public ChunkRenderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Renderable), typeof(Chunks))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _renderableMapper = mapperService.GetMapper<Renderable>();
            _chunksMapper = mapperService.GetMapper<Chunks>();
        }

        public override void Draw(GameTime gameTime) {
            foreach (var entityId in ActiveEntities) {
                var renderable = _renderableMapper.Get(entityId);
                var chunks = _chunksMapper.Get(entityId);
                var model = Cubicle.Model;

                foreach (var chunk in chunks.LoadedChunks.Values) {
                    var chunk_pos = chunk.Position;
                    var world_pos = new Vector3(chunk_pos.X * 16, chunk_pos.Y, chunk_pos.Z * 16);

                    // TODO: Generate mesh insteard
                    foreach (var block in chunk.Blocks) {
                        var block_position = world_pos + block.Position;

                        var model_matrix = Matrix.CreateScale(0.01f)
                            * Matrix.CreateRotationX(0)
                            * Matrix.CreateRotationY(0)
                            * Matrix.CreateRotationZ(0)
                            * Matrix.CreateTranslation(block_position + new Vector3(0f, -8f, -25f));
                        model.Draw(renderable.World * model_matrix, renderable.View, renderable.Projection);
                    }
                }
            }
        }
    }
}
