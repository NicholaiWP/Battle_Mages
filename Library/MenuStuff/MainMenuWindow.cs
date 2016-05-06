using System;
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

        public MainMenuWindow()
        {
            var content = GameWorld.Instance.Content;
            //Play button
            var playSpr1 = content.Load<Texture2D>("Images/BMPlayGameButton");
            var playSpr2 = content.Load<Texture2D>("Images/BMPlayGameButton");
            mainMenuButtons.Add(new Button(
                playSpr1,
                playSpr2,
                new Vector2(-playSpr1.Width / 2, playSpr1.Height * -1f),
                () => { GameWorld.SetState(GameState.InGame); },
                true
                ));
            //Load game button
            var loadSpr1 = content.Load<Texture2D>("Images/BMLoadGameButton");
            var loadSpr2 = content.Load<Texture2D>("Images/BMLoadGameButton");
            mainMenuButtons.Add(new Button(
                loadSpr1,
                loadSpr2,
                new Vector2(-loadSpr1.Width / 2, 0),
                () => { },
                true
                ));
            //Settings button
            var settingsSpr1 = content.Load<Texture2D>("Images/BMSettingsButton");
            var settingsSpr2 = content.Load<Texture2D>("Images/BMSettingsButton");
            mainMenuButtons.Add(new Button(
                settingsSpr1,
                settingsSpr2,
                new Vector2(-settingsSpr1.Width / 2, settingsSpr1.Height * 1f),
                () => { GameWorld.SetState(GameState.Settings); },
                true
                ));
            //Quit button
            var quitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
            var quitSpr2 = content.Load<Texture2D>("Images/BMQuitButton");
            mainMenuButtons.Add(new Button(
                quitSpr1,
                quitSpr2,
                new Vector2(-quitSpr1.Width / 2, quitSpr1.Height * 2f),
                () => { GameWorld.SetState(GameState.Quit); },
                true
                ));
        }

        public void LoadContent(ContentManager content)
        {
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
            foreach (Button button in mainMenuButtons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}
