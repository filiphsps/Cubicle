using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities.Systems;
using Steamworks;

namespace Cubicle.Source.Systems {
    public class SteamSystem : UpdateSystem {
        public SteamSystem(uint appId) {
            SteamClient.Init(appId);
        }

        public override void Update(GameTime gameTime) { }
    }
}
