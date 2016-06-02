using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace BattleMages
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        //Constants
        public const int GameWidth = 320;

        public const int GameHeight = 180;

        //Fields

        private Drawer drawer;

        //Subsystems
        private Scene scene;

        private PlayerControls playerControls;
        private SoundManager soundManager;
        private Cursor cursor;
        private Camera2D camera;
        private SavedState state;
        private float deltaTime;
        private GraphicsDeviceManager graphics;
        public int ResScreenWidth { get; set; }
        public int ResScreenHeight { get; set; }
        private Random random = new Random();
        public static Scene Scene { get { return Instance.scene; } }
        public static PlayerControls PlayerControls { get { return Instance.playerControls; } }
        public static SoundManager SoundManager { get { return Instance.soundManager; } }
        public static Cursor Cursor { get { return Instance.cursor; } }
        public static Camera2D Camera { get { return Instance.camera; } }
        public static SavedState State { get { return Instance.state; } }
        public static float DeltaTime { get { return Instance.deltaTime; } }
        public static GraphicsDeviceManager Graphics { get { return Instance.graphics; } }
        public static Random Random { get { return Instance.random; } }

        //Misc properties
        public float HalfViewPortWidth { get { return GraphicsDevice.Viewport.Width * 0.5f; } }

        public float HalfViewPortHeight { get { return GraphicsDevice.Viewport.Height * 0.5f; } }
        public Vector2 ScalingVector { get; set; }

        //Singleton pattern
        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        /// <summary>
        /// Constructor for the GameWorld
        /// </summary>
        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        }

        /// <summary>
        /// Shorthand method for loading content from the GameWorld instance's content manager
        /// </summary>
        public static T Load<T>(string assetName)
        {
            return Instance.Content.Load<T>(assetName);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ScalingVector = new Vector2(Utils.CalculateWidthScale(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width),
                Utils.CalculateHeightScale(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));

            playerControls = new PlayerControls();
            soundManager = new SoundManager();
            cursor = new Cursor();
            camera = new Camera2D();
            state = new SavedState();
            drawer = new Drawer(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new Drawer, which can be used to draw textures.
            StaticData.LoadContent();
            cursor.LoadContent(Content);
            soundManager.LoadContent(Content);

            scene = new MenuScene();
            ResScreenWidth = GraphicsDevice.Viewport.Width;
            ResScreenHeight = GraphicsDevice.Viewport.Height;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static void ChangeScene(Scene targetScene)
        {
            Instance.scene = targetScene;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            cursor.Update();
            soundManager.Update();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            scene.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>ProcessObjectLists
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            drawer.Matrix = camera.ViewMatrix;
            drawer.BeginBatches();

            state.Draw(drawer);

            scene.Draw(drawer);
            cursor.Draw(drawer[DrawLayer.Cursor]);

            drawer.EndBatches();

            base.Draw(gameTime);
        }
    }
}