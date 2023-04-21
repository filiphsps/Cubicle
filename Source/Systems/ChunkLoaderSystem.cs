using Cubicle.Components;
using Cubicle.Level;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class ChunkLoaderSystem : EntityProcessingSystem {
        ComponentMapper<Chunks> _chunksMapper;
        ComponentMapper<ChunkRequester> _requesterMapper;

        public ChunkLoaderSystem()
            : base(Aspect.All(typeof(Chunks), typeof(ChunkRequester))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _chunksMapper = mapperService.GetMapper<Chunks>();
            _requesterMapper = mapperService.GetMapper<ChunkRequester>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            var chunks = _chunksMapper.Get(entityId);
            var requester = _requesterMapper.Get(entityId);

            if (chunks.LoadedChunks.Count > 0)
                return;

            // TODO
            var blocks = new List<Block>();
            for (var x = 0; x < 16; x++) {
                for (var y = 0; y < 1; y++) {
                    for (var z = 0; z < 16; z++) {
                        blocks.Add(new Block() { Position = new Vector3(x, y, z) });
                    }
                }
            }

            foreach (var position in requester.RequestedChunks) {
                chunks.LoadedChunks.Add(new Chunk() {
                    Position = position,
                    Blocks = blocks
                });
            }

            requester.RequestedChunks.Clear();
        }
    }
}
