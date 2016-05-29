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
        private Texture2D runeSprite;
        private SpriteFont font;
        private SpriteFont titleFont;
        private KeyboardState keyState;

        //bool for returning to the lobby after entering the shop
        private bool cPressed;

        //vectors for the rune positions
        private Vector2 runeButtonPos;
        private Vector2 runeButtonPos1;
        private Vector2 runeButtonPos2;
        private Vector2 runeButtonPos3;
        private Vector2 runeButtonPos4;


        public ShopScene(Scene oldScene)
        {
            //TODO we need to make all the buttons

            var content = GameWorld.Instance.Content;
            this.oldScene = oldScene;
            CreateRuneButtons();
            background = content.Load<Texture2D>("Backgrounds/Shop");
            runeSprite = content.Load<Texture2D>("Images/Button_Rune");
            font = content.Load<SpriteFont>("FontBM");
            titleFont = content.Load<SpriteFont>("TitleFont");

            //positions of the rune buttons
            runeButtonPos = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 18);
            runeButtonPos1 = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 36);
            runeButtonPos2 = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 54);
            runeButtonPos3 = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 72);
            runeButtonPos4 = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 90);

        }

        /// <summary>
        /// Rune slot buttons
        /// </summary>
        private void CreateRuneButtons()
        {
            var content = GameWorld.Instance.Content;

            
            var runeBtnSpr1 = content.Load<Texture2D>("Images/Button_Rune");
            var runeBtnSpr2 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(runeButtonPos, runeBtnSpr1, runeBtnSpr2,
             () => { }, null, false));

            var runeBtnSpr3 = content.Load<Texture2D>("Images/Button_Rune");
            var runeBtnSpr4 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(runeButtonPos1, runeBtnSpr3, runeBtnSpr4,
             () => { }, null, false));

            var runeBtnSpr5 = content.Load<Texture2D>("Images/Button_Rune");
            var runeBtnSpr6 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(runeButtonPos2, runeBtnSpr5, runeBtnSpr6,
             () => { }, null, false));

            var runeBtnSpr7 = content.Load<Texture2D>("Images/Button_Rune");
            var runeBtnSpr8 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(runeButtonPos3, runeBtnSpr7, runeBtnSpr8,
             () => { }, null, false));

            var runeBtnSpr9 = content.Load<Texture2D>("Images/Button_Rune");
            var runeBtnSpr10 = content.Load<Texture2D>("Images/Button_Rune_Hover");
            AddObject(ObjectBuilder.BuildButton(runeButtonPos4, runeBtnSpr9, runeBtnSpr10,
             () => { }, null, false));

        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);

            //Text
            drawer[DrawLayer.UI].DrawString(font, "Damage rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 20), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Cooldown rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 40), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Mana  rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 58), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "nameless rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 76), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "nameless rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 94), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Description", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 70, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 120), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Cost", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 250, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Buy", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 150, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15), Color.Black);

            //background
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));
         
            base.Draw(drawer);
        }

        public override void Update()
        {
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
