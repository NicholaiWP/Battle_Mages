﻿using Microsoft.Xna.Framework;
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
        private int elemenAtNumber;
        private string currentResolutionString;
        private DisplayMode currentResolution;
        private List<DisplayMode> resolutions = new List<DisplayMode>();
        public Vector2 scale;
        public bool mouseCanClickButton;
        public Button play;
        public Button settings;
        public Button quit;
        public Button oneRes;
        public Button twoRes;
        public Button threeRes;
        public Button fourRes;
        public Button back;
        public SpriteFont fontBM;

        public MenuScreenManager()
        {
            fontPosition = new Vector2(-100, -50);
        }

        public void LoadContent(ContentManager content)
        {
            DisplayMode lastResolution = null;
            fontBM = content.Load<SpriteFont>("FontBM");
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
            scale = new Vector2(GameWorld.Instance.Calculator.WidthScaleCalculate(currentResolution.Width),
                GameWorld.Instance.Calculator.HeightScaleCalculate(currentResolution.Height));

            back = new Button(content.Load<Texture2D>("Images/Back"),
            content.Load<Texture2D>("Images/Back"));

            #region Resolution Buttons

            oneRes = new Button(content.Load<Texture2D>("Images/1366x768"),
              content.Load<Texture2D>("Images/1366x768"));

            twoRes = new Button(content.Load<Texture2D>("Images/1280x800"),
               content.Load<Texture2D>("Images/1280x800"));

            #endregion Resolution Buttons

            #region Menu Buttons

            play = new Button(content.Load<Texture2D>("Images/playButton"),
                content.Load<Texture2D>("Images/playButtonHL"));

            quit = new Button(content.Load<Texture2D>("Images/Quit"), content.Load<Texture2D>("Images/Quit"));

            settings = new Button(content.Load<Texture2D>("Images/Settings"),
                content.Load<Texture2D>("Images/Settings"));

            #endregion Menu Buttons
        }

        /// <summary>
        /// Method for updating the menu
        /// </summary>
        public void UpdateMenu()
        {
            play.Update();
            settings.Update();
            quit.Update();
            play.SetPosition(new Vector2(-play.rectangle.Width / 2, -play.rectangle.Height * 1.5f));
            settings.SetPosition(new Vector2(-settings.rectangle.Width / 2, 0));
            quit.SetPosition(new Vector2(-quit.rectangle.Width / 2, quit.rectangle.Height * 1.5f));

            if (play.isClicked == true)
            {
                play.isClicked = false;
                GameWorld.Instance.currentGameState = GameState.InGame;
            }
            else if (settings.isClicked == true)
            {
                settings.isClicked = false;
                GameWorld.Instance.currentGameState = GameState.Settings;
            }
            else if (quit.isClicked == true)
            {
                GameWorld.Instance.Exit();
            }
        }

        public void UpdateSettingWindow(GraphicsDeviceManager graphics)
        {
            back.Update();
            back.SetPosition(new Vector2(-back.rectangle.Width / 2, back.rectangle.Height * 3f));
            oneRes.SetPosition(new Vector2(150, -50));
            twoRes.SetPosition(new Vector2(-550, -50));
            oneRes.Update();
            twoRes.Update();
            if (oneRes.isClicked == true)
            {
                oneRes.isClicked = false;
                elemenAtNumber++;
                if (elemenAtNumber >= resolutions.Count) elemenAtNumber = 0;
                currentResolution = resolutions[elemenAtNumber];
                graphics.PreferredBackBufferWidth = currentResolution.Width;
                graphics.PreferredBackBufferHeight = currentResolution.Height;
                graphics.ApplyChanges();
                scale = new Vector2(GameWorld.Instance.Calculator.WidthScaleCalculate(currentResolution.Width),
                    GameWorld.Instance.Calculator.HeightScaleCalculate(currentResolution.Height));
            }
            else if (twoRes.isClicked == true)
            {
                twoRes.isClicked = false;
                elemenAtNumber--;
                if (elemenAtNumber <= 0) elemenAtNumber = resolutions.Count - 1;
                currentResolution = resolutions[elemenAtNumber];
                graphics.PreferredBackBufferWidth = currentResolution.Width;
                graphics.PreferredBackBufferHeight = currentResolution.Height;
                graphics.ApplyChanges();
                scale = new Vector2(GameWorld.Instance.Calculator.WidthScaleCalculate(currentResolution.Width),
                    GameWorld.Instance.Calculator.HeightScaleCalculate(currentResolution.Height));
            }
            if (back.isClicked == true)
            {
                back.isClicked = false;
                GameWorld.Instance.currentGameState = GameState.MainMenu;
            }

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (currentResolution == resolutions.ElementAt(i))
                {
                    elemenAtNumber = i;
                    currentResolutionString = (currentResolution.Width + "x" + currentResolution.Height);
                }
            }
        }

        public void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite,
            destinationRectangle: new Rectangle((int)(0 - GameWorld.Instance.HalfViewPortWidth),
               (int)(0 - GameWorld.Instance.HalfViewPortHeight),
               (int)GameWorld.Instance.HalfViewPortWidth * 2,
               (int)GameWorld.Instance.HalfViewPortHeight * 2),
            color: Color.White,
            origin: Vector2.Zero,
            effects: SpriteEffects.None);
            settings.Draw(spriteBatch);
            quit.Draw(spriteBatch);
            play.Draw(spriteBatch);
        }

        public void DrawSettingsWindow(SpriteBatch spriteBatch)
        {
            oneRes.Draw(spriteBatch);
            twoRes.Draw(spriteBatch);
            spriteBatch.DrawString(fontBM, currentResolutionString, fontPosition, Color.White);
            back.Draw(spriteBatch);
        }
    }
}