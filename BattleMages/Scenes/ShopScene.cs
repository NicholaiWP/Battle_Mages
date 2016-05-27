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
        private Vector2 itemPosition = GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 18, (GameWorld.GameHeight / 2) - 5);
        private Vector2[] shopItemPositions;
        KeyboardState oldKbState = Keyboard.GetState();
        KeyboardState kbState = Keyboard.GetState();
        private List<GameObject> runeList = new List<GameObject>();

        public ShopScene(Scene oldScene)
        {
            this.oldScene = oldScene;
            var content = GameWorld.Instance.Content;
            background = content.Load<Texture2D>("Backgrounds/Shop");
            runeSprite = content.Load<Texture2D>("Images/Button_Rune");
            font = content.Load<SpriteFont>("FontBM");

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

            base.Draw(drawer);
        }

        public override void Update()
        {
        }

    }
}
