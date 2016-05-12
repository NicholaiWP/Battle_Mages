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
   public class PausedGameScreen
    {
        private Texture2D pauseTexture;
        private Vector2 position;
        private List<Button> PauseButton = new List<Button>();

        public PausedGameScreen()
        {
            
            ContentManager content = GameWorld.Instance.Content;
            pauseTexture = content.Load<Texture2D>("Images/paused_bMages");
            
            //Continue button in pause screen
                var ContinueSpr1 = content.Load<Texture2D>("Images/BMContinueButton1");
                var ContinueSpr2 = content.Load<Texture2D>("Images/BMContinueButton_hover7");
                PauseButton.Add(new Button(
               ContinueSpr1,
               ContinueSpr2,
               new Vector2(-ContinueSpr1.Width / 2, ContinueSpr1.Height * -1f),
               () => { GameWorld.Instance.Paused = false; },
               false));

                //Quit button for pause screen
                var QuitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
                var QuitSpr2 = content.Load<Texture2D>("Images/BMQuitButton_Hover");
                PauseButton.Add(new Button(
                    QuitSpr1,
                    QuitSpr2,
                    new Vector2(-QuitSpr1.Width / 2, 0),
                    () => { GameWorld.SetState(GameState.Quit); },
                    false
                    ));

            //Forfeit button for pause screen
            var ForfeitSpr1 = content.Load<Texture2D>("Images/forfeit_button");
            var ForfeitSpr2 = content.Load<Texture2D>("Images/forfeit_hover_red2");
            PauseButton.Add(new Button(
                ForfeitSpr1,
                ForfeitSpr2,
                 new Vector2(-ForfeitSpr1.Width / 2, ForfeitSpr1.Height * 1f),
                () => { GameWorld.SetState(GameState.Lobby); GameWorld.Instance.LobbyS.SetPosition(new Vector2(-100, -100)); },
                false
                ));
        }

        public void DrawPause(SpriteBatch spriteBatch)
        {
           spriteBatch.Draw(pauseTexture, position, Color.White);

            foreach (Button button in PauseButton)
            {
                button.Draw(spriteBatch);
            }
        }

        public void Update()
        {
                position = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2);

            foreach  (Button button in PauseButton)
            {
                button.UpdatePosition(GameWorld.Camera.Position);

                button.Update();
            }
        }
    }
}
