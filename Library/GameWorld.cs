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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Camera2D camera;
        private Texture2D testTexture;
        private float camMovespeed;
        private float speed;
        private float deltaTime;
        private Cursor cursor;
        public int CursorPictureNumber { get; set; } = 0;

        public Cursor GetCursor
        {
            get
            {
                return cursor;
            }
        }

        public Camera2D GetCamera
        {
            get
            {
                return camera;
            }
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

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            camera = new Camera2D();
            cursor = Cursor.GetInstance;
            speed = 150;
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
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            camMovespeed = speed * deltaTime;
            CursorPictureNumber = 0;
            Vector2 mousePos = cursor.GetPosition;
            if (camera.GetTopRectangle.Contains(mousePos))
            {
                camera.Pos -= new Vector2(0, camMovespeed);
                CursorPictureNumber = 1;
            }
            else if (camera.GetBottomRectangle.Contains(mousePos))
            {
                camera.Pos += new Vector2(0, camMovespeed);
            }
            if (camera.GetRightRectangle.Contains(mousePos))
            {
                camera.Pos += new Vector2(camMovespeed, 0);
            }
            else if (camera.GetLeftRectangle.Contains(mousePos))
            {
                camera.Pos -= new Vector2(camMovespeed, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                camera.Pos = Vector2.Zero;
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

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null,
               null, null, null, camera.GetViewMatrix);

            cursor.Draw(spriteBatch, CursorPictureNumber);

            spriteBatch.Draw(testTexture, new Vector2(-98.5f, -109), Color.White);
            camera.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}