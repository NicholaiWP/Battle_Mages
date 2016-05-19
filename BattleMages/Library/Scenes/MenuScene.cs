using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class MenuScene : Scene
    {
        public MenuScene()
        {
            var content = GameWorld.Instance.Content;
            //Play button
            var playSpr1 = content.Load<Texture2D>("Images/BMPlayGameButton");
            var playSpr2 = content.Load<Texture2D>("Images/BMPlayGameButton_Hover");
            AddObject(new Button(
                playSpr1,
                playSpr2,
                new Vector2(GameWorld.Camera.Position.X - playSpr1.Width / 2, GameWorld.Camera.Position.Y + playSpr1.Height * -1f),
                () => { GameWorld.ChangeScene(new LobbyScene()); },
                true
                ));
            //Load game button
            var loadSpr1 = content.Load<Texture2D>("Images/BMLoadGameButton");
            var loadSpr2 = content.Load<Texture2D>("Images/BMLoadGameButton_Hover");
            AddObject(new Button(
                loadSpr1,
                loadSpr2,
                new Vector2(GameWorld.Camera.Position.X - loadSpr1.Width / 2, GameWorld.Camera.Position.Y + 0),
                () => { },
                true
                ));
            //Settings button
            var settingsSpr1 = content.Load<Texture2D>("Images/BMSettingsButton");
            var settingsSpr2 = content.Load<Texture2D>("Images/BMSettingsButton_Hover");
            AddObject(new Button(
                settingsSpr1,
                settingsSpr2,
                new Vector2(GameWorld.Camera.Position.X - settingsSpr1.Width / 2, GameWorld.Camera.Position.Y + settingsSpr1.Height * 1f),
                () => { GameWorld.ChangeScene(new SettingsScene()); },
                true
                ));
            //Quit button
            var quitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
            var quitSpr2 = content.Load<Texture2D>("Images/BMQuitButton_Hover");
            AddObject(new Button(
                quitSpr1,
                quitSpr2,
                new Vector2(GameWorld.Camera.Position.X - quitSpr1.Width / 2, GameWorld.Camera.Position.Y + quitSpr1.Height * 2f),
                () => { GameWorld.Instance.Exit(); },
                true
                ));
            background = content.Load<Texture2D>("Images/BMmenu");
        }

        public override void Update()
        {
            foreach (GameObject button in ActiveObjects)
            {
                button.Update();
            }
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(background, new Vector2(-GameWorld.GameWidth / 2, -GameWorld.GameHeight / 2));
            foreach (GameObject button in ActiveObjects)
            {
                button.Draw(drawer);
            }
        }
    }
}