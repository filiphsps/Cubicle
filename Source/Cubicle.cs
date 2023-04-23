using Cubicle.Entities;
using Cubicle.Singletons;
using Cubicle.Systems;
using Cubicle.Systems.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using Steamworks;
using System;

namespace Cubicle {
    public class Cubicle : Game {
        public uint AppID = 1882990;

        public static Viewport Viewport;
        // TODO: ShaderManager
        public static BasicEffect Effect;

        GraphicsDeviceManager _graphics;
        EntityFactory _entityFactory;
        World _world;

        RasterizerState _rasterizerFill;
        RasterizerState _rasterizerWire;

        public Cubicle() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1366,
                PreferredBackBufferHeight = 768,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = false
            };

            IsFixedTimeStep = false;
            IsMouseVisible = false;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (Object sender, EventArgs args) => {
                Viewport = GraphicsDevice.Viewport;
            };

            Content.RootDirectory = "Assets";
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            DebugManager.Initialize(_graphics, GraphicsDevice);
            DebugManager.Font = Content.Load<SpriteFont>("Fonts/primary_font");

            Effect = new BasicEffect(GraphicsDevice);
            Viewport = GraphicsDevice.Viewport;

            TexturesManager.LoadContent(GraphicsDevice);
            BlocksManager.LoadContent();

            _world = new WorldBuilder()
                .AddSystem(new DebugPrepareSystem())
                .AddSystem(new ChunkRequestSystem())
                .AddSystem(new ChunkLoaderSystem(GraphicsDevice))
                .AddSystem(new PrepareRenderSystem(GraphicsDevice))
                .AddSystem(new InputSystem(this))
                .AddSystem(new MovementSystem())
                .AddSystem(new CameraSystem())
                .AddSystem(new MeshRenderSystem(GraphicsDevice))
                .AddSystem(new ModelRenderSystem(GraphicsDevice))
                .AddSystem(new ChunkRenderSystem(GraphicsDevice))
                .AddSystem(new SteamSystem(AppID))
                .AddSystem(new DebugRenderSystem())
                .Build();

            _entityFactory = new EntityFactory(_world);

            _entityFactory.CreateDebugHandler();
            _entityFactory.CreateSettingsHandler();
            _entityFactory.CreateChunkHandler();
            _entityFactory.CreatePlayer();

            _rasterizerFill = GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.FillMode = FillMode.WireFrame;
            _rasterizerWire = rasterizerState;

            // Effect
            Cubicle.Effect.Alpha = 1f;
            Cubicle.Effect.VertexColorEnabled = true;

            Cubicle.Effect.EnableDefaultLighting();
            Cubicle.Effect.PreferPerPixelLighting = true;
            Cubicle.Effect.EmissiveColor = new Vector3(0.75f, 0.75f, 0.75f);
            Cubicle.Effect.AmbientLightColor = new Vector3(0.25f, 0.25f, 0.25f);
            Cubicle.Effect.DirectionalLight0.Enabled = false;
            Cubicle.Effect.DirectionalLight1.Enabled = false;
            Cubicle.Effect.DirectionalLight2.Enabled = false;
            Cubicle.Effect.LightingEnabled = true;

            Cubicle.Effect.TextureEnabled = true;
        }

        protected override void Update(GameTime gameTime) {
            _world.Update(gameTime);
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.R)) {
                if (GraphicsDevice.RasterizerState.FillMode == FillMode.WireFrame)
                    GraphicsDevice.RasterizerState = _rasterizerFill;
                else
                    GraphicsDevice.RasterizerState = _rasterizerWire;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }
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
