using Cubicle.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Systems {
    public class ChunkRequestSystem : EntityProcessingSystem {
        ComponentMapper<ChunkRequester> _requesterMapper;
        ComponentMapper<Transform> _transformMapper;

        public ChunkRequestSystem()
            : base(Aspect.All(typeof(ChunkRequester), typeof(Transform))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _requesterMapper = mapperService.GetMapper<ChunkRequester>();
            _transformMapper = mapperService.GetMapper<Transform>();
        }

        public override void Process(GameTime gameTime, int entityId) {
            var requester = _requesterMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);

            // TODO: do this properly
            if (requester.RequestedChunks.Count > 0)
                return;

            var distance = 6;
            var distance_y = 1;

            var center_x = (int)transform.Position.X >> 4;
            var center_z = (int)transform.Position.Z >> 4;
            var center_y = (int)transform.Position.Y >> 4;

            var start_x = (int)center_x + -distance;
            var end_x = (int)center_x + distance;
            var start_z = (int)center_z + -distance;
            var end_z = (int)center_z + distance;

            var start_y = (int)center_y + -distance_y;
            var end_y = (int)center_y + distance_y;

            for (var x = start_x; x < end_x; x++) {
                for (var z = start_z; z < end_z; z++) {
                    for (var y = start_y; y < end_y; y++) {
                        requester.RequestedChunks.Add(new Vector3(x, y, z));
                    }
                }
            }
        }
    }
}
