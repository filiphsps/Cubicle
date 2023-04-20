using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.IO;

namespace Cubicle {
    public class Cubicle : Game {
        // TODO: Create proper steam class
        public uint AppID;

        GraphicsDeviceManager _graphics;

        public Cubicle() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 854,
                PreferredBackBufferHeight = 480,
                IsFullScreen = false,
            };

            IsFixedTimeStep = true;
            IsMouseVisible = true;

            Content.RootDirectory = "Assets";
        }

        protected override void Initialize() {
            AppID = Convert.ToUInt32(File.ReadAllText("steam_appid.txt").TrimEnd('\r', '\n'), 10);
            SteamClient.Init(AppID);

            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        protected override void UnloadContent() {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }

        protected override void OnDeactivated(Object game, EventArgs args) {
            base.OnDeactivated(game, args);
        }

        protected override void OnExiting(Object game, EventArgs args) {
            SteamClient.Shutdown();
            base.OnExiting(game, args);
        }
    }
}
