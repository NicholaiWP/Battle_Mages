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
        public bool mouseCanClickButton;

        public Button play;

        public Button settings;
        public Button quit;
        public Button oneRes;
        public Button twoRes;
        public Button threeRes;
        public Button fourRes;
        public Button back;

        private static MenuScreenManager instance;

        public static MenuScreenManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MenuScreenManager();
                }
                return instance;
            }
        }

        private MenuScreenManager()
        {
            mouseCanClickButton = true;
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Images/apple");

            #region Resolution Buttons

            oneRes = new Button(content.Load<Texture2D>("Images/1366x768"),
               content.Load<Texture2D>("Images/1366x768"));
            oneRes.SetPosition(new Vector2(-128, -350));

            twoRes = new Button(content.Load<Texture2D>("Images/1280x800"),
               content.Load<Texture2D>("Images/1280x800"));
            twoRes.SetPosition(new Vector2(-128, -200));

            threeRes = new Button(content.Load<Texture2D>("Images/1024x768"),
               content.Load<Texture2D>("Images/1024x768"));
            threeRes.SetPosition(new Vector2(-128, -50));

            fourRes = new Button(content.Load<Texture2D>("Images/800x600"),
                content.Load<Texture2D>("Images/800x600"));
            fourRes.SetPosition(new Vector2(-128, 100));

            back = new Button(content.Load<Texture2D>("Images/Back"),
               content.Load<Texture2D>("Images/Back"));
            back.SetPosition(new Vector2(-128, 250));

            #endregion Resolution Buttons

            #region Menu Buttons

            play = new Button(content.Load<Texture2D>("Images/playButton"),
                content.Load<Texture2D>("Images/playButtonHL"));
            play.SetPosition(new Vector2(-128, -150));

            quit = new Button(content.Load<Texture2D>("Images/Quit"), content.Load<Texture2D>("Images/Quit"));
            quit.SetPosition(new Vector2(-128, 0));

            settings = new Button(content.Load<Texture2D>("Images/Settings"),
                content.Load<Texture2D>("Images/Settings"));
            settings.SetPosition(new Vector2(-128, 150));

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
            if (play.isClicked == true)
            {
                play.isClicked = false;
                GameWorld.GetInstance.currentGameState = GameState.InGame;
            }
            else if (settings.isClicked == true)
            {
                settings.isClicked = false;
                GameWorld.GetInstance.currentGameState = GameState.Settings;
            }
            else if (quit.isClicked == true)
            {
                Environment.Exit(0);
            }
        }

        public void UpdateSettingWindow(GraphicsDeviceManager graphics)
        {
            oneRes.Update();
            twoRes.Update();
            threeRes.Update();
            fourRes.Update();
            back.Update();

            #region Button Is Clicked

            if (oneRes.isClicked == true)
            {
                oneRes.isClicked = false;
                graphics.PreferredBackBufferHeight = 768;
                graphics.PreferredBackBufferWidth = 1366;
                graphics.ApplyChanges();
            }
            else if (twoRes.isClicked == true)
            {
                twoRes.isClicked = false;
                graphics.PreferredBackBufferHeight = 800;
                graphics.PreferredBackBufferWidth = 1280;
                graphics.ApplyChanges();
            }
            else if (threeRes.isClicked == true)
            {
                threeRes.isClicked = false;
                graphics.PreferredBackBufferHeight = 768;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.ApplyChanges();
            }
            else if (fourRes.isClicked == true)
            {
                fourRes.isClicked = false;
                graphics.PreferredBackBufferHeight = 600;
                graphics.PreferredBackBufferWidth = 800;
                graphics.ApplyChanges();
            }
            else if (back.isClicked == true)
            {
                back.isClicked = false;
                GameWorld.GetInstance.currentGameState = GameState.MainMenu;
            }

            #endregion Button Is Clicked
        }

        public void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Rectangle(
                        (int)(0 - GameWorld.GetInstance.GetHalfViewPortWidth),
                        (int)(0 - GameWorld.GetInstance.GetHalfViewPortHeight),
                        (int)GameWorld.GetInstance.GetHalfViewPortWidth * 2,
                        (int)GameWorld.GetInstance.GetHalfViewPortHeight * 2), null, Color.White,
                        0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            settings.Draw(spriteBatch);
            quit.Draw(spriteBatch);
            play.Draw(spriteBatch);
            Cursor.GetInstance.Draw(spriteBatch, 0);
        }

        public void DrawSettingsWindow(SpriteBatch spriteBatch)
        {
            oneRes.Draw(spriteBatch);
            twoRes.Draw(spriteBatch);
            threeRes.Draw(spriteBatch);
            fourRes.Draw(spriteBatch);
            back.Draw(spriteBatch);
            Cursor.GetInstance.Draw(spriteBatch, 0);
        }
    }
}