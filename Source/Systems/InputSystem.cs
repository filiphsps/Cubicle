using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Apos.Input;
using Track = Apos.Input.Track;

namespace Cubicle.Source.Systems {
    public class InputSystem : UpdateSystem {
        Game _game;

        public InputSystem(Game game) {
            _game = game; ;
        }

        public override void Initialize(World world) {
            InputHelper.Setup(_game);
            base.Initialize(world);
        }

        public override void Update(GameTime gameTime) {
            InputHelper.UpdateSetup();

            // TODO: Check input here

            InputHelper.UpdateCleanup();
        }
    }
}
