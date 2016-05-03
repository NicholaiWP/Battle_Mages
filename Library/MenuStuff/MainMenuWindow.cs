using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            mainMenuButtons.Add(new Button(MenuButtons.Play));
            mainMenuButtons.Add(new Button(MenuButtons.Settings));
            mainMenuButtons.Add(new Button(MenuButtons.Quit));
            mainMenuButtons.Add(new Button(MenuButtons.LoadGame));
        }

        public void LoadContent(ContentManager content)
        {
            background = content.Load<Texture2D>("Images/BMmenu");
            foreach (Button button in mainMenuButtons)
            {
                button.LoadContent(content);
            }
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