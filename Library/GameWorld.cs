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

        private SpriteBatch spriteBatch;
        private Camera2D camera;
        private Texture2D testTexture;
        private float camMovespeed;
        private float speed;
        private float deltaTime;
        private Cursor cursor;
        private GameState currentGameState = GameState.MainMenu;
        public Button play;

        //Lists
        private List<GameObject> objectsToDraw = new List<GameObject>();

        private List<Collider> colliders = new List<Collider>();
        public List<GameObject> objectsToAdd = new List<GameObject>();
        public List<GameObject> objectsToRemove = new List<GameObject>();
        public List<Collider> collidersToAdd = new List<Collider>();
        public List<Collider> collidersToRemove = new List<Collider>();

        //Properties
        public int CursorPictureNumber { get; set; } = 0;

        public GameState GetCurrentGameState
        {
            get { return currentGameState; }
        }

        public Cursor GetCursor
        {
            get { return cursor; }
        }

        public Camera2D GetCamera
        {
            get { return camera; }
        }

        public float GetDeltaTime
        {
            get { return deltaTime; }
        }

        public float GetHalfViewPortWidth
        {
            get { return GraphicsDevice.Viewport.Width * 0.5f; }
        }

        public float GetHalfViewPortHeight
        {
            get { return GraphicsDevice.Viewport.Height * 0.5f; }
        }

        //Singleton
        private static GameWorld instance;

        public static GameWorld GetInstance
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
            camera = new Camera2D();
            cursor = Cursor.GetInstance;
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
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            camera.LoadContent(Content);
            cursor.LoadContent(Content);
            testTexture = Content.Load<Texture2D>("Images/apple");
            play = new Button(Content.Load<Texture2D>("Images/playButton"), Content.Load<Texture2D>("Images/playButtonHL")
                , graphics.GraphicsDevice);
            play.SetPosition(new Vector2(50, 100));
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
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    MouseState mouse = Mouse.GetState();
                    play.Update(mouse);
                    if (play.isClicked == true)
                    {
                        currentGameState = GameState.InGame;
                    }
                    break;

                case GameState.InGame:
                    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    camMovespeed = speed * deltaTime;
                    CursorPictureNumber = 0;

                    #region Camera Movement

                    Vector2 mousePos = cursor.GetPosition;
                    if (camera.GetTopRectangle.Contains(mousePos) && camera.GetRightRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(camMovespeed, -camMovespeed);
                    }
                    else if (camera.GetTopRectangle.Contains(mousePos) && camera.GetLeftRectangle.Contains(mousePos))
                    {
                        camera.Position -= new Vector2(camMovespeed, camMovespeed);
                    }
                    else if (camera.GetBottomRectangle.Contains(mousePos) && camera.GetLeftRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(-camMovespeed, camMovespeed);
                    }
                    else if (camera.GetBottomRectangle.Contains(mousePos) && camera.GetRightRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(camMovespeed, camMovespeed);
                    }
                    else if (camera.GetTopRectangle.Contains(mousePos))
                    {
                        camera.Position -= new Vector2(0, camMovespeed);
                        CursorPictureNumber = 1;
                    }
                    else if (camera.GetBottomRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(0, camMovespeed);
                    }
                    else if (camera.GetRightRectangle.Contains(mousePos))
                    {
                        camera.Position += new Vector2(camMovespeed, 0);
                    }
                    else if (camera.GetLeftRectangle.Contains(mousePos))
                    {
                        camera.Position -= new Vector2(camMovespeed, 0);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        camera.Position = Vector2.Zero;
                    }

                    #endregion Camera Movement

                    //DONT DEBUGG HERE
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        Exit();
                    }
                    TemplateControl();
                    break;

                case GameState.Settings:
                    break;

                case GameState.Shop:
                    break;

                default:
                    break;
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
            }

            objectsToDraw.AddRange(objectsToAdd);
            colliders.AddRange(collidersToAdd);
            foreach (GameObject gameObject in objectsToRemove)
            {
                objectsToDraw.Remove(gameObject);
            }

            foreach (Collider collider in collidersToRemove)
            {
                colliders.Remove(collider);
            }
            ClearTemplates();
        }

        /// <summary>
        /// Method for clearing the templates (lists)
        /// </summary>
        private void ClearTemplates()
        {
            objectsToAdd.Clear();
            objectsToRemove.Clear();
            collidersToAdd.Clear();
            collidersToRemove.Clear();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Switch case for checking the current game state, in each case something different happens
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null);
                    spriteBatch.Draw(Content.Load<Texture2D>("Images/apple"), new Rectangle(0, 0,
                        (int)GetHalfViewPortWidth * 2, (int)GetHalfViewPortHeight * 2), null, Color.White,
                        0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                    play.Draw(spriteBatch);
                    cursor.Draw(spriteBatch, 0);
                    break;

                case GameState.InGame:
                    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null,
           null, null, null, camera.GetViewMatrix);
                    cursor.Draw(spriteBatch, CursorPictureNumber);
                    spriteBatch.Draw(testTexture, new Rectangle(-99, -109, testTexture.Width, testTexture.Height), null, Color.White,
                        0f, Vector2.Zero, SpriteEffects.None, 0.1f);
                    foreach (GameObject gameObject in objectsToDraw)
                    {
                        gameObject.Draw(spriteBatch);
                    }
                    camera.Draw(spriteBatch);
                    break;

                case GameState.Settings:

                    break;

                case GameState.Shop:

                    break;

                default:
                    break;
            }
            // TODO: Add your drawing code here

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}