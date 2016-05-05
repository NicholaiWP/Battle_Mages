﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battle_Mages
{
    public class MainMenuWindow
    {
        private List<Button> mainMenuButtons = new List<Button>();
        private Texture2D background;
        private Vector2 bgStartPos;

        public MainMenuWindow()
        {
            bgStartPos = new Vector2(-GameWorld.GameWidth / 2, -GameWorld.GameHeight / 2);
            var content = GameWorld.Instance.Content;
            //Play button
            var playSpr1 = content.Load<Texture2D>("Images/BMPlayGameButton");
            var playSpr2 = content.Load<Texture2D>("Images/BMPlayGameButton");
            mainMenuButtons.Add(new Button(
                playSpr1,
                playSpr2,
                new Vector2(-playSpr1.Width / 2, playSpr1.Height * -1f),
                () => { GameWorld.SetState(GameState.InGame); }
                ));
            //Load game button
            var loadSpr1 = content.Load<Texture2D>("Images/BMLoadGameButton");
            var loadSpr2 = content.Load<Texture2D>("Images/BMLoadGameButton");
            mainMenuButtons.Add(new Button(
                loadSpr1,
                loadSpr2,
                new Vector2(-loadSpr1.Width / 2, 0),
                () => { }
                ));
            //Settings button
            var settingsSpr1 = content.Load<Texture2D>("Images/BMSettingsButton");
            var settingsSpr2 = content.Load<Texture2D>("Images/BMSettingsButton");
            mainMenuButtons.Add(new Button(
                settingsSpr1,
                settingsSpr2,
                new Vector2(-settingsSpr1.Width / 2, settingsSpr1.Height * 1f),
                () => { GameWorld.SetState(GameState.Settings); }
                ));
            //Quit button
            var quitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
            var quitSpr2 = content.Load<Texture2D>("Images/BMQuitButton");
            mainMenuButtons.Add(new Button(
                quitSpr1,
                quitSpr2,
                new Vector2(-quitSpr1.Width / 2, quitSpr1.Height * 2f),
                () => { GameWorld.SetState(GameState.Quit); }
                ));
        }

        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("Images/BMmenu");
        }

        public void Update()
        {
            foreach (Button button in mainMenuButtons)
            {
                button.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position: bgStartPos, color: Color.White);
            foreach (Button button in mainMenuButtons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
