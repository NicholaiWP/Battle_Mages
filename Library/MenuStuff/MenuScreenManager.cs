using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private bool mouseCanClickButton;
        private SettingsWindow settingsWindow;
        private MainMenuWindow mainMenuWindow;
        public Vector2 ScalingVector { get { return scalingVector; } set { scalingVector = value; } }
        public string CurrentResolutionString { get { return currentResolutionString; } }
        public bool MouseCanClickButton { get { return mouseCanClickButton; } set { mouseCanClickButton = value; } }
        public int ElementAtNumber { get; set; }
        public bool SwappingKeyBind { get; set; } = false;
        public PlayerBind ChosenKeyToRebind { get; set; }

        public MenuScreenManager()
        {
            fontPosition = new Vector2(-100, -50);
            mouseCanClickButton = true;
            settingsWindow = new SettingsWindow();
            mainMenuWindow = new MainMenuWindow();
        }

        public void LoadContent(ContentManager content)
        {
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
            ScalingVector = new Vector2(ViewCalculator.CalculateWidthScale(currentResolution.Width),
                ViewCalculator.CalculateHeightScale(currentResolution.Height));
        }

        /// <summary>
        /// Method for updating the menu
        /// </summary>
        public void Update(GraphicsDeviceManager graphics, GameState currentState)
        {
            if (SwappingKeyBind)
            {
                mouseCanClickButton = false;
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
            if (!mouseCanClickButton && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                mouseCanClickButton = true;
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

            scalingVector = new Vector2(ViewCalculator.CalculateWidthScale(currentResolution.Width),
                ViewCalculator.CalculateHeightScale(currentResolution.Height));

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
                mouseCanClickButton = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameState currentState)
        {
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
