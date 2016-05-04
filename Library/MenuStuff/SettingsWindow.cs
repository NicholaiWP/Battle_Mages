using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            var content = GameWorld.Instance.Content;
            var backSpr = content.Load<Texture2D>("Images/Back");
            settingsButtons.Add(new Button(
                    backSpr,
                    backSpr,
                    new Vector2(-backSpr.Width / 2, backSpr.Height * 3f),
                    () => { GameWorld.SetState(GameState.MainMenu); }
                ));
            /* settingsButtons.Add(new Button(MenuButtons.KeyBindDown));
             settingsButtons.Add(new Button(MenuButtons.KeyBindLeft));
             settingsButtons.Add(new Button(MenuButtons.KeyBindRight));*/
            //Keybind up
            var keyBindUpSpr = content.Load<Texture2D>("Images/1366x768");
            settingsButtons.Add(new Button(
                keyBindUpSpr,
                keyBindUpSpr,
                new Vector2(-keyBindUpSpr.Width / 2, -250),
                () =>
                {
                    GameWorld.MenuScreenManager.SwappingKeyBind = true;
                    GameWorld.MenuScreenManager.ChosenKeyToRebind = PlayerBind.Up;
                }));
            //Res down
            var resDown = content.Load<Texture2D>("Images/800x600");
            settingsButtons.Add(new Button(
                resDown,
                resDown,
                new Vector2(-550, -50),
                () => { GameWorld.MenuScreenManager.ElementAtNumber--; }
                ));
            //Res up
            var resUp = content.Load<Texture2D>("Images/800x600");
            settingsButtons.Add(new Button(
                resUp,
                resUp,
                new Vector2(150, -50),
                () => { GameWorld.MenuScreenManager.ElementAtNumber++; }
                ));
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
