using Cubicle.Components;
using Cubicle.Gearset;
using Cubicle.Level;
using MonoGame.Extended.Entities;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Entities {
    public class EntityFactory {
        private readonly World _world;
        private readonly WorldManager _worldManager;
        private Renderable _renderable;

        public EntityFactory(World world, WorldManager worldManager) {
            _world = world;
            _worldManager = worldManager;

            _renderable = new Renderable() { };
        }

        public Entity CreatePlayer() {
            Entity entity = _world.CreateEntity();
            var transform = new Transform() { Position = new Vector3(0, 45, 0) };
            entity.Attach(transform);
            entity.Attach(_renderable);
            entity.Attach(new Camera());
            entity.Attach(new Input());
            entity.Attach(new Player());
            _worldManager.AttachChunkRequesterComponent(entity);

            GS.RegisterCommand("tp", "tp <x> <y> <z>", (host, command, args) => {
                transform.Position.X = float.Parse(args[0]);
                if (args.Count == 1) {
                    transform.Position.Y = float.Parse(args[1]);
                    if (args.Count == 2) {
                        transform.Position.Z = float.Parse(args[2]);
                    }
                }
                host.Echo($"Teleported player to: {transform.Position}");
            });

            return entity;
        }

        public Entity CreateDebugHandler() {
            Entity entity = _world.CreateEntity();
            entity.Attach(new Debug());

            return entity;
        }

        public Entity CreateSettingsHandler() {
            Entity entity = _world.CreateEntity();

            return entity;
        }
    }
}
