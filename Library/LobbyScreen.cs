using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battle_Mages
{
   public class LobbyScreen
    {
        private Texture2D lobbyTexture;
        private Vector2 lobbyPosition;

        public LobbyScreen()
        {
            ContentManager content = GameWorld.Instance.Content;
            lobbyTexture = content.Load<Texture2D>("Images/BMtavern");
            

        }

        public void DrawLobby(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(lobbyTexture, lobbyPosition, Color.White);
            
        }
        public void SetPosition(Vector2 pos)
        {
            lobbyPosition = pos;
        }
        public void Update()
        {

        }
    }
}
