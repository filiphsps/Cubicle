using Cubicle.Components;
using Cubicle.Level;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Collections.Generic;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class ChunkLoaderSystem : EntityProcessingSystem {
        ComponentMapper<Chunks> _chunksMapper;
        ComponentMapper<ChunkRequester> _requesterMapper;

        List<Block> _blocks;

        public ChunkLoaderSystem()
            : base(Aspect.All(typeof(Chunks), typeof(ChunkRequester))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _chunksMapper = mapperService.GetMapper<Chunks>();
            _requesterMapper = mapperService.GetMapper<ChunkRequester>();

            _blocks = new List<Block>();
            for (var x = 0; x < 16; x++) {
                for (var y = 0; y < 4; y++) {
                    for (var z = 0; z < 16; z++) {
                        _blocks.Add(new Block() { Position = new Vector3(x, y, z) });
                    }
                }
            }
        }

        public override void Process(GameTime gameTime, int entityId) {
            var chunks = _chunksMapper.Get(entityId);
            var requester = _requesterMapper.Get(entityId);

            // TODO: Unloading

            foreach (var position in requester.RequestedChunks) {
                if (chunks.LoadedChunks.ContainsKey(position))
                    continue;

                var chunk = new Chunk() {
                    Position = position,
                    Blocks = _blocks
                };
                chunk.CalculateMesh();

                chunks.LoadedChunks.Add(position, chunk);
            }

            requester.RequestedChunks.Clear();
        }
    }
}
