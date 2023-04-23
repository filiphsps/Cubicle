﻿using Cubicle.Components;
using MonoGame.Extended.Entities;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Entities {
    public class EntityFactory {
        private readonly World _world;
        Renderable _renderable;
        ChunkRequester _chunkRequester;

        public EntityFactory(World world) {
            _world = world;

            _renderable = new Renderable() { };
            _chunkRequester = new ChunkRequester() { };
        }

        public Entity CreatePlayer() {
            Entity entity = _world.CreateEntity();
            entity.Attach(new Transform() { Position = new Vector3(0, 10, 0) });
            entity.Attach(_renderable);
            entity.Attach(new Camera());
            entity.Attach(new Input());
            entity.Attach(new Player());
            entity.Attach(_chunkRequester);

            return entity;
        }

        public Entity CreateDebugHandler() {
            Entity entity = _world.CreateEntity();
            entity.Attach(new Debug());

            return entity;
        }

        public Entity CreateChunkHandler() {
            Entity entity = _world.CreateEntity();
            entity.Attach(_renderable);
            entity.Attach(_chunkRequester);
            entity.Attach(new Chunks());

            return entity;
        }

        public Entity CreateSettingsHandler() {
            Entity entity = _world.CreateEntity();

            return entity;
        }
    }
}
