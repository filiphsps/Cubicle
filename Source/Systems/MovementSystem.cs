using Apos.Input;
using Cubicle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Cubicle.Systems {
    public class MovementSystem : EntityUpdateSystem {
        Game _game;

        private ComponentMapper<Input> _inputMapper;
        private ComponentMapper<Transform3> _transformMapper;

        public MovementSystem()
            : base(Aspect.All(typeof(Input), typeof(Transform3))) {
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _inputMapper = mapperService.GetMapper<Input>();
            _transformMapper = mapperService.GetMapper<Transform3>();
        }

        public override void Update(GameTime gameTime) {
            foreach (var entityId in ActiveEntities) {
                var input = _inputMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float speed = 20f;

                // TODO: Velocity
                if (input.Backward == Components.KeyState.Pressed)
                    transform.Position += new Vector3(0, 0, speed * delta);
                if (input.Forward == Components.KeyState.Pressed)
                    transform.Position += new Vector3(0, 0, -speed * delta);
                if (input.Right == Components.KeyState.Pressed)
                    transform.Position += new Vector3(speed * delta, 0, 0);
                if (input.Left == Components.KeyState.Pressed)
                    transform.Position += new Vector3(-speed * delta, 0, 0);
            }
        }
    }
}
