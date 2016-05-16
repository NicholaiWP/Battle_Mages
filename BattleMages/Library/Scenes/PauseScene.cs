﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class PauseScene : Scene
    {
        private Texture2D pauseTexture;
        private Vector2 position;

        public PauseScene(Scene oldScene)
        {
            var content = GameWorld.Instance.Content;
            pauseTexture = content.Load<Texture2D>("Images/paused_bMages");

            //Continue button in pause screen
            var ContinueSpr1 = content.Load<Texture2D>("Images/BMContinueButton1");
            var ContinueSpr2 = content.Load<Texture2D>("Images/BMContinueButton_hover7");
            AddObject(new Button(
           ContinueSpr1,
           ContinueSpr2,
           new Vector2(-ContinueSpr1.Width / 2, ContinueSpr1.Height * -1f),
           () => { GameWorld.ChangeScene(oldScene); },
           false));

            //Quit button for pause screen
            var QuitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
            var QuitSpr2 = content.Load<Texture2D>("Images/BMQuitButton_Hover");
            AddObject(new Button(
                QuitSpr1,
                QuitSpr2,
                new Vector2(-QuitSpr1.Width / 2, 0),
                () => { GameWorld.Instance.Exit(); },
                false
                ));

            if (oldScene is GameScene)
            {
                //Forfeit button for pause screen
                var ForfeitSpr1 = content.Load<Texture2D>("Images/forfeit_button");
                var ForfeitSpr2 = content.Load<Texture2D>("Images/forfeit_hover_red2");
                AddObject(new Button(
                    ForfeitSpr1,
                    ForfeitSpr2,
                     new Vector2(-ForfeitSpr1.Width / 2, ForfeitSpr1.Height * 1f),
                    () => { GameWorld.ChangeScene(new LobbyScene()); },
                    false
                    ));
            }

            ProcessObjectLists();

            foreach (GameObject go in ActiveObjects)
            {
                go.Update();
            }
        }

        public override void Update()
        {
            position = new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2);

            foreach (GameObject go in ActiveObjects)
            {
                go.Update();
            }
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(pauseTexture, position, Color.White);

            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}