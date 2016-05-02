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

        private Drawer drawer;
        private GameObject player;
        private Director director;
        private GameState currentGameState = GameState.MainMenu;
        private const int gameWidth = 1366;
        private const int gameHeight = 768;
        private PlayerControls playerControls;

        //Lists
        private List<GameObject> activeObjects = new List<GameObject>();

        public List<GameObject> objectsToAdd = new List<GameObject>();
        public List<GameObject> objectsToRemove = new List<GameObject>();

        //Properties
        public GameState CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }

        public ReadOnlyCollection<GameObject> ActiveObjects
        {
            get { return activeObjects.AsReadOnly(); }
        }

        private SoundManager soundManager = new SoundManager();
        private MenuScreenManager menuScreenManager = new MenuScreenManager();
        private Cursor cursor = new Cursor();
        private Camera2D camera = new Camera2D();

        public static SoundManager SoundManager { get { return Instance.soundManager; } }
        public static MenuScreenManager MenuScreenManager { get { return Instance.menuScreenManager; } }
        public static Cursor Cursor { get { return Instance.cursor; } }
        public static Camera2D Camera { get { return Instance.camera; } }

        public float DeltaTime { get; private set; }

        public float HalfViewPortWidth
        {
            get { return GraphicsDevice.Viewport.Width * 0.5f; }
        }

        public float HalfViewPortHeight
        {
            get { return GraphicsDevice.Viewport.Height * 0.5f; }
        }

        public static int GameWidth
        {
            get
            {
                return gameWidth;
            }
        }

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

        public static int GameHeight
        {
            get
            {
                return gameHeight;
            }
        }

        public PlayerControls PlayerControls
        {
            get
            {
                return playerControls;
            }

            set
            {
                playerControls = value;
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
            PlayerControls = new PlayerControls();
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
            objectsToAdd.Add(player);
            camera.LoadContent(Content);
            cursor.LoadContent(Content);
            menuScreenManager.LoadContent(Content);
            soundManager.LoadContent(Content);
            soundManager.Music("hey");
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
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                soundManager.SoundVolume -= 0.01f;
                soundManager.UpdateMusicVolume();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                soundManager.SoundVolume += 0.01f;
                soundManager.UpdateMusicVolume();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    menuScreenManager.UpdateMenu();
                    break;

                case GameState.InGame:
                    DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    foreach (GameObject go in activeObjects)
                    {
                        go.Update();
                    }

                    #region Camera Movement

                    camera.Update(DeltaTime);

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        camera.Position = new Vector2(player.Transform.Position.X + 80,
                            player.Transform.Position.Y + 98);
                    }

                    #endregion Camera Movement

                    //DONT DEBUGG HERE

                    TemplateControl();

                    break;

                case GameState.Settings:
                    menuScreenManager.UpdateSettingWindow(graphics);
                    break;

                case GameState.Shop:
                    break;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                menuScreenManager.mouseCanClickButton = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This method is for controlling our templates (lists)
        /// </summary>
        private void TemplateControl()
        {
            foreach (GameObject gameObject in objectsToAdd)
            {
                gameObject.LoadContent(Content);
                activeObjects.Add(gameObject);
            }
            objectsToAdd.Clear();

            foreach (GameObject gameObject in objectsToRemove)
            {
                activeObjects.Remove(gameObject);
            }
            objectsToRemove.Clear();
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

            cursor.Draw(drawer[DrawLayer.AboveUI]);

            //Switch case for checking the current game state, in each case something different happens
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    menuScreenManager.DrawMenu(drawer[DrawLayer.UI]);
                    break;

                case GameState.InGame:
                    foreach (GameObject gameObject in activeObjects)
                    {
                        gameObject.Draw(drawer);
                    }
                    camera.Draw(drawer[DrawLayer.UI]);
                    break;

                case GameState.Settings:
                    menuScreenManager.DrawSettingsWindow(drawer[DrawLayer.UI]);
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
