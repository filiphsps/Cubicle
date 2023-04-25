using Cubicle.Gearset;
using Cubicle.Singletons;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities.Systems;
using Steamworks;

namespace Cubicle.Systems {
    public class SteamSystem : UpdateSystem {
        public SteamSystem(uint appId) {
            SteamClient.Init(appId);
        }

        public override void Update(GameTime gameTime) {
            GS.BeginMark("SteamSystem.Debug", Color.Purple);
            DebugManager.Text($"Steam: {SteamClient.AppId}, State: {SteamClient.State}");
            DebugManager.Text($"User: {SteamClient.SteamId}, LoggedIn: {SteamClient.IsLoggedOn}");
            DebugManager.Div();
            GS.EndMark("SteamSystem.Debug");
        }
    }
}
