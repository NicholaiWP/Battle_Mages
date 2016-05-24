﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class IngameUI : Component
    {
        public Texture2D healthBar;
        private Texture2D manaBar;
        private Texture2D spellOneSprite;
        private Texture2D spellTwoSprite;
        private Texture2D spellThreeSprite;
        private Texture2D spellFourSprite;
        private Texture2D coinsSprite;
        

        private SpriteFont haxFont;

        public IngameUI(GameObject gameObject) : base(gameObject)
        {
            Listen<InitializeMsg>(Initialize);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            haxFont = GameWorld.Load<SpriteFont>("FontBM");
            healthBar = GameWorld.Load<Texture2D>("images/healthBar");
            manaBar = GameWorld.Load<Texture2D>("images/manaBar");
            spellOneSprite = GameWorld.Load<Texture2D>("images/BMspellSprite");
            spellTwoSprite = GameWorld.Load<Texture2D>("images/BMspellSprite");
            spellThreeSprite = GameWorld.Load<Texture2D>("images/BMspellSprite");
            spellFourSprite = GameWorld.Load<Texture2D>("images/BMspellSprite");
            coinsSprite = GameWorld.Load<Texture2D>("images/coinsSprite");
        }

        public void Draw(DrawMsg msg)
        {
            int offset = 6;
            int halfOffset = offset / 2;
            Vector2 topLeft = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            Vector2 topRight = GameWorld.Camera.Position - new Vector2((-GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            Vector2 bottomMiddle = GameWorld.Camera.Position - new Vector2(0, ((-GameWorld.GameHeight / 2 + offset) + spellOneSprite.Height));

            //Player player = GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Player>()).Where(a => a != null).FirstOrDefault();
            //if (player != null)
            //{
            //    drawer[DrawLayer.UI].DrawString(haxFont, "Health: " + player.Health, topLeft, Color.Purple);
            //}

            Player player = GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Player>()).Where(a => a != null).FirstOrDefault();
            if (player != null)
            {
                player.Health = healthBar.Width;
                msg.Drawer[DrawLayer.UI].Draw(healthBar,  destinationRectangle: new Rectangle((int)topLeft.X, (int)topLeft.Y, player.CurrentHealth, healthBar.Height), color: Color.White);
                msg.Drawer[DrawLayer.UI].Draw(manaBar, position: new Vector2(topLeft.X, topLeft.Y + healthBar.Height));

            }

            msg.Drawer[DrawLayer.UI].Draw(spellOneSprite, position: new Vector2((bottomMiddle.X - (offset + halfOffset)) - (spellOneSprite.Width * 2), bottomMiddle.Y));
            msg.Drawer[DrawLayer.UI].Draw(spellTwoSprite, position: new Vector2((bottomMiddle.X - halfOffset) - (spellOneSprite.Width), bottomMiddle.Y));
            msg.Drawer[DrawLayer.UI].Draw(spellThreeSprite, position: new Vector2((bottomMiddle.X + halfOffset), bottomMiddle.Y));
            msg.Drawer[DrawLayer.UI].Draw(spellFourSprite, position: new Vector2((bottomMiddle.X + (offset + halfOffset)) + (spellOneSprite.Width), bottomMiddle.Y));
            msg.Drawer[DrawLayer.UI].Draw(coinsSprite, position: new Vector2(topRight.X - (coinsSprite.Width + offset), topRight.Y));

        }

    }
      
}