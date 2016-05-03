using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class SettingsWindow
    {
        private List<Button> settingsButtons = new List<Button>();
        private SpriteFont fontBM;
        private Vector2 fontPosition;

        public SettingsWindow()
        {
            fontPosition = Vector2.Zero;
            settingsButtons.Add(new Button(MenuButtons.Back));
            /* settingsButtons.Add(new Button(MenuButtons.KeyBindDown));
             settingsButtons.Add(new Button(MenuButtons.KeyBindLeft));
             settingsButtons.Add(new Button(MenuButtons.KeyBindRight));*/
            settingsButtons.Add(new Button(MenuButtons.KeyBindUp));
            settingsButtons.Add(new Button(MenuButtons.ResDown));
            settingsButtons.Add(new Button(MenuButtons.ResUp));
        }

        public void LoadContent(ContentManager content)
        {
            fontBM = content.Load<SpriteFont>("FontBM");
            foreach (Button button in settingsButtons)
            {
                button.LoadContent(content);
            }
        }

        public void Update()
        {
            foreach (Button button in settingsButtons)
            {
                button.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in settingsButtons)
            {
                button.Draw(spriteBatch);
            }
        }

        public void DrawStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(fontBM, GameWorld.MenuScreenManager.CurrentResolutionString, fontPosition,
                Color.Black);
            spriteBatch.DrawString(fontBM,
                GameWorld.PlayerControls.KeyToString(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)),
                new Vector2(100, -250), Color.Black);
        }
    }
}