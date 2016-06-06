using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class MenuScene : Scene
    {
        private Vector2 backgroundPos;
        private Texture2D background;

        public MenuScene()
        {
            //Title
            GameObject titleObj = new GameObject(GameWorld.Camera.Position + new Vector2(0, -64));
            titleObj.AddComponent(new SpriteRenderer("Textures/UI/Menu/Title"));
            AddObject(titleObj);
            //Play button

            var playSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/NewGame");
            var playSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/NewGame_Hover");

            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - playSpr1.Width / 2, GameWorld.Camera.Position.Y + playSpr1.Height * -1f),
                playSpr1,
                playSpr2,
                () => { GameWorld.ChangeScene(new IntroductionScene()); GameWorld.State.NewGame(); },
                null,
                true
                ));
            //Load game button
            var loadSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/LoadGame");
            var loadSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/LoadGame_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - loadSpr1.Width / 2, GameWorld.Camera.Position.Y + 0),
                loadSpr1,
                loadSpr2,
                () => { GameWorld.State.Load(); },
                null,
                true
                ));
            //Settings button
            var settingsSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Settings");
            var settingsSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Settings_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - settingsSpr1.Width / 2, GameWorld.Camera.Position.Y + settingsSpr1.Height * 1f),
                settingsSpr1,
                settingsSpr2,
                () => { GameWorld.ChangeScene(new SettingsScene()); },
                null,
                true
                ));
            //Quit button
            var quitSpr1 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Quit");
            var quitSpr2 = GameWorld.Load<Texture2D>("Textures/UI/Menu/Quit_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - quitSpr1.Width / 2, GameWorld.Camera.Position.Y + quitSpr1.Height * 2f),
                quitSpr1,
                quitSpr2,
                () => { GameWorld.Instance.Exit(); },
                null,
                true
                ));
            background = GameWorld.Load<Texture2D>("Textures/Backgrounds/Menu");
            backgroundPos = new Vector2(
                GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2);

            GameWorld.SoundManager.StopSound("AmbienceSound");
            GameWorld.SoundManager.PlayMusic("HubMusic");
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(background, backgroundPos);
            base.Draw(drawer);
        }
    }
}