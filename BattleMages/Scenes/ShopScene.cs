﻿using System;
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
        private Scene oldScene;
        private Texture2D background;
        private SpriteFont font;
        private SpriteFont titleFont;

        private AttributeRune selectedRune;

        private UITab upperTab;
        private UITab lowerLeftTab;
        private UITab lowerRightTab;

        private string descriptionText;

        //TO DO:
        //connect "currentMoney" to master's "playerGold".
        //-add the rune to the spellbook's rune slots when bought.
        //fix weird purchase method

        public ShopScene(Scene oldScene)
        {
            upperTab = new UITab(this, GameWorld.Camera.Position - GameWorld.GameSize / 2 + new Vector2(12, 12), new Vector2(181, 100));
            lowerLeftTab = new UITab(this, GameWorld.Camera.Position - GameWorld.GameSize / 2 + new Vector2(12, 129), new Vector2(103, 39));
            lowerRightTab = new UITab(this, GameWorld.Camera.Position - GameWorld.GameSize / 2 + new Vector2(124, 129), new Vector2(69, 39));

            descriptionText = "Click a rune to select it.";

            var content = GameWorld.Instance.Content;
            this.oldScene = oldScene;
            background = content.Load<Texture2D>("Textures/Backgrounds/ShopKeeperbg");
            font = content.Load<SpriteFont>("FontBM");
            titleFont = content.Load<SpriteFont>("TitleFont");

            //Buy Button
            var shopButton = content.Load<Texture2D>("Textures/UI/Spellbook/buyButton");
            var shopButton_hover = content.Load<Texture2D>("Textures/UI/Spellbook/buyButtonHL");
            lowerRightTab.AddObject(ObjectBuilder.BuildButton(lowerRightTab.BotLeft + new Vector2(lowerRightTab.Size.X / 2, -8),
                shopButton,
                shopButton_hover,
                 () =>
                 {
                     if (selectedRune != null)
                     {
                         if (GameWorld.State.PlayerGold >= selectedRune.CostInShop)
                         {
                             GameWorld.State.PlayerGold -= selectedRune.CostInShop;
                             GameWorld.State.AvailableRunes.Add(selectedRune);
                             selectedRune = null;
                             descriptionText = "The Grand Wizard thanks you for your purchase.";
                             GameWorld.SoundManager.PlaySound("GetCoinSound");
                             RefreshRuneList();
                         }
                     }
                     else
                     {
                         descriptionText = "No rune selected.";
                     }
                 }, centered: true));

            RefreshRuneList();
        }

        private void RefreshRuneList()
        {
            upperTab.Clear();

            int nextRuneX = 0;
            int nextRuneY = 0;
            int nextRunePos = 0;

            var btnTex = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/smallButton");
            var btnTexHover = GameWorld.Load<Texture2D>("Textures/UI/Spellbook/SmallButtonHL");

            foreach (AttributeRune attrRune in StaticData.AttributeRunes)
            {
                if (GameWorld.State.AvailableRunes.Contains(attrRune)) continue;
                AttributeRune thisRune = attrRune;

                Vector2 pos = upperTab.TopLeft + new Vector2(4 + nextRuneX, 4 + nextRuneY);
                GameObject runeObj = new GameObject(pos + Vector2.One * 8);
                runeObj.AddComponent(new Button(
                    btnTex,
                    btnTexHover,
                    () =>
                    {
                        selectedRune = thisRune;
                        descriptionText = selectedRune.Name + Environment.NewLine + selectedRune.Description;
                    },
                    centered: true));
                runeObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisRune.TextureName));
                upperTab.AddObject(runeObj);

                nextRuneX += 16;
                nextRunePos++;
                if (nextRunePos % 8 == 0)
                {
                    nextRuneX = 0;
                    nextRuneY += 16;
                }
            }
        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);

            //Draws the description
            var descriptionPos = lowerLeftTab.TopLeft + Vector2.One * 4;
            drawer[DrawLayer.UI].DrawString(font, Utils.WarpText(descriptionText, (int)lowerLeftTab.Size.X - 8, font), descriptionPos, textColor);

            if (selectedRune != null)
            {
                //Draws costs of runes
                var costPos = lowerRightTab.TopLeft + new Vector2(4, 14);
                drawer[DrawLayer.UI].DrawString(font, "Cost: " + selectedRune.CostInShop, costPos, textColor);
            }

            //Draws player's current money
            var currentMoneyPos = lowerRightTab.TopLeft + new Vector2(4, 4);
            drawer[DrawLayer.UI].DrawString(font, "Gold: " + GameWorld.State.PlayerGold, currentMoneyPos, textColor);

            var buyTextPos = lowerRightTab.BotLeft + new Vector2(lowerRightTab.Size.X / 2 - 8, -14);
            drawer[DrawLayer.UI].DrawString(font, "Buy", buyTextPos, textColor);

            //background
            drawer[DrawLayer.Background].Draw(background, GameWorld.Camera.Position - GameWorld.GameSize / 2);

            base.Draw(drawer);
        }

        public override void Update()
        {
            base.Update();

            if (GameWorld.KeyPressed(Keys.Escape) || GameWorld.KeyPressed(Keys.Tab))
            {
                GameWorld.ChangeScene(oldScene);
            }
        }
    }
}