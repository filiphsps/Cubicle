using Cubicle.Components;
using Cubicle.Gearset;
using Cubicle.Singletons;
using Cubicle.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

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
            GS.BeginMark("ChunkRenderSystem", Color.DarkRed);
            var chunks_n = 0;
            var rendered = 0;
            foreach (var entityId in ActiveEntities) {
                GS.BeginMark("ChunkRenderSystem.Chunks", Color.IndianRed);
                var renderable = _renderableMapper.Get(entityId);
                var chunks = _chunksMapper.Get(entityId);

                var camera_pos = renderable.CameraPosition;
                var direction = renderable.Direction;
                var cardinal = direction.Cardinal();

                foreach (var chunk in chunks.LoadedChunks.Values) {
                    GS.BeginMark("ChunkRenderSystem.Chunks.Chunk", Color.PaleVioletRed);
                    chunks_n += 1;

                    if (chunk.VertexCount <= 0 || chunk.Empty()) {
                        GS.EndMark("ChunkRenderSystem.Chunks.Chunk");
                        continue;
                    }

                    var chunk_pos = chunk.Position;
                    var world_pos = new Vector3(chunk_pos.X * 16, chunk_pos.Y * 16, chunk_pos.Z * 16);

                    // Cull chunks behind the player
                    // TODO: properly account for FOV
                    var exit = false;
                    if (cardinal == CardinalDirection.North) {
                        if (world_pos.Z > camera_pos.Z)
                            exit = true;
                    } else if (cardinal == CardinalDirection.South) {
                        if (world_pos.Z < camera_pos.Z - 16)
                            exit = true;
                    } else if (cardinal == CardinalDirection.West) {
                        if (world_pos.X > camera_pos.X)
                            exit = true;
                    } else if (cardinal == CardinalDirection.East) {
                        if (world_pos.X < camera_pos.X - 16)
                            exit = true;
                    }

                    if (exit) {
                        GS.EndMark("ChunkRenderSystem.Chunks.Chunk");
                        continue;
                    }

                    // TODO: Should coords be centered?
                    var model_matrix = renderable.World * Matrix.CreateTranslation(world_pos);

                    Cubicle.Effect.World = renderable.World * model_matrix;
                    Cubicle.Effect.View = renderable.View;
                    Cubicle.Effect.Projection = renderable.Projection;
                    Cubicle.Effect.Texture = chunk.Texture;

                    GS.BeginMark("ChunkRenderSystem.Chunks.Draw", Color.IndianRed);
                    chunk.Apply(_graphics);
                    foreach (EffectPass pass in Cubicle.Effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        _graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, chunk.VertexCount / 3);
                    }
                    GS.EndMark("ChunkRenderSystem.Chunks.Draw");

                    rendered += 1;
                    GS.EndMark("ChunkRenderSystem.Chunks.Chunk");
                }

                GS.BeginMark("ChunkRenderSystem.Debug", Color.Purple);
                var culled = 100 - (rendered / (float)chunks.LoadedChunks.Count) * 100;
                DebugManager.Text($"Drew {rendered}/{chunks.LoadedChunks.Count} chunks", true);
                DebugManager.Text($"{culled.ToString("0.00")}% Culling", true);
                DebugManager.Div();
                GS.EndMark("ChunkRenderSystem.Debug");
                GS.EndMark("ChunkRenderSystem.Chunks");
            }
            GS.EndMark("ChunkRenderSystem");
            GS.Plot("Chunks", chunks_n, 256);
            GS.Plot("Chunks Culled", chunks_n - rendered, 256);
        }
    }
}
