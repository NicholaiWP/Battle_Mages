using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages.Scenes
{
    class ShopScene : Scene
    {
        private Texture2D background;
        private SpriteFont spriteFont;
        private Vector2 bgPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);

        private Scene oldScene;

        public ShopScene(Scene oldScene)
        {

        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);
        }

    }
}
