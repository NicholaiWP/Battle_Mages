using Microsoft.Xna.Framework;
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

        private KeyboardState keyState, oldKeyState;
        private Drawer drawer;
        private GameObject player;
        private GameState currentGameState = GameState.MainMenu;

        private PlayerControls playerControls;
        private SoundManager soundManager;
        private MenuScreenManager menuScreenManager;
        private Cursor cursor;
        private Camera2D camera;
        private Scene scene;
        private float deltaTime;
        private bool paused;
        private PausedGameScreen pGS;
        private LobbyScreen lobbyS;
        private static GameWorld instance;

        public static PlayerControls PlayerControls { get { return Instance.playerControls; } }
        public static SoundManager SoundManager { get { return Instance.soundManager; } }
        public static MenuScreenManager MenuScreenManager { get { return Instance.menuScreenManager; } }
        public static Cursor Cursor { get { return Instance.cursor; } }
        public static Camera2D Camera { get { return Instance.camera; } }
        public static Scene Scene { get { return Instance.scene; } }
        public float DeltaTime { get { return Instance.deltaTime; } }
        public bool Paused { get { return paused; } set { paused = value; } }
        public LobbyScreen LobbyS { get { return lobbyS; } set { lobbyS = value; } }
        public float HalfViewPortWidth { get { return GraphicsDevice.Viewport.Width * 0.5f; } }
        public float HalfViewPortHeight { get { return GraphicsDevice.Viewport.Height * 0.5f; } }

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

            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer(ellipse, "Images/BMarena"));
            scene.AddObject(ellipse);

            player = ObjectBuilder.BuildPlayer(Vector2.Zero);
            camera.Target = player.Transform;
            GameObject enemy = ObjectBuilder.BuildEnemy(new Vector2(50, 50));
            scene.AddObject(player);
            scene.AddObject(enemy);
            camera.LoadContent(Content);
            cursor.LoadContent(Content);
            menuScreenManager.LoadContent(Content);
            soundManager.LoadContent(Content);
            soundManager.Music("hey");
            pGS = new PausedGameScreen();
            LobbyS = new LobbyScreen();
            var wall = new GameObject(new Vector2(0, -100));
            wall.AddComponent(new Collider(wall, new Vector2(128, 32)));
            scene.AddObject(wall);
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
            if (!Cursor.CanClick && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                Cursor.CanClick = true;
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
                        Paused = !Paused;
                        pGS.Update();
                    }
                    if (!Paused)
                    {
                        deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                    else if (Paused)
                    {
                        pGS.Update();
                    }
                    oldKeyState = keyState;

                    break;

                case GameState.Settings:
                    menuScreenManager.Update(graphics, currentGameState);
                    break;

                case GameState.Shop:
                    break;

                case GameState.Lobby:

                    player.Update();
                    LobbyS.Update();
                    player.Update();
                    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    #region Camera Movement

                    camera.Update(DeltaTime);

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        camera.Position = new Vector2(player.Transform.Position.X + 80,
                            player.Transform.Position.Y + 98);
                    }

                    #endregion Camera Movement

                    //DONT DEBUGG HERE

                    break;
            }

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

                    if (Paused)
                    {
                        pGS.DrawPause(drawer[DrawLayer.UI]);
                    }

                    break;

                case GameState.Settings:
                    menuScreenManager.Draw(drawer[DrawLayer.UI], currentGameState);
                    break;

                case GameState.Shop:

                    break;

                case GameState.Lobby:

                    player.Draw(drawer);
                    camera.Draw(drawer[DrawLayer.UI]);
                    LobbyS.DrawLobby(drawer[DrawLayer.Background]);

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