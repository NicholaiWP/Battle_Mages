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
            spellOneSprite = content.Load<Texture2D>("images/BMspellSprite");
            spellTwoSprite = content.Load<Texture2D>("images/BMspellSprite");
            spellThreeSprite = content.Load<Texture2D>("images/BMspellSprite");
            spellFourSprite = content.Load<Texture2D>("images/BMspellSprite");
            coinsSprite = content.Load<Texture2D>("images/coinsSprite");
        }

        public void Draw(Drawer drawer)
        {
            int offset = 6;
            int halfOffset = offset / 2;
            Vector2 topLeft = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            Vector2 topRight = GameWorld.Camera.Position - new Vector2((- GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            Vector2 bottomMiddle = GameWorld.Camera.Position - new Vector2(0, ((- GameWorld.GameHeight / 2 + 5) + spellOneSprite.Height));
            drawer[DrawLayer.UI].Draw(healthBarSprite, position: topLeft);
            drawer[DrawLayer.UI].Draw(manaBarSprite, position: new Vector2(topLeft.X, topLeft.Y + healthBarSprite.Height));
            drawer[DrawLayer.UI].Draw(spellOneSprite, position: new Vector2((bottomMiddle.X - (offset + halfOffset))- (spellOneSprite.Width * 2), bottomMiddle.Y));
            drawer[DrawLayer.UI].Draw(spellTwoSprite, position: new Vector2((bottomMiddle.X - halfOffset)- (spellOneSprite.Width), bottomMiddle.Y));
            drawer[DrawLayer.UI].Draw(spellThreeSprite, position: new Vector2((bottomMiddle.X + halfOffset), bottomMiddle.Y));
            drawer[DrawLayer.UI].Draw(spellFourSprite, position: new Vector2((bottomMiddle.X + (offset + halfOffset))+ (spellOneSprite.Width), bottomMiddle.Y));
            drawer[DrawLayer.UI].Draw(coinsSprite, position: new Vector2(topRight.X - (coinsSprite.Width + offset), topRight.Y));

        }
    }
}