using Apos.Input;
using Cubicle.Components;
using Cubicle.Gearset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using KeyState = Cubicle.Components.KeyState;
using Vector2 = System.Numerics.Vector2;

namespace Cubicle.Systems {
    public class InputSystem : EntityUpdateSystem {
        Game _game;

        private ComponentMapper<Input> _inputMapper;

        ICondition _forward, _backward, _left, _right;
        ICondition _up, _down;

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

            _up = new KeyboardCondition(Keys.Space);
            _down = new KeyboardCondition(Keys.LeftControl);

            base.Initialize(world);
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _inputMapper = mapperService.GetMapper<Input>();
        }

        public override void Update(GameTime gameTime) {
            GS.BeginMark("InputSystem", Color.Aqua);
            InputHelper.UpdateSetup();

            foreach (var entityId in ActiveEntities) {
                // TODO: figure out if this is actually okay
                var input = _inputMapper.Get(entityId);
                var currentMouseState = Mouse.GetState();

                if (!input.Enabled) {
                    input.MousePointer = new Vector2(currentMouseState.X, currentMouseState.Y);
                    input.LastMousePointer = input.MousePointer;
                    break;
                }

                input.Forward = _forward.Held() ? KeyState.Pressed : KeyState.Released;
                input.Backward = _backward.Held() ? KeyState.Pressed : KeyState.Released;
                input.Left = _left.Held() ? KeyState.Pressed : KeyState.Released;
                input.Right = _right.Held() ? KeyState.Pressed : KeyState.Released;

                input.Up = _up.Held() ? KeyState.Pressed : KeyState.Released;
                input.Down = _down.Held() ? KeyState.Pressed : KeyState.Released;


                float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 20f;
                input.MouseDelta = delta * (new Vector2(currentMouseState.X, currentMouseState.Y) -
                    new Vector2(input.LastMousePointer.X, input.LastMousePointer.Y));
                input.MouseDelta = Vector2.Clamp(input.MouseDelta, new Vector2(-20, -20), new Vector2(20, 20));
                input.MouseDelta *= 2.5f; // Rotation speed

                if ((new Vector2(currentMouseState.X, currentMouseState.Y) - new Vector2(400, 300)).Length() > 200) {
                    Mouse.SetPosition(400, 300);
                    currentMouseState = Mouse.GetState();
                }

                input.MousePointer = new Vector2(currentMouseState.X, currentMouseState.Y);
                input.LastMousePointer = input.MousePointer;
            }

            InputHelper.UpdateCleanup();
            GS.EndMark("InputSystem");
        }
    }
}
