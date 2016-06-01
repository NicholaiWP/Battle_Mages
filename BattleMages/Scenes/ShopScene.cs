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
        private Texture2D rune1;
        private Texture2D rune2;
        private Texture2D rune3;
        private string bottomLeftText = string.Empty;
        private string upperRightText = string.Empty;
        private Dictionary<string, Vector2> descriptionPlacement = new Dictionary<string, Vector2>();
        private int runeCost;
        private int StartingMoney;
        private int currentMoney;

        //bool for returning to the lobby after entering the shop
        private bool cPressed;

        public int Cost { get { return runeCost; } set { runeCost = value; } }

        public int StartingMoney1 {get {return StartingMoney;} set {StartingMoney = value;}}

        public int CurrentMoney { get {return currentMoney;} set {currentMoney = value;}}

        //TO DO:
        //-check whether the rune/button is selected
        //-then subtracts currentMoney from the rune's cost and adds the rune to the spellbook's rune slots.
        //-set the rune's Cost and write currentMoney into the game using perhaps a drawstring, convert the int's number to a string in this AboveUI draw sentence.
        //-On coin/money pick-up, add money to currentMoney counter.

        public ShopScene(Scene oldScene)
        {
            
            var content = GameWorld.Instance.Content;
            this.oldScene = oldScene;
            StartingMoney1 = 50;
            CurrentMoney -= Cost;

            background = content.Load<Texture2D>("Backgrounds/Shop");
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
                            bottomLeftText = attrRune.Description;
                            upperRightText = attrRune.RuneCost.ToString();
                        },
                        null, false));

                    xPos++;
                    if (xPos >= 2)
                    {
                        yPos++;
                        xPos = 0;
                  
                    }

                if (Cost > currentMoney)
                {
                    GameObject dialougeObj = new GameObject(Vector2.Zero);
                    dialougeObj.AddComponent(new DialougeBox("You do not have sufficient money to buy this item, please try again once you've earned some more"));
                    AddObject(dialougeObj);
                }
                if (Cost <= currentMoney)
                {
                    //Buy Button
                    var shopButton = content.Load<Texture2D>("Images/Button_Rune");
                    var shopButton_hover = content.Load<Texture2D>("Images/Button_Rune_Hover");
                    AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 150, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 80), shopButton, shopButton_hover,
                     () =>
                     {
                         //if(selectedRune != SelectedRune.None)
                         {

                         }
                     },
                     null, false));
                }
                  
        }   

      
          
     }
        

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);

            //Text
            drawer[DrawLayer.UI].DrawString(titleFont, "Description", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 70, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 120), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Purchase", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 130, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 60), Color.Black);

            drawer[DrawLayer.UI].DrawString(titleFont, "Cost", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 140, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15), Color.Black);

            //Draws the descriptions
            var bottomLeftTextPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 140);
            drawer[DrawLayer.UI].DrawString(font, bottomLeftText, bottomLeftTextPos, Color.Black);

            var uppperTopTextPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 150, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 35);
            drawer[DrawLayer.UI].DrawString(font, upperRightText, uppperTopTextPos, Color.Black);

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