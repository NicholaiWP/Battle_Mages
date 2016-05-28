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
        private Vector2 bgPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2);
        private Texture2D runeSprite;
        private SpriteFont font;
        private SpriteFont titleFont;
        private Vector2 itemPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 18, (GameWorld.GameHeight / 2) - 5);
        private List<GameObject> runeList = new List<GameObject>();
        private KeyboardState keyState;

        public ShopScene(Scene oldScene)
        {
            var content = GameWorld.Instance.Content;
            Update();
            this.oldScene = oldScene;
            background = content.Load<Texture2D>("Backgrounds/Shop");
            runeSprite = content.Load<Texture2D>("Images/Button_Rune");
            font = content.Load<SpriteFont>("FontBM");
            titleFont = content.Load<SpriteFont>("TitleFont");

        }
            
        private void AddShopItem (GameObject go)
        {
            runeList.Add(go);
            AddObject(go);
        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);
            drawer[DrawLayer.UI].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));
            drawer[DrawLayer.UI].Draw(runeSprite, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 18));
            drawer[DrawLayer.UI].Draw(runeSprite, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 36));
            drawer[DrawLayer.UI].Draw(runeSprite, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 54));
            drawer[DrawLayer.UI].Draw(runeSprite, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 72));
            drawer[DrawLayer.UI].Draw(runeSprite, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 20, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 90));
            drawer[DrawLayer.UI].DrawString(font, "Damage rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight/ 2+20), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Cooldown rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 40), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "Mana  rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 58), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "nameless rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 76), Color.Black);
            drawer[DrawLayer.UI].DrawString(font, "nameless rune", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 40, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 94), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Description", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 70, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 120), Color.Black);
            drawer[DrawLayer.UI].DrawString(titleFont, "Cost", new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2 + 250, GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2 + 15), Color.Black);




            base.Draw(drawer);
        }

        public override void Update()
        {
            if(GameWorld.Scene is ShopScene)
            {
                keyState = Keyboard.GetState();

                if (keyState.IsKeyDown(Keys.C))
                {
                    GameWorld.ChangeScene(new LobbyScene(GameWorld.Scene));
                }
            }
        }

    }
}
