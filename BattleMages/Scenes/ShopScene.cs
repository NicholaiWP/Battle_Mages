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
        private AttributeRune selectedAttrRune;
        private SpellInfo currentlyEditing;
        private string bottomLeftText1 = string.Empty;
        private string bottomLeftText2 = string.Empty;
        private string bottomLeftText3 = string.Empty;
        private int cost;

        //bool for returning to the lobby after entering the shop
        private bool cPressed;

        public int Cost { get { return cost; } set { cost = value; } }

        public ShopScene(Scene oldScene)
        {
           
            var content = GameWorld.Instance.Content;

            this.oldScene = oldScene;

            bottomLeftText1 = StaticData.AttributeRunes[0].Description;
            bottomLeftText2 = StaticData.AttributeRunes[1].Description;
            bottomLeftText3 = StaticData.AttributeRunes[2].Description;

            background = content.Load<Texture2D>("Backgrounds/Shop");
            font = content.Load<SpriteFont>("FontBM");
            titleFont = content.Load<SpriteFont>("TitleFont");

            //runes
            rune1 = content.Load<Texture2D>("Rune Images/rune1");
            rune2 = content.Load<Texture2D>("Rune Images/rune2");
            rune3 = content.Load<Texture2D>("Rune Images/rune3");


               
            var Button_Rune = content.Load<Texture2D>("Images/Button_Rune");
            var Button_Rune_Hover = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 15, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15), Button_Rune, Button_Rune_Hover,
                () =>
                {
                  
                  
                },
                null, false));
                 
               
            var Button_Rune1 = content.Load<Texture2D>("Images/Button_Rune");
            var Button_Rune_Hover1 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 15, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 34), Button_Rune1, Button_Rune_Hover1,
                () =>
                {
                   
                },
                null, false));
                
             
            var Button_Rune2 = content.Load<Texture2D>("Images/Button_Rune");
            var Button_Rune_Hover2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 15, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 54), Button_Rune2, Button_Rune_Hover2,
                () =>
                {
                    
                }, 
                null, false));
             
      }
        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);

            //Text
            drawer[DrawLayer.UI].DrawString(font, "Rune of might", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Rune of haste", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 40), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Rune of persistence", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 58), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Description", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 70, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 120), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Cost", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 250, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15), Color.Black);

            //drawer[DrawLayer.UI].DrawString(font, StaticData.AttributeRunes[0].Description, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 140), Color.Black);
            //drawer[DrawLayer.UI].DrawString(font, StaticData.AttributeRunes[1].Description, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 140), Color.Black);
            //drawer[DrawLayer.UI].DrawString(font, StaticData.AttributeRunes[2].Description, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 140), Color.Black);

            //Runes
            drawer[DrawLayer.UI].Draw(rune1, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), Color.White);
            drawer[DrawLayer.UI].Draw(rune3, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 40), Color.White);
            drawer[DrawLayer.UI].Draw(rune2, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 60), Color.White);
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

