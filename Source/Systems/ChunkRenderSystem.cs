using Cubicle.Components;
using Cubicle.Singletons;
using Cubicle.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Linq;

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

                var camera_pos = renderable.CameraPosition;
                var direction = renderable.Direction;
                var cardinal = direction.Cardinal();

                var rendered = 0;
                foreach (var chunk in chunks.LoadedChunks.Values) {
                    if (!chunk.Blocks.Any())
                        continue;

                    var chunk_pos = chunk.Position;
                    var world_pos = new Vector3(chunk_pos.X * 16, chunk_pos.Y * 16, chunk_pos.Z * 16);

                    // Cull chunks behind the player
                    // TODO: properly account for FOV
                    if (cardinal == CardinalDirection.North) {
                        if (world_pos.Z > camera_pos.Z)
                            continue;
                    } else if (cardinal == CardinalDirection.South) {
                        if (world_pos.Z < camera_pos.Z - 16)
                            continue;
                    } else if (cardinal == CardinalDirection.West) {
                        if (world_pos.X > camera_pos.X)
                            continue;
                    } else if (cardinal == CardinalDirection.East) {
                        if (world_pos.X < camera_pos.X - 16)
                            continue;
                    }

                    var model_matrix = Matrix.CreateScale(1f)
                            * Matrix.CreateRotationX(0)
                            * Matrix.CreateRotationY(0)
                            * Matrix.CreateRotationZ(0)
                            * Matrix.CreateTranslation(world_pos + new Vector3(0.5f, 0.5f, 0.5f)); // FIXME: Should coords be centered?

                    Cubicle.Effect.World = renderable.World * model_matrix;
                    Cubicle.Effect.View = renderable.View;
                    Cubicle.Effect.Projection = renderable.Projection;
                    Cubicle.Effect.Texture = chunk.Texture;

                    chunk.Apply(_graphics);
                    foreach (EffectPass pass in Cubicle.Effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, chunk.VertexCount / 3);
                    }
                    rendered += 1;
                }

                var culled = 100 - (rendered / (float)chunks.LoadedChunks.Count) * 100;
                DebugManager.Text($"Drew {rendered}/{chunks.LoadedChunks.Count} chunks", true);
                DebugManager.Text($"{culled.ToString("0.00")}% Culling", true);
                DebugManager.Div();
            }
        }
    }
}
