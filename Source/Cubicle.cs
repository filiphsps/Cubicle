using Cubicle.Components;
using Cubicle.Entities;
using Cubicle.Gearset;
using Cubicle.Level;
using Cubicle.Singletons;
using Cubicle.Systems;
using Cubicle.Systems.Debug;
using Gearset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using Steamworks;
using System;
using System.Diagnostics;
#if USE_GEARSET
#endif

namespace Cubicle {
    public class Cubicle : Game {
        public uint AppID = 1882990;

        public static Viewport Viewport;
        // TODO: ShaderManager
        public static BasicEffect Effect;

        GraphicsDeviceManager _graphics;
        WorldManager _worldManager;
        EntityFactory _entityFactory;
        World _world;
        Entity _player, _chunkHandler;

        public Cubicle() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1366,
                PreferredBackBufferHeight = 768,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = false,
                PreferMultiSampling = false,
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
            var watch = Stopwatch.StartNew();
            GS.Initialize(this, createUI: true);
            base.Initialize();

#if USE_GEARSET
            GearsetSettings.Instance.SaveFrequency = float.MaxValue;
            GearsetSettings.Instance.DataSamplerConfig.DefaultHistoryLength = 1024;
            GearsetSettings.Instance.DepthBufferEnabled = true;

            GearsetSettings.Instance.FinderConfig.Enabled = false;
            GearsetSettings.Instance.FinderConfig.Visible = false;

            GearsetSettings.Instance.BenderConfig.Enabled = false;
            GearsetSettings.Instance.BenderConfig.Visible = false;

            //GearsetSettings.Instance.ProfilerConfig.Visible = fals

            GS.Action(() => {
                GS.GearsetComponent.Console.MemoryMonitor.MemoryGraph.Visible = true;
                GS.GearsetComponent.Console.MemoryMonitor.MonitorXbox360Garbage = false;
                GS.GearsetComponent.Console.MemoryMonitor.SkipFrames = 15;
                GS.GearsetComponent.Console.MemoryMonitor.MemoryGraph.TopLeft = new Vector2(10, 90);
                GS.GearsetComponent.Console.MemoryMonitor.MemoryGraph.Size = new Vector2(290, 75);

                GS.GearsetComponent.Console.Profiler.PerformanceGraph.Visible = true;
                GS.GearsetComponent.Console.Profiler.PerformanceGraph.VisibleLevelsFlags = 7;
                GS.GearsetComponent.Console.Profiler.PerformanceGraph.SkipFrames = 15;
                GS.GearsetComponent.Console.Profiler.PerformanceGraph.TopLeft = new Vector2(20 + 290, 90);
                GS.GearsetComponent.Console.Profiler.PerformanceGraph.Size = new Vector2(170, 75);

                GS.GearsetComponent.Console.Profiler.TimeRuler.Visible = true;
                GS.GearsetComponent.Console.Profiler.TimeRuler.VisibleLevelsFlags = 1;
                GS.GearsetComponent.Console.Profiler.TimeRuler.TopLeft = new Vector2(10, 180);
                GS.GearsetComponent.Console.Profiler.TimeRuler.Size = new Vector2(470, 50);

                GS.GearsetComponent.Console.Profiler.ProfilerSummary.Visible = true;
                GS.GearsetComponent.Console.Profiler.ProfilerSummary.VisibleLevelsFlags = byte.MaxValue;
                GS.GearsetComponent.Console.Profiler.ProfilerSummary.TopLeft = new Vector2(10, 180 + 30);
                GS.GearsetComponent.Console.Profiler.ProfilerSummary.Size = new Vector2(470, 150);
            });

#endif

            _world = new WorldBuilder()
                .AddSystem(new DebugPrepareSystem())
                .AddSystem(new ChunkRequestSystem())
                .AddSystem(new PrepareRenderSystem(GraphicsDevice))
                .AddSystem(new InputSystem(this))
                .AddSystem(new MovementSystem())
                .AddSystem(new CameraSystem())
                .AddSystem(new ChunkRenderSystem(GraphicsDevice))
                //.AddSystem(new SteamSystem(AppID))
                .AddSystem(new DebugRenderSystem())
                .Build();

            _worldManager = new WorldManager(_world, GraphicsDevice);
            _entityFactory = new EntityFactory(_world, _worldManager);

            _chunkHandler = _worldManager.CreateChunkHandler();

            // TODO: Proper entity manager
            _entityFactory.CreateDebugHandler();
            _entityFactory.CreateSettingsHandler();

            _player = _entityFactory.CreatePlayer();

            // FIXME: Do this properly.
            _chunkHandler.Attach(_player.Get<Renderable>());

            // TODO: Move this back into the World
            _ = new SteamSystem(AppID);
        }

        protected override void LoadContent() {
            var watch = Stopwatch.StartNew();
            DebugManager.Initialize(_graphics, GraphicsDevice);
            DebugManager.Font = Content.Load<SpriteFont>("Fonts/primary_font");

            Effect = new BasicEffect(GraphicsDevice);
            Viewport = GraphicsDevice.Viewport;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.DepthClipEnable = true;
            GraphicsDevice.RasterizerState = rasterizerState;

            BlocksManager.LoadContent();
            TexturesManager.LoadContent(GraphicsDevice);

            // Effect
            {
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
        }

        KeyboardState prevState;
        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            GS.StartFrame();
            GS.BeginMark("Update", Color.Red);

            _worldManager.Update(gameTime);
            _world.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevState.IsKeyDown(Keys.Escape)) {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && !prevState.IsKeyDown(Keys.P)) {
                Pause(_player.Get<Input>().Enabled);
            }
            if (_player.Get<Input>().Enabled && !IsActive) {
                Pause(true);
            }


            var pos = _player.Get<Transform>().Position;
            var chunk = new Vector3((int)pos.X >> 4, (int)pos.Y >> 4, (int)pos.Z >> 4);
            GS.ShowBoxOnce(chunk, chunk + new Vector3(16, 16, 16), Color.Red);

            prevState = Keyboard.GetState();
            GS.EndMark("Update");
        }

        protected override void Draw(GameTime gameTime) {
            GS.BeginMark("Draw", Color.Blue);
            _world.Draw(gameTime);

            if (!_player.Get<Input>().Enabled) {
                var batch = new SpriteBatch(GraphicsDevice);
                batch.Begin();
                batch.FillRectangle(new RectangleF(0, 0, Viewport.Width, Viewport.Height), new Color(0, 0, 0, 145));
                batch.End();
            }

            base.Draw(gameTime);
            GS.Plot("FPS", 1.0f / gameTime.GetElapsedSeconds());
            GS.EndMark("Draw");
        }

        protected override void OnDeactivated(Object game, EventArgs args) {
            if (_player != null)
                _player.Get<Input>().Enabled = false;
            base.OnDeactivated(game, args);
        }

        protected override void OnExiting(Object game, ExitingEventArgs args) {
            SteamClient.Shutdown();
            base.OnExiting(game, args);
            GS.Shutdown(this);
        }

        private void Pause(bool pause) {
            _player.Get<Input>().Enabled = !pause;
            IsMouseVisible = pause;
        }
    }
}
