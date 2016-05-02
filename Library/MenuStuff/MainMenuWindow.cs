using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class MainMenuWindow
    {
        private List<Button> mainMenuButtons = new List<Button>();

        public MainMenuWindow()
        {
            mainMenuButtons.Add(new Button(MenuButtons.Play));
            mainMenuButtons.Add(new Button(MenuButtons.Settings));
            mainMenuButtons.Add(new Button(MenuButtons.Quit));
        }

        public void LoadContent(ContentManager content)
        {
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
            foreach (Button button in mainMenuButtons)
            {
                button.Draw(spriteBatch);
            }
        }
    }
}