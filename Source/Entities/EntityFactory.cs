using Cubicle.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Entities {
    public class EntityFactory {
        private readonly World _world;

        public EntityFactory(World world) {
            _world = world;
        }

        public Entity CreatePlayer() {
            Entity entity = _world.CreateEntity();
            entity.Attach(new Transform3(new Vector3(10, 0, 10)));
            entity.Attach(new Renderable() { Effect = Cubicle.Effect });
            entity.Attach(new Camera() {
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(70),
                (float)800 / 600, 0.1f, 100f) // FIXME
            });
            entity.Attach(new Input());
            entity.Attach(new Player());

            return entity;
        }

        public Entity CreateCube() {
            Entity entity = _world.CreateEntity();
            entity.Attach(new Transform3(new Vector3(0, 0, 0)));
            entity.Attach(new Renderable() { Effect = Cubicle.Effect });
            entity.Attach(new Mesh());

            return entity;
        }
    }
}
