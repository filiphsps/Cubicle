using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Cubicle.Systems.Debug {
    public class DebugPrepareSystem : DrawSystem {
        public DebugPrepareSystem() {
        }

        public override void Initialize(World world) {
            DebugManager.SetWorld(world);
        }

        public override void Draw(GameTime gameTime) {
            DebugManager.BeginDraw();
        }
    }
}
