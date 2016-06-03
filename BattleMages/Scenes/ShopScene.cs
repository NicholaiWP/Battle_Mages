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
        private Scene oldScene;
        private Texture2D background;
        private SpriteFont font;
        private SpriteFont titleFont;

        private AttributeRune selectedRune;

        //bool for returning to the lobby after entering the shop
        private bool cPressed;

        //TO DO:
        //connect "currentMoney" to master's "playerGold".
        //-add the rune to the spellbook's rune slots when bought.
        //fix weird purchase method

        public ShopScene(Scene oldScene)
        {
            var content = GameWorld.Instance.Content;
            this.oldScene = oldScene;
            background = content.Load<Texture2D>("Backgrounds/ShopKeeperbg");
            font = content.Load<SpriteFont>("FontBM");
            titleFont = content.Load<SpriteFont>("TitleFont");

            int nextRuneX = 0;
            int nextRuneY = 0;
            int nextRunePos = 0;

            var btnTex = content.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var btnTexHover = content.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");

            foreach (AttributeRune attrRune in StaticData.AttributeRunes)
            {
                AttributeRune thisRune = attrRune;

                Vector2 pos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 16 + nextRuneX, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15 + nextRuneY);

                GameObject runeObj = new GameObject(pos + Vector2.One * 8);
                runeObj.AddComponent(new Button(
                    btnTex,
                    btnTexHover,
                    () =>
                    {
                        selectedRune = thisRune;
                    },
                    centered: true));
                runeObj.AddComponent(new SpriteRenderer(StaticData.RuneImagePath + thisRune.TextureName));
                AddObject(runeObj);

                nextRuneX += 16;
                nextRunePos++;
                if (nextRunePos % 8 == 0)
                {
                    nextRuneX = 0;
                    nextRuneY += 16;
                }
            }
            //Buy Button
            var shopButton = content.Load<Texture2D>("Textures/UI/Spellbook/SmallButton");
            var shopButton_hover = content.Load<Texture2D>("Textures/UI/Spellbook/SmallButton_Hover");
            AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 170, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 50),
                shopButton,
                shopButton_hover,
             () =>
             {
                 if (GameWorld.State.PlayerGold >= selectedRune.CostInShop)
                 {
                     GameWorld.State.PlayerGold -= selectedRune.CostInShop;
                     //TODO: Add rune to the list of available runes
                 }
             }));
        }

        public override void Draw(Drawer drawer)
        {
            //Text
            Color textColor = new Color(120, 100, 80);
            drawer[DrawLayer.UI].DrawString(titleFont, "Description", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 50, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 128), textColor);
            drawer[DrawLayer.UI].DrawString(titleFont, "Your Money", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 50), textColor);
            drawer[DrawLayer.UI].DrawString(font, "Cost", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 168, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), textColor);

            if (selectedRune != null)
            {
                //Draws the descriptions
                var bottomLeftTextPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 145);
                drawer[DrawLayer.UI].DrawString(font, Utils.WarpText(selectedRune.Name + Environment.NewLine + selectedRune.Description, 150, font), bottomLeftTextPos, textColor);

                //Draws costs of runes
                var uppperTopTextPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 170, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 35);
                drawer[DrawLayer.UI].DrawString(font, selectedRune.CostInShop.ToString(), uppperTopTextPos, textColor);
            }

            //Draws player's current money
            var currentMoneyPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 70, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 70);
            drawer[DrawLayer.UI].DrawString(font, GameWorld.State.PlayerGold.ToString(), currentMoneyPos, textColor);

            //background
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));

            base.Draw(drawer);
        }

        public override void Update()
        {
            base.Update();

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.C))
            {
                if (!cPressed)
                    GameWorld.ChangeScene(oldScene);
            }
            else
            {
                if (cPressed)
                    cPressed = false;
            }
        }
    }
}