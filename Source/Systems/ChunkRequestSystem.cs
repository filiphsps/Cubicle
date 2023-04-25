using Cubicle.Components;
using Cubicle.Gearset;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Threading;
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
            GS.BeginMark("ChunkRequestSystem", Color.Olive);
            var requester = _requesterMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);

            if (Monitor.TryEnter(requester.RequestedChunksLock, 0)) {
                Monitor.Exit(requester.RequestedChunksLock);

                lock (requester.RequestedChunksLock) {
                    var distance = 8;
                    var distance_y = 2;

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
                                var pos = new Vector3(x, y, z);

                                if (requester.RequestedChunks.Contains(pos))
                                    continue;

                                requester.RequestedChunks.Add(pos);
                            }
                        }
                    }

                }
            }
            GS.EndMark("ChunkRequestSystem");
        }
    }
}
