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

        public SettingsWindow()
        {
            var content = GameWorld.Instance.Content;
            //Back button
            var backSpr = content.Load<Texture2D>("Images/Back");
            settingsButtons.Add(new Button(
                    backSpr,
                    backSpr,
                    new Vector2(-backSpr.Width / 2, backSpr.Height * 2f),
                    () => { GameWorld.SetState(GameState.MainMenu); }
                ));
            /* settingsButtons.Add(new Button(MenuButtons.KeyBindDown));
             settingsButtons.Add(new Button(MenuButtons.KeyBindLeft));
             settingsButtons.Add(new Button(MenuButtons.KeyBindRight));*/
            //Keybind up
            var keyBindUpSpr = content.Load<Texture2D>("Images/Rebind");
            settingsButtons.Add(new Button(
                keyBindUpSpr,
                keyBindUpSpr,
                new Vector2(-keyBindUpSpr.Width / 2, -78),
                () =>
                {
                    GameWorld.MenuScreenManager.SwappingKeyBind = true;
                    GameWorld.MenuScreenManager.ChosenKeyToRebind = PlayerBind.Up;
                }));
            //Res down
            var resDown = content.Load<Texture2D>("Images/ResDown");
            settingsButtons.Add(new Button(
                resDown,
                resDown,
                new Vector2(-64 - resDown.Width / 2, -50),
                () => { GameWorld.MenuScreenManager.ElementAtNumber--; }
                ));
            //Res up
            var resUp = content.Load<Texture2D>("Images/ResUp");
            settingsButtons.Add(new Button(
                resUp,
                resUp,
                new Vector2(64 - resUp.Width / 2, -50),
                () => { GameWorld.MenuScreenManager.ElementAtNumber++; }
                ));
        }

        public void LoadContent(ContentManager content)
        {
            fontBM = content.Load<SpriteFont>("FontBM");
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

            spriteBatch.DrawString(fontBM, GameWorld.MenuScreenManager.CurrentResolutionString, Vector2.Zero,
                Color.Black);
            spriteBatch.DrawString(fontBM,
                GameWorld.PlayerControls.KeyToString(GameWorld.PlayerControls.GetBinding(PlayerBind.Up)),
                new Vector2(64, -72), Color.Black);
        }
    }
}
