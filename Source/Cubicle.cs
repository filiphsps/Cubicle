using Cubicle.Components;
using Cubicle.Entities;
using Cubicle.Gearset;
using Cubicle.Singletons;
using Cubicle.Systems;
using Cubicle.Systems.Debug;
using Gearset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
        Entity _player;

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
            GS.Initialize(this, createUI: true);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.DepthClipEnable = true;
            GraphicsDevice.RasterizerState = rasterizerState;

            base.Initialize();
#if USE_GEARSET
            GearsetSettings.Instance.DepthBufferEnabled = true;
#endif

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

            // TODO: Proper entity manager
            _entityFactory.CreateDebugHandler();
            _entityFactory.CreateSettingsHandler();
            _entityFactory.CreateChunkHandler();
            _player = _entityFactory.CreatePlayer();

            GS.Log("Hello World!");
        }

        protected override void LoadContent() {
            DebugManager.Initialize(_graphics, GraphicsDevice);
            DebugManager.Font = Content.Load<SpriteFont>("Fonts/primary_font");

            Effect = new BasicEffect(GraphicsDevice);
            Viewport = GraphicsDevice.Viewport;

            TexturesManager.LoadContent(GraphicsDevice);
            BlocksManager.LoadContent();

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

        KeyboardState prevState;
        protected override void Update(GameTime gameTime) {
            GS.StartFrame();
            GS.BeginMark("Update", Color.Red);
            _world.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevState.IsKeyDown(Keys.Escape)) {
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P) && !prevState.IsKeyDown(Keys.P)) {
                _player.Get<Input>().Enabled = !_player.Get<Input>().Enabled;
            }

            prevState = Keyboard.GetState();
            GS.BeginMark("Base", Color.Cyan);
            base.Update(gameTime);
            GS.EndMark("Base");
            GS.EndMark("Update");
        }

        protected override void Draw(GameTime gameTime) {
            GS.Plot("FPS", 1.0f / gameTime.GetElapsedSeconds());

            GS.BeginMark("Draw", Color.Blue);
            _world.Draw(gameTime);

            if (!_player.Get<Input>().Enabled) {
                var batch = new SpriteBatch(GraphicsDevice);
                batch.Begin();
                batch.FillRectangle(new RectangleF(0, 0, Viewport.Width, Viewport.Height), new Color(0, 0, 0, 145));
                batch.End();
            }

            GS.BeginMark("Base", Color.Cyan);
            base.Draw(gameTime);
            GS.EndMark("Base");
            GS.EndMark("Draw");
        }

        protected override void OnDeactivated(Object game, EventArgs args) {
            base.OnDeactivated(game, args);
        }

        protected override void OnExiting(Object game, EventArgs args) {
            SteamClient.Shutdown();
            base.OnExiting(game, args);
            GS.Shutdown(this);
        }
    }
}
