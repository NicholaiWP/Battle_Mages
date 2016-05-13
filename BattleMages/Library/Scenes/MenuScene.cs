using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            objectsToAdd.Add(new Button(
                playSpr1,
                playSpr2,
                new Vector2(-playSpr1.Width / 2, playSpr1.Height * -1f),
                () => { GameWorld.ChangeScene(new GameScene()); },
                true
                ));
            //Load game button
            var loadSpr1 = content.Load<Texture2D>("Images/BMLoadGameButton");
            var loadSpr2 = content.Load<Texture2D>("Images/BMLoadGameButton_Hover");
            objectsToAdd.Add(new Button(
                loadSpr1,
                loadSpr2,
                new Vector2(-loadSpr1.Width / 2, 0),
                () => { },
                true
                ));
            //Settings button
            var settingsSpr1 = content.Load<Texture2D>("Images/BMSettingsButton");
            var settingsSpr2 = content.Load<Texture2D>("Images/BMSettingsButton_Hover");
            objectsToAdd.Add(new Button(
                settingsSpr1,
                settingsSpr2,
                new Vector2(-settingsSpr1.Width / 2, settingsSpr1.Height * 1f),
                () => { GameWorld.ChangeScene(new SettingsScene()); },
                true
                ));
            //Quit button
            var quitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
            var quitSpr2 = content.Load<Texture2D>("Images/BMQuitButton_Hover");
            objectsToAdd.Add(new Button(
                quitSpr1,
                quitSpr2,
                new Vector2(-quitSpr1.Width / 2, quitSpr1.Height * 2f),
                () => { GameWorld.Instance.Exit(); },
                true
                ));
        }

        public override void LoadContent(ContentManager content)
        {
            GameObject go = new GameObject(Vector2.Zero);
            GameWorld.Camera.Target = go.Transform;

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
            drawer[DrawLayer.Background].Draw(background, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2));
            foreach (GameObject button in ActiveObjects)
            {
                button.Draw(drawer);
            }
        }
    }
}