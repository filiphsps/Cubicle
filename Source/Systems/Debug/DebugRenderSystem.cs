using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Cubicle.Systems.Debug {
    public class DebugRenderSystem : DrawSystem {
        public DebugRenderSystem() {
        }

        public override void Initialize(World world) {
            DebugManager.SetWorld(world);
        }

        public override void Draw(GameTime gameTime) {
            DebugManager.EndDraw();
        }
    }
}
