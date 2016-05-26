using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        private GraphicsDeviceManager graphics;

        private Drawer drawer;
        private Scene currentScene;
        private PlayerControls playerControls;
        private SoundManager soundManager;
        private Cursor cursor;
        private Camera2D camera;
        private SavedState state;
        private float deltaTime;
        private static GameWorld instance;

        public static PlayerControls PlayerControls { get { return Instance.playerControls; } }
        public static SoundManager SoundManager { get { return Instance.soundManager; } }
        public static Cursor Cursor { get { return Instance.cursor; } }
        public static Camera2D Camera { get { return Instance.camera; } }
        public static SavedState State { get { return Instance.state; } }
        public static float DeltaTime { get { return Instance.deltaTime; } }
        public static GraphicsDeviceManager Graphics { get { return Instance.graphics; } }
        public static Scene Scene { get { return Instance.currentScene; } }
        public float HalfViewPortWidth { get { return GraphicsDevice.Viewport.Width * 0.5f; } }
        public float HalfViewPortHeight { get { return GraphicsDevice.Viewport.Height * 0.5f; } }
        public Vector2 ScalingVector { get; set; }

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

            //Create 4 test spell for both the bar and the book
            for (int i = 1; i < 5; i++)
            {
                SpellInfo ps = new SpellInfo();
                ps.SetBaseRune(i);
                for (int j = 0; j < i; j++)
                {
                    ps.SetAttributeRune(j, 0);
                }
                state.SpellBook.Add(ps);
                state.SpellBar.Add(state.SpellBook.IndexOf(ps));
            }
            currentScene = new MenuScene();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new Drawer, which can be used to draw textures.
            drawer = new Drawer(GraphicsDevice);
            cursor.LoadContent(Content);
            soundManager.LoadContent(Content);
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
            Instance.currentScene = targetScene;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            cursor.Update();

            if (currentScene is MenuScene || currentScene is PauseScene || currentScene is SettingsScene || currentScene is SpellbookScene)
            {
                SoundManager.PlayMusic("HubMusic");
                SoundManager.StopSound("AmbienceSound");
            }
            if (currentScene is LobbyScene || currentScene is HallwayScene)
            {
                SoundManager.PlayMusic("HubMusic");
                SoundManager.PlaySound("AmbienceSound");
            }
            if (currentScene is GameScene)
            {
                SoundManager.PlayMusic("CombatMusic");
                SoundManager.PlaySound("AmbienceSound");
            }

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var s = Mouse.GetState();
            if (!Cursor.CanClick && s.LeftButton == ButtonState.Released && s.RightButton == ButtonState.Released)
            {
                Cursor.CanClick = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            currentScene.Update();

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

            currentScene.Draw(drawer);
            cursor.Draw(drawer[DrawLayer.Cursor]);

            drawer.EndBatches();

            base.Draw(gameTime);
        }
    }
}