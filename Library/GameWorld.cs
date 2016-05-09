using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battle_Mages
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        //Fields
        private GraphicsDeviceManager graphics;

        private bool paused;
        private KeyboardState keyState, oldKeyState;
        private Drawer drawer;
        private GameObject player;
        private Director director;
        private GameState currentGameState = GameState.MainMenu;

        private PlayerControls playerControls;
        private SoundManager soundManager;
        private MenuScreenManager menuScreenManager;
        private Cursor cursor;
        private Camera2D camera;
        private Scene scene;
        private PausedGameScreen pGS;

        public static PlayerControls PlayerControls { get { return Instance.playerControls; } }
        public static SoundManager SoundManager { get { return Instance.soundManager; } }
        public static MenuScreenManager MenuScreenManager { get { return Instance.menuScreenManager; } }
        public static Cursor Cursor { get { return Instance.cursor; } }
        public static Camera2D Camera { get { return Instance.camera; } }
        public static Scene Scene { get { return Instance.scene; } }
        public float DeltaTime { get; private set; }
        public float HalfViewPortWidth { get { return GraphicsDevice.Viewport.Width * 0.5f; } }
        public float HalfViewPortHeight { get { return GraphicsDevice.Viewport.Height * 0.5f; } }
        public const int GameWidth = 320;
        public const int GameHeight = 180;

        //Singleton
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

        public static void SetState(GameState newState)
        {
            Instance.currentGameState = newState;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            playerControls = new PlayerControls();
            soundManager = new SoundManager();
            menuScreenManager = new MenuScreenManager();
            cursor = new Cursor();
            camera = new Camera2D();
            scene = new Scene();

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

            director = new Director(new PlayerBuilder());
            player = director.Construct(Vector2.Zero);
            camera.Target = player.Transform;
            director = new Director(new EnemyBuilder());
            GameObject enemy = director.Construct(new Vector2(50, 50));
            scene.AddObject(player);
            scene.AddObject(enemy);
            camera.LoadContent(Content);
            cursor.LoadContent(Content);
            menuScreenManager.LoadContent(Content);
            soundManager.LoadContent(Content);
            soundManager.Music("hey");
            pGS = new PausedGameScreen();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (currentGameState == GameState.Quit)
            {
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    menuScreenManager.Update(graphics, currentGameState);
                    break;

                case GameState.InGame:

                    keyState = Keyboard.GetState();

                   
                  if (keyState.IsKeyDown(Keys.P) && !oldKeyState.IsKeyDown(Keys.P))
                    {                     	        
                         paused = !paused;
                        pGS.Update();                      
                    }
                  

                    if (!paused)
                    {
                        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                        foreach (GameObject go in scene.ActiveObjects)
                            go.Update();

                        scene.ProcessObjectLists();

                        #region Camera Movement

                        camera.Update(DeltaTime);

                        if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            camera.Position = new Vector2(player.Transform.Position.X + 80,
                                player.Transform.Position.Y + 98);
                        }

                        #endregion Camera Movement

                        //DONT DEBUGG HERE

                    }

                    oldKeyState = keyState;
                   
                    break;

                case GameState.Settings:
                    menuScreenManager.Update(graphics, currentGameState);
                    break;

                case GameState.Shop:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawer.Matrix = camera.ViewMatrix;
            drawer.BeginBatches();

            cursor.Draw(drawer[DrawLayer.Mouse]);

            //Switch case for checking the current game state, in each case something different happens
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    menuScreenManager.Draw(drawer[DrawLayer.UI], currentGameState);
                    break;

                case GameState.InGame:

                    foreach (GameObject gameObject in scene.ActiveObjects)
                    {
                        gameObject.Draw(drawer);
                    }
                    camera.Draw(drawer[DrawLayer.UI]);

                    if (paused)
                    {
                        pGS.DrawPause(drawer[DrawLayer.UI]);
                    }
                   
                    break;

                case GameState.Settings:
                    menuScreenManager.Draw(drawer[DrawLayer.UI], currentGameState);
                    break;

                case GameState.Shop:

                    break;

                default:
                    break;
            }
            // TODO: Add your drawing code here

            drawer.EndBatches();
            base.Draw(gameTime);
        }
    }
}