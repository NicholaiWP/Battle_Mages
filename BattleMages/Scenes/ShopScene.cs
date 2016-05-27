using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
  public class ShopScene : Scene
    {
        private bool C_Pressed = true;
        private Texture2D background;
        private SpriteFont spriteFont;
        private LobbyScene lobbyScene;
        private Vector2 bgPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);

        public ShopScene()
        {
            Update();
        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);
        }

        public override void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.C))
            {
                if (C_Pressed)
                    GameWorld.ChangeScene(lobbyScene);
            }
            else
            {
                if (!C_Pressed)
                    C_Pressed = false;
            }

            base.Update();
        }

    }
}
