using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class MenuScreenManager
    {
        private Texture2D sprite;
        private Vector2 fontPosition;
        private int elementAtNumber;
        private string currentResolutionString;
        private DisplayMode currentResolution;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        private Vector2 scalingVector;
        private bool mouseCanClickButton;
        private Button play;
        private Button settings;
        private Button quit;
        private Button oneRes;
        private Button twoRes;
        private Button threeRes;
        private Button fourRes;
        private Button back;
        private SpriteFont fontBM;

        public Vector2 ScalingVector
        {
            get
            {
                return scalingVector;
            }

            set
            {
                scalingVector = value;
            }
        }

        public bool MouseCanClickButton
        {
            get
            {
                return mouseCanClickButton;
            }

            set
            {
                mouseCanClickButton = value;
            }
        }

        public Button Play
        {
            get
            {
                return play;
            }

            set
            {
                play = value;
            }
        }

        public Button Settings
        {
            get
            {
                return settings;
            }

            set
            {
                settings = value;
            }
        }

        public Button Quit
        {
            get
            {
                return quit;
            }

            set
            {
                quit = value;
            }
        }

        public Button OneRes
        {
            get
            {
                return oneRes;
            }

            set
            {
                oneRes = value;
            }
        }

        public Button TwoRes
        {
            get
            {
                return twoRes;
            }

            set
            {
                twoRes = value;
            }
        }

        public Button ThreeRes
        {
            get
            {
                return threeRes;
            }

            set
            {
                threeRes = value;
            }
        }

        public Button FourRes
        {
            get
            {
                return fourRes;
            }

            set
            {
                fourRes = value;
            }
        }

        public Button Back
        {
            get
            {
                return back;
            }

            set
            {
                back = value;
            }
        }

        public SpriteFont FontBM
        {
            get
            {
                return fontBM;
            }

            set
            {
                fontBM = value;
            }
        }

        public MenuScreenManager()
        {
            fontPosition = new Vector2(-100, -50);
        }

        public void LoadContent(ContentManager content)
        {
            DisplayMode lastResolution = null;
            FontBM = content.Load<SpriteFont>("FontBM");
            sprite = content.Load<Texture2D>("Images/apple");

            foreach (DisplayMode dmode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (dmode.Height == GameWorld.Instance.HalfViewPortHeight * 2 &&
                    dmode.Width == GameWorld.Instance.HalfViewPortWidth * 2)
                {
                    currentResolution = dmode;
                    currentResolutionString = (dmode.Width + "x" + dmode.Height);
                }

                if (lastResolution != dmode)
                {
                    resolutions.Add(dmode);
                }
                lastResolution = dmode;
            }
            ScalingVector = new Vector2(ViewCalculator.CalculateWidthScale(currentResolution.Width),
                ViewCalculator.CalculateHeightScale(currentResolution.Height));

            Back = new Button(content.Load<Texture2D>("Images/Back"),
            content.Load<Texture2D>("Images/Back"));

            #region Resolution Buttons

            OneRes = new Button(content.Load<Texture2D>("Images/1366x768"),
              content.Load<Texture2D>("Images/1366x768"));

            TwoRes = new Button(content.Load<Texture2D>("Images/1280x800"),
               content.Load<Texture2D>("Images/1280x800"));

            #endregion Resolution Buttons

            #region Menu Buttons

            Play = new Button(content.Load<Texture2D>("Images/playButton"),
                content.Load<Texture2D>("Images/playButtonHL"));

            Quit = new Button(content.Load<Texture2D>("Images/Quit"), content.Load<Texture2D>("Images/Quit"));

            Settings = new Button(content.Load<Texture2D>("Images/Settings"),
                content.Load<Texture2D>("Images/Settings"));

            #endregion Menu Buttons
        }

        /// <summary>
        /// Method for updating the menu
        /// </summary>
        public void UpdateMenu()
        {
            Play.Update();
            Settings.Update();
            Quit.Update();
            Play.SetPosition(new Vector2(-Play.rectangle.Width / 2, -Play.rectangle.Height * 1.5f));
            Settings.SetPosition(new Vector2(-Settings.rectangle.Width / 2, 0));
            Quit.SetPosition(new Vector2(-Quit.rectangle.Width / 2, Quit.rectangle.Height * 1.5f));

            if (Play.isClicked == true)
            {
                Play.isClicked = false;
                GameWorld.Instance.CurrentGameState = GameState.InGame;
            }
            else if (Settings.isClicked == true)
            {
                Settings.isClicked = false;
                GameWorld.Instance.CurrentGameState = GameState.Settings;
            }
            else if (Quit.isClicked == true)
            {
                GameWorld.Instance.Exit();
            }
        }

        public void UpdateSettingWindow(GraphicsDeviceManager graphics)
        {
            Back.Update();
            Back.SetPosition(new Vector2(-Back.rectangle.Width / 2, Back.rectangle.Height * 3f));
            OneRes.SetPosition(new Vector2(150, -50));
            TwoRes.SetPosition(new Vector2(-550, -50));
            OneRes.Update();
            TwoRes.Update();
            if (OneRes.isClicked == true)
            {
                OneRes.isClicked = false;
                elementAtNumber++;
                if (elementAtNumber >= resolutions.Count) elementAtNumber = 0;
                currentResolution = resolutions[elementAtNumber];
                graphics.PreferredBackBufferWidth = currentResolution.Width;
                graphics.PreferredBackBufferHeight = currentResolution.Height;
                graphics.ApplyChanges();
                ScalingVector = new Vector2(ViewCalculator.CalculateWidthScale(currentResolution.Width),
                    ViewCalculator.CalculateHeightScale(currentResolution.Height));
            }
            else if (TwoRes.isClicked == true)
            {
                TwoRes.isClicked = false;
                elementAtNumber--;
                if (elementAtNumber <= 0) elementAtNumber = resolutions.Count - 1;
                currentResolution = resolutions[elementAtNumber];
                graphics.PreferredBackBufferWidth = currentResolution.Width;
                graphics.PreferredBackBufferHeight = currentResolution.Height;
                graphics.ApplyChanges();
                ScalingVector = new Vector2(ViewCalculator.CalculateWidthScale(currentResolution.Width),
                    ViewCalculator.CalculateHeightScale(currentResolution.Height));
            }
            if (Back.isClicked == true)
            {
                Back.isClicked = false;
                GameWorld.Instance.CurrentGameState = GameState.MainMenu;
            }

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (currentResolution == resolutions.ElementAt(i))
                {
                    elementAtNumber = i;
                    currentResolutionString = (currentResolution.Width + "x" + currentResolution.Height);
                }
            }
        }

        public void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite,
            destinationRectangle: new Rectangle(-GameWorld.GameWidth / 2,
               -GameWorld.GameHeight / 2,
               GameWorld.GameWidth, GameWorld.GameHeight),
            color: Color.White,
            origin: Vector2.Zero,
            effects: SpriteEffects.None);
            Settings.Draw(spriteBatch);
            Quit.Draw(spriteBatch);
            Play.Draw(spriteBatch);
        }

        public void DrawSettingsWindow(SpriteBatch spriteBatch)
        {
            OneRes.Draw(spriteBatch);
            TwoRes.Draw(spriteBatch);
            spriteBatch.DrawString(FontBM, currentResolutionString, fontPosition, Color.White);
            spriteBatch.DrawString(FontBM, GameWorld.Instance.Cursor.Position.ToString(),
                new Vector2(0, -150), Color.White);
            spriteBatch.DrawString(FontBM,
                GameWorld.Instance.PlayerControls.KeyToString(GameWorld.Instance.PlayerControls.GetBinding(PlayerBind.Spell1)),
                new Vector2(50, 50), Color.Black);
            Back.Draw(spriteBatch);
        }
    }
}