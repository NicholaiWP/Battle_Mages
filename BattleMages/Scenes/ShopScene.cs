using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class ShopScene : Scene
    {
        private Scene oldScene;
        private Texture2D background;
        private SpriteFont font;
        private SpriteFont titleFont;
        private Texture2D rune1;
        private Texture2D rune2;
        private Texture2D rune3;
        private int cost;
        private int currentMoney;
        private string bottomLeftText = string.Empty;
        private int buttonSelected;

        //bool for returning to the lobby after entering the shop
        private bool cPressed;

        //TO DO:
        //-then subtracts currentMoney from the rune's cost and adds the rune to the spellbook's rune slots.
        //-set the rune's Cost and write currentMoney into the game using perhaps a drawstring, convert the int's number to a string in this AboveUI draw sentence.
        //-On coin/money pick-up, add money to currentMoney counter.

        public ShopScene(Scene oldScene)
        {
            var content = GameWorld.Instance.Content;
            this.oldScene = oldScene;
            buttonSelected = -1;
            background = content.Load<Texture2D>("Backgrounds/ShopKeeperbg");
            font = content.Load<SpriteFont>("FontBM");
            titleFont = content.Load<SpriteFont>("TitleFont");
            //runes
            rune1 = content.Load<Texture2D>("Rune Images/rune1");
            rune2 = content.Load<Texture2D>("Rune Images/rune2");
            rune3 = content.Load<Texture2D>("Rune Images/rune3");

            int xPos = 0;
            int yPos = 0;
           
            foreach (AttributeRune attrRune in StaticData.AttributeRunes)
            {
                var Button_Rune = content.Load<Texture2D>("Images/Button_Rune");
                var Button_Rune_Hover = content.Load<Texture2D>("Images/Button_Rune_Hover");
                AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 16 + xPos * 16, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15 + yPos * 16), Button_Rune, Button_Rune_Hover,
                    () =>
                    {
                        buttonSelected = StaticData.AttributeRunes.IndexOf(attrRune);
                        cost = attrRune.RuneCost;
                        currentMoney = attrRune.CurrentMoney;
                        bottomLeftText = attrRune.Description;

                        if (buttonSelected == 0)
                        {
                            cost = 300;
                        }
                        else if (buttonSelected == 1)
                        {
                            cost = 200;
                        }
                        else if (buttonSelected == 2)
                        {
                            cost = 150;
                        }

                    },
                    null, false));

                xPos++;
                if (xPos >= 2)
                {
                    yPos++;
                    xPos = 0;
                }
             
                    //Buy Button
                    var shopButton = content.Load<Texture2D>("Images/Button_Rune");
                    var shopButton_hover = content.Load<Texture2D>("Images/Button_Rune_Hover");
                    AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 170, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 50), shopButton, shopButton_hover,
                     () =>
                     {
                         if(currentMoney >= cost)
                         {
                             attrRune.CurrentMoney -= cost;
                         }
                       
                       else if (currentMoney < 0)
                         {
                             currentMoney = 0;
                         }
                     },
                     null, false));               
            }
        }
     
        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);

            //Text
            drawer[DrawLayer.UI].DrawString(titleFont, "Description", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 50, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 128), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Your Money", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 50), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Cost", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 168, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), Color.Black);

            //Draws the descriptions
            var bottomLeftTextPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 145);
            drawer[DrawLayer.UI].DrawString(font, bottomLeftText, bottomLeftTextPos, Color.Black);

            //Draws costs of runes
            var uppperTopTextPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 170, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 35);
            drawer[DrawLayer.UI].DrawString(font,  cost.ToString(), uppperTopTextPos, Color.Black);

            //Draws player's current money
            var currentMoneyPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 70, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 70);
            drawer[DrawLayer.UI].DrawString(font, currentMoney.ToString(), currentMoneyPos, Color.Black);

                

            //Runes
            drawer[DrawLayer.UI].Draw(rune1, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 22, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), Color.White);
            drawer[DrawLayer.UI].Draw(rune3, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 22, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 36), Color.White);
            drawer[DrawLayer.UI].Draw(rune2, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 37, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), Color.White);
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