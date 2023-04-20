using Cubicle.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using Steamworks;
using System;
using System.IO;

namespace Cubicle {
    public class Cubicle : Game {
        public uint AppID = 1882990;

        GraphicsDeviceManager _graphics;
        World _world;

        public Cubicle() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 854,
                PreferredBackBufferHeight = 480,
                IsFullScreen = false,
            };

            IsFixedTimeStep = true;
            IsMouseVisible = true;

            Window.AllowUserResizing = true;

            Content.RootDirectory = "Assets";
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            _world = new WorldBuilder()
                .AddSystem(new InputSystem(this))
                .AddSystem(new SteamSystem(AppID))
                .AddSystem(new RenderSystem(GraphicsDevice))
                .Build();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            _world.Draw(gameTime);
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
