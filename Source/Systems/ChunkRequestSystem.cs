using Cubicle.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class ChunkRequestSystem : EntityProcessingSystem {
        ComponentMapper<ChunkRequester> _requesterMapper;
        ComponentMapper<Player> _playerMapper;

        public ChunkRequestSystem()
            : base(Aspect.All(typeof(ChunkRequester), typeof(Player))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _requesterMapper = mapperService.GetMapper<ChunkRequester>();
            _playerMapper = mapperService.GetMapper<Player>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            var requester = _requesterMapper.Get(entityId);
            var player = _playerMapper.Get(entityId);

            // TODO: do this properly
            if (requester.RequestedChunks.Count > 0)
                return;

            for (var x = -2; x < 2; x++) {
                for (var z = -2; z < 2; z++) {
                    requester.RequestedChunks.Add(new Vector3(x, 0, z));
                }
            }
        }
    }
}
