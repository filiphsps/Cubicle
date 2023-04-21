using Cubicle.Components;
using Cubicle.Level;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

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

            // TODO: Unloading

            foreach (var position in requester.RequestedChunks) {
                if (chunks.LoadedChunks.ContainsKey(position))
                    continue;

                var chunk = new Chunk() {
                    Position = position
                };
                chunk.Generate();

                chunks.LoadedChunks.Add(position, chunk);
            }

            requester.RequestedChunks.Clear();
        }
    }
}
