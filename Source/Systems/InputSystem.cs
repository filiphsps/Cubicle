using Apos.Input;
using Cubicle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using KeyState = Cubicle.Components.KeyState;

namespace Cubicle.Systems {
    public class InputSystem : EntityUpdateSystem {
        Game _game;

        private ComponentMapper<Input> _inputMapper;

        ICondition _forward, _backward, _left, _right;

        public InputSystem(Game game)
            : base(Aspect.All(typeof(Input))) {
            _game = game; ;
        }

        public override void Initialize(World world) {
            InputHelper.Setup(_game);

            _forward = new KeyboardCondition(Keys.W);
            _backward = new KeyboardCondition(Keys.S);
            _left = new KeyboardCondition(Keys.A);
            _right = new KeyboardCondition(Keys.D);

            base.Initialize(world);
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _inputMapper = mapperService.GetMapper<Input>();
        }

        public override void Update(GameTime gameTime) {
            InputHelper.UpdateSetup();

            foreach (var entityId in ActiveEntities) {
                // TODO: figre out if this is actually okay
                var input = _inputMapper.Get(entityId);

                input.Forward = _forward.Pressed() ? KeyState.Pressed : KeyState.Released;
                input.Backward = _backward.Pressed() ? KeyState.Pressed : KeyState.Released;
                input.Left = _left.Pressed() ? KeyState.Pressed : KeyState.Released;
                input.Right = _right.Pressed() ? KeyState.Pressed : KeyState.Released;

                _inputMapper.Put(entityId, input);
            }

            InputHelper.UpdateCleanup();
        }
    }
}
