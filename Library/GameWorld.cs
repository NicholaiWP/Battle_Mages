using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        private Camera2D camera;
        private GameObject player;
        private Director director;
        private float camMovespeed;
        private float speed;
        private float deltaTime;
        private KeyboardState currentKey;
        private KeyboardState lastKey;
        private Cursor cursor;
        private MenuScreenManager menuScreenManager;
        private Calculator calculator;
        public GameState currentGameState = GameState.MainMenu;

        //Lists
        private List<GameObject> activeObjects = new List<GameObject>();

        public List<GameObject> objectsToAdd = new List<GameObject>();
        public List<GameObject> objectsToRemove = new List<GameObject>();

        //Properties
        public int CursorPictureNumber { get; set; } = 0;

        public Cursor Cursor { get { return cursor; } }
        public MenuScreenManager MenuScreenManager { get { return menuScreenManager; } }
        public Calculator Calculator { get { return calculator; } }

        public List<GameObject> ActiveObjects
        {
            get { return activeObjects; }
        }

        public Camera2D Camera
        {
            get { return camera; }
        }

        public float DeltaTime
        {
            get { return deltaTime; }
        }

        public float HalfViewPortWidth
        {
            get { return GraphicsDevice.Viewport.Width * 0.5f; }
        }

        public float HalfViewPortHeight
        {
            get { return GraphicsDevice.Viewport.Height * 0.5f; }
        }

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
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            camera = new Camera2D();
            cursor = new Cursor();
            calculator = new Calculator();
            menuScreenManager = new MenuScreenManager();
            speed = 250;
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
            SoundManager.Instance.LoadContent(Content);

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
            //Testing sound
            SoundManager.Instance.Update();
            lastKey = currentKey;
            currentKey = Keyboard.GetState();
            //Testing sound
            if (currentKey.IsKeyDown(Keys.F) && lastKey.IsKeyUp(Keys.F))
            {
                SoundManager.Instance.PlaySound("FireBall");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    menuScreenManager.UpdateMenu();
                    break;

                case GameState.InGame:
                    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    foreach (GameObject go in activeObjects)
                    {
                        go.Update();
                    }
                    camMovespeed = speed * deltaTime;
                    CursorPictureNumber = 0;

                    #region Camera Movement

                    Vector2 mousePos = cursor.Position;
                    if (camera.TopRectangle.Contains(mousePos) && camera.RightRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(camMovespeed, -camMovespeed);
                    }
                    else if (camera.TopRectangle.Contains(mousePos) && camera.LeftRectangle.Contains(mousePos))
                    {
                        camera.Position -= new Vector2(camMovespeed, camMovespeed);
                    }
                    else if (camera.BottomRectangle.Contains(mousePos) && camera.LeftRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(-camMovespeed, camMovespeed);
                    }
                    else if (camera.BottomRectangle.Contains(mousePos) && camera.RightRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(camMovespeed, camMovespeed);
                    }
                    else if (camera.TopRectangle.Contains(mousePos))
                    {
                        camera.Position -= new Vector2(0, camMovespeed);
                        CursorPictureNumber = 1;
                    }
                    else if (camera.BottomRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(0, camMovespeed);
                    }
                    else if (camera.RightRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(camMovespeed, 0);
                    }
                    else if (camera.LeftRectangle.Contains(mousePos))
                    {
                        camera.Position -= new Vector2(camMovespeed, 0);
                    }
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
            switch (currentGameState)
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