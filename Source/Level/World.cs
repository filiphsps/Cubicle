using Cubicle.Components;
using Cubicle.Gearset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Level {
    public class WorldManager {
        private readonly World _world;
        private GraphicsDevice _graphics;
        private ChunkRequester _chunkRequester;
        private Chunks _chunks;

        private bool _preparingChunks;
        private List<Vector3> _requested;

        public WorldManager(World world, GraphicsDevice graphics) {
            _world = world;
            _graphics = graphics;
            _chunkRequester = new ChunkRequester() { };
            _chunks = new Chunks() { };
        }

        public Entity CreateChunkHandler() {
            Entity entity = _world.CreateEntity();
            entity.Attach(_chunkRequester);
            entity.Attach(_chunks);
            return entity;
        }

        public void AttachChunksComponent(Entity entity) {
            entity.Attach(_chunks);
        }
        public void AttachChunkRequesterComponent(Entity entity) {
            entity.Attach(_chunkRequester);
        }

        public void Update(GameTime gameTime) {
            // TODO: Figure out a proper multi-threaded way
            //  or maybe this is the best way?
            if (_preparingChunks || _chunkRequester.RequestedChunks.Count <= 0) {
                return;
            }

            _preparingChunks = true;
            _requested = _chunkRequester.RequestedChunks;
            new Task(() => {
                lock (_chunkRequester.RequestedChunksLock) {
                    var added = 0;
                    Parallel.ForEach(_requested,
                       position => {
                           // FIXME: Check if chunk needs update.
                           if (_chunks.LoadedChunks.ContainsKey(position))
                               return;

                           var chunk = new Chunk(_graphics) {
                               Position = position
                           };

                           chunk.Generate();

                           //Don't add empty chunks
                           if (chunk.Empty())
                               return;

                           try {
                               // TODO: TextureManager needs the main UI thread.
                               chunk.GenerateMesh();
                           } catch (Exception ex) {
                               GS.Log(ex.ToString());
                           }

                           _chunks.LoadedChunks.TryAdd(position, chunk);
                           added += 1;
                       });

                    if (added > 0) {
                        // FIXME: Handle unloading in a better way.
                        // Remove non-requested
                        Parallel.ForEach(_chunks.LoadedChunks.Keys,
                           position => {
                               if (_requested.Contains(position))
                                   return;

                               // FIXME: Error handling
                               _chunks.LoadedChunks.TryRemove(position, out _);
                           });

                        _chunkRequester.RequestedChunks.Clear();
                        GS.Log("WorldManager", "Loaded Chunks ({0})", added);
                    }
                }
                _preparingChunks = false;
            }).Start();
        }
    }
}
