using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class IngameUI : Component, ICanBeDrawn, ICanBeLoaded
    {
        private Texture2D healthBarSprite;
        private Texture2D manaBarSprite;
        private Texture2D spellOneSprite;
        private Texture2D spellTwoSprite;
        private Texture2D spellThreeSprite;
        private Texture2D spellFourSprite;
        private Texture2D coinsSprite;


        public IngameUI(GameObject gameObject) : base(gameObject)
        {
            
        }
        public void LoadContent(ContentManager content)
        {
            healthBarSprite = content.Load<Texture2D>("images/healthBar");
            manaBarSprite = content.Load<Texture2D>("images/manaBar");
            spellOneSprite = content.Load<Texture2D>("images/healthBar");
            spellTwoSprite = content.Load<Texture2D>("images/healthBar");
            spellThreeSprite = content.Load<Texture2D>("images/healthBar");
            spellFourSprite = content.Load<Texture2D>("images/healthBar");
            coinsSprite = content.Load<Texture2D>("images/healthBar");
        }
        public void Draw(Drawer drawer)
        {
            Vector2 topLeft = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));

            drawer[DrawLayer.UI].Draw(healthBarSprite, position: topLeft);
            drawer[DrawLayer.UI].Draw(manaBarSprite, position: new Vector2(topLeft.X, topLeft.Y + healthBarSprite.Height));
            drawer[DrawLayer.UI].Draw(spellOneSprite, position: GameWorld.Camera.Position);
            drawer[DrawLayer.UI].Draw(spellTwoSprite, position: GameWorld.Camera.Position);
            drawer[DrawLayer.UI].Draw(spellThreeSprite, position: GameWorld.Camera.Position);
            drawer[DrawLayer.UI].Draw(spellFourSprite, position: GameWorld.Camera.Position);
            drawer[DrawLayer.UI].Draw(coinsSprite, position: GameWorld.Camera.Position);

        }


    }
}
