using System;
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
        private GameScene gameScene;

        public Texture2D healthBar;
        private Texture2D manaBar;
        private Texture2D spellbarBgTex;
        private Texture2D spellTwoSprite;
        private Texture2D spellThreeSprite;
        private Texture2D spellFourSprite;
        private Texture2D coinsSprite;
        private Texture2D waveCount;
        private Texture2D aboveUI;
        private Texture2D behindUI;

        float healthbarSize = 1f;
        float manabarSize = 1f;

        Player player;

        private SpriteFont haxFont;

        public IngameUI() : base()
        {
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            behindUI = GameWorld.Load<Texture2D>("images/behindUI");
            aboveUI = GameWorld.Load<Texture2D>("images/aboveUI");
            haxFont = GameWorld.Load<SpriteFont>("FontBM");
            healthBar = GameWorld.Load<Texture2D>("images/healthBar");
            manaBar = GameWorld.Load<Texture2D>("images/manaBar");
            spellbarBgTex = GameWorld.Load<Texture2D>("images/spellSpriteBackground1");
            spellTwoSprite = GameWorld.Load<Texture2D>("images/spellSpriteBackground1");
            spellThreeSprite = GameWorld.Load<Texture2D>("images/spellSpriteBackground1");
            spellFourSprite = GameWorld.Load<Texture2D>("images/spellSpriteBackground1");
            coinsSprite = GameWorld.Load<Texture2D>("images/coinsSprite");
            
        }

        private void Update(UpdateMsg msg)
        {
            player = GameWorld.Scene.ActiveObjects.Select(a => a.GetComponent<Player>()).Where(a => a != null).FirstOrDefault();
            if (player != null)
            {
                healthbarSize = Math.Max(0, MathHelper.Lerp(healthbarSize, player.CurrentHealth / (float)Player.MaxHealth, GameWorld.DeltaTime * 10f));
                manabarSize = Math.Max(0,MathHelper.Lerp(manabarSize, player.CurrentMana / Player.MaxMana, GameWorld.DeltaTime * 10f));
            }
        }

        private void Draw(DrawMsg msg)
        {
            int offset = 6;
            int halfOffset = offset / 2;
            Vector2 topLeft = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            Vector2 topRight = GameWorld.Camera.Position - new Vector2((-GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));

            Vector2 healthBarPos = new Vector2(topLeft.X + 13, topLeft.Y + 2);
            Vector2 manaBarPos = new Vector2(topLeft.X + 2, topLeft.Y + 15);

            Vector2 bottomMiddle = GameWorld.Camera.Position - new Vector2(0, ((-GameWorld.GameHeight / 2 + offset) + spellbarBgTex.Height));

            //Player player = GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Player>()).Where(a => a != null).FirstOrDefault();
            //if (player != null)
            //{
            //    drawer[DrawLayer.UI].DrawString(haxFont, "Health: " + player.Health, topLeft, Color.Purple);
            //}

            if (player != null)
                msg.Drawer[DrawLayer.Gameplay].Draw(behindUI, position: topLeft);
            {
                msg.Drawer[DrawLayer.UI].Draw(healthBar, position: healthBarPos, scale: new Vector2(healthbarSize,1));
                msg.Drawer[DrawLayer.UI].Draw(manaBar, position: manaBarPos, scale: new Vector2(manabarSize,1));
            }

            Vector2 spellBarCenter = GameWorld.Camera.Position + new Vector2(0, GameWorld.GameHeight / 2 - spellbarBgTex.Height / 2 - 8);

            int space = spellbarBgTex.Width + 8;
            int num = GameWorld.State.SpellBar.Count;

            for (int i=0;i<num;i++)
            {
                SpellInfo spell = GameWorld.State.SpellBook[GameWorld.State.SpellBar[i]];
                
                Vector2 pos = new Vector2(spellBarCenter.X - (space * num)/2f + space * i, spellBarCenter.Y);
                msg.Drawer[DrawLayer.UI].Draw(spellbarBgTex, position: pos - GetHalfSize(spellbarBgTex));

                Texture2D baseRuneTex = spell.GetBaseRune().Texture;
                msg.Drawer[DrawLayer.UI].Draw(baseRuneTex, position: pos - GetHalfSize(baseRuneTex));

                Vector2[] runePositions = new Vector2[] { new Vector2(0, -6), new Vector2(6, 0), new Vector2(0, 6), new Vector2(-6, 0) };
                for (int j=0;j<SpellInfo.AttributeRuneSlotCount;j++)
                {
                    AttributeRune attrRune = spell.GetAttributeRune(j);
                    if (attrRune != null)
                    {
                        msg.Drawer[DrawLayer.UI].Draw(attrRune.Texture, position: pos + runePositions[j] - GetHalfSize(attrRune.Texture));
                    }
                }
            }
            //msg.Drawer[DrawLayer.UI].Draw(GameWorld.State.SpellBook[GameWorld.State.SpellBar[0]].GetBaseRune().Texture, position: new Vector2((bottomMiddle.X - (offset + halfOffset)) - (spellbarBgTex.Width * 2), bottomMiddle.Y));
            //msg.Drawer[DrawLayer.UI].Draw(spellTwoSprite, position: new Vector2((bottomMiddle.X - halfOffset) - (spellbarBgTex.Width), bottomMiddle.Y));
            //msg.Drawer[DrawLayer.UI].Draw(spellThreeSprite, position: new Vector2((bottomMiddle.X + halfOffset), bottomMiddle.Y));
            //msg.Drawer[DrawLayer.UI].Draw(spellFourSprite, position: new Vector2((bottomMiddle.X + (offset + halfOffset)) + (spellbarBgTex.Width), bottomMiddle.Y));

            msg.Drawer[DrawLayer.UI].Draw(coinsSprite, position: new Vector2(topRight.X - (coinsSprite.Width + offset), topRight.Y));
        }

        Vector2 GetHalfSize(Texture2D tex)
        {
            return new Vector2(tex.Width, tex.Height) / 2f;
        }


    }
      
}