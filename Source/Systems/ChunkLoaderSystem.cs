using Cubicle.Components;
using Cubicle.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Linq;

namespace Cubicle.Systems {
    public class ChunkLoaderSystem : EntityProcessingSystem {
        ComponentMapper<Chunks> _chunksMapper;
        ComponentMapper<ChunkRequester> _requesterMapper;
        private GraphicsDevice _graphics;

        public ChunkLoaderSystem(GraphicsDevice graphics)
            : base(Aspect.All(typeof(Chunks), typeof(ChunkRequester))) {
            _graphics = graphics;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _chunksMapper = mapperService.GetMapper<Chunks>();
            _requesterMapper = mapperService.GetMapper<ChunkRequester>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            var chunks = _chunksMapper.Get(entityId);
            var requester = _requesterMapper.Get(entityId);

            foreach (var position in requester.RequestedChunks) {
                if (chunks.LoadedChunks.ContainsKey(position))
                    continue;

                var chunk = new Chunk(_graphics) {
                    Position = position
                };
                chunk.Generate();

                chunks.LoadedChunks.Add(position, chunk);
            }

            var correct = chunks.LoadedChunks
                .Where(x => !requester.RequestedChunks.Any(y => y == x.Key)).ToList();

            foreach (var chunk in correct) {
                chunks.LoadedChunks.Remove(chunk.Key);
            }

            requester.RequestedChunks.Clear();
        }
    }
}
