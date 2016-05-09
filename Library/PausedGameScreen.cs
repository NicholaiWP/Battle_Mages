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

        public PausedGameScreen()
        {
            ContentManager content = GameWorld.Instance.Content;
            pauseTexture = content.Load<Texture2D>("Images/paused_bMages");
        }

        public void DrawPause(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pauseTexture, position, Color.White);
        }

        public void Update()
        {
                position = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2);
        }
    }
}
