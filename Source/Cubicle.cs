using Cubicle.Entities;
using Cubicle.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using Steamworks;
using System;

namespace Cubicle {
    public class Cubicle : Game {
        public uint AppID = 1882990;

        // TODO: ShaderManager
        public static Effect Effect;
        public static Model Model;

        GraphicsDeviceManager _graphics;
        EntityFactory _entityFactory;
        World _world;

        public Cubicle() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
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
            Effect = Content.Load<Effect>("Shaders/Basic");
            Model = Content.Load<Model>("Models/Test");

            _world = new WorldBuilder()
                .AddSystem(new PrepareRenderSystem(GraphicsDevice))
                .AddSystem(new InputSystem(this))
                .AddSystem(new MovementSystem())
                .AddSystem(new CameraSystem())
                .AddSystem(new MeshRenderSystem(GraphicsDevice))
                .AddSystem(new ModelRenderSystem(GraphicsDevice))
                .AddSystem(new SteamSystem(AppID))
                .Build();

            _entityFactory = new EntityFactory(_world);

            _entityFactory.CreatePlayer();
            _entityFactory.CreateCube();
        }

        protected override void Update(GameTime gameTime) {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
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
