using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private Vector2 backgroundPos;
        private Texture2D background;

        public MenuScene()
        {
            var content = GameWorld.Instance.Content;
            //Play button
            var playSpr1 = content.Load<Texture2D>("Images/BMPlayGameButton");
            var playSpr2 = content.Load<Texture2D>("Images/BMPlayGameButton_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - playSpr1.Width / 2, GameWorld.Camera.Position.Y + playSpr1.Height * -1f),
                playSpr1,
                playSpr2,
                () => { GameWorld.ChangeScene(new LobbyScene()); GameWorld.State.NewGame(); },
                null,
                true
                ));
            //Load game button
            var loadSpr1 = content.Load<Texture2D>("Images/BMLoadGameButton");
            var loadSpr2 = content.Load<Texture2D>("Images/BMLoadGameButton_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - loadSpr1.Width / 2, GameWorld.Camera.Position.Y + 0),
                loadSpr1,
                loadSpr2,
                () => { GameWorld.State.Load(); },
                null,
                true
                ));
            //Settings button
            var settingsSpr1 = content.Load<Texture2D>("Images/BMSettingsButton");
            var settingsSpr2 = content.Load<Texture2D>("Images/BMSettingsButton_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - settingsSpr1.Width / 2, GameWorld.Camera.Position.Y + settingsSpr1.Height * 1f),
                settingsSpr1,
                settingsSpr2,
                () => { GameWorld.ChangeScene(new SettingsScene()); },
                null,
                true
                ));
            //Quit button
            var quitSpr1 = content.Load<Texture2D>("Images/BMQuitButton");
            var quitSpr2 = content.Load<Texture2D>("Images/BMQuitButton_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - quitSpr1.Width / 2, GameWorld.Camera.Position.Y + quitSpr1.Height * 2f),
                quitSpr1,
                quitSpr2,
                () => { GameWorld.Instance.Exit(); },
                null,
                true
                ));
            background = content.Load<Texture2D>("Images/BMmenu");
            backgroundPos = new Vector2(
                GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2);
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(background, backgroundPos);
            base.Draw(drawer);
        }
    }
}