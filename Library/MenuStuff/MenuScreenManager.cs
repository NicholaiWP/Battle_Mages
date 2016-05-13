using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class MenuScreenManager
    {
        private Vector2 fontPosition;
        private string currentResolutionString;
        private DisplayMode currentResolution;
        private DisplayMode resolutionHolder;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        private Vector2 scalingVector;
        private SettingsWindow settingsWindow;
        private MainMenuWindow mainMenuWindow;
        private Texture2D background;
        private Vector2 bgStartPos;

        public Vector2 ScalingVector { get { return scalingVector; } set { scalingVector = value; } }
        public string CurrentResolutionString { get { return currentResolutionString; } }
        public int ElementAtNumber { get; set; }
        public bool SwappingKeyBind { get; set; } = false;
        public PlayerBind ChosenKeyToRebind { get; set; }

       

        public MenuScreenManager()
        {
            bgStartPos = new Vector2(-GameWorld.GameWidth / 2, -GameWorld.GameHeight / 2);
            fontPosition = new Vector2(-100, -50);
            settingsWindow = new SettingsWindow();
            mainMenuWindow = new MainMenuWindow();
        }

        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("Images/BMmenu");
            settingsWindow.LoadContent(content);
            mainMenuWindow.LoadContent(content);
            DisplayMode lastResolution = null;
            int i = 0;
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
                    i++;
                }
                lastResolution = dmode;
            }
            ElementAtNumber = i - 1;
            ScalingVector = new Vector2(Utils.CalculateWidthScale(currentResolution.Width),
                Utils.CalculateHeightScale(currentResolution.Height));
        }

        /// <summary>
        /// Method for updating the menu
        /// </summary>
        public void Update(GraphicsDeviceManager graphics, GameState currentState)
        {
            if (SwappingKeyBind)
            {
                GameWorld.Cursor.CanClick = false;
                ReadNewKey();
            }
            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenuWindow.Update();
                    break;

                case GameState.Settings:
                    settingsWindow.Update();
                    break;
            }


            if (ElementAtNumber >= resolutions.Count) ElementAtNumber = 0;
            if (ElementAtNumber < 0) ElementAtNumber = resolutions.Count - 1;

            currentResolution = resolutions[ElementAtNumber];

            if (currentResolution != resolutionHolder)
            {
                graphics.PreferredBackBufferWidth = currentResolution.Width;
                graphics.PreferredBackBufferHeight = currentResolution.Height;
                graphics.ApplyChanges();
            }
            resolutionHolder = currentResolution;

            scalingVector = new Vector2(Utils.CalculateWidthScale(currentResolution.Width),
                Utils.CalculateHeightScale(currentResolution.Height));

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (currentResolution == resolutions.ElementAt(i))
                {
                    ElementAtNumber = i;
                    currentResolutionString = (currentResolution.Width + "x" + currentResolution.Height);
                }
            }
        }

        private void ReadNewKey()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] keyPressed = kbState.GetPressedKeys();
            if (keyPressed.Length == 1)
            {
                GameWorld.PlayerControls.ChangeBinding(ChosenKeyToRebind, keyPressed[0]);
                SwappingKeyBind = false;
                GameWorld.Cursor.CanClick = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameState currentState)
        {
            spriteBatch.Draw(background, position: bgStartPos, color: Color.White);
            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenuWindow.Draw(spriteBatch);
                    break;

                case GameState.Settings:
                    settingsWindow.Draw(spriteBatch);
                    break;
            }
        }
    }
}