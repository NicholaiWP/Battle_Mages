using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BattleMages
{
    public class GameScene : Scene
    {
        private KeyboardState keyState;

        private SoundEffectInstance crowdSnd;

        public GameScene(string challengeName)
        {
            //Creating the brackground for the arena and adding it to the list
            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer("Textures/Backgrounds/Arena", layerToUse: DrawLayer.Background));
            AddObject(ellipse);

            //Making a player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(new Vector2(0, -300), true);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;

            //Music and soundhandling through SoundManager
            GameWorld.SoundManager.PlayMusic("CombatMusic");

            var ingameUI = new GameObject(new Vector2(100, 100));
            ingameUI.AddComponent(new IngameUI());
            AddObject(ingameUI);

            if (StaticData.challenges.ContainsKey(challengeName))
            {
                WaveController waveController = StaticData.challenges[challengeName].MakeWaveController();
                GameObject waveControllerObj = new GameObject(Vector2.Zero);
                waveControllerObj.AddComponent(waveController);
                waveControllerObj.SendMessage(new UpdateMsg());
            }
        }

        public override void Open()
        {
            crowdSnd = GameWorld.SoundManager.PlaySound("AmbienceSound", true);
            crowdSnd.Volume = 0;

            base.Open();
        }

        public override void Close()
        {
            crowdSnd.Stop();

            base.Close();
        }

        public override void Update()
        {
            //Playing ambient sounds using SoundManager
            //GameWorld.SoundManager.PlaySound("AmbienceSound");

            if (crowdSnd.Volume < 1)
                crowdSnd.Volume = MathHelper.Min(crowdSnd.Volume + GameWorld.DeltaTime, 0.5f);

            keyState = Keyboard.GetState();

            //If the key P is down then we change to the pause scene
            if (GameWorld.KeyPressed(Keys.Escape))
            {
                GameWorld.ChangeScene(new PauseScene(this));
            }

            GameWorld.Camera.Update(GameWorld.DeltaTime);

            base.Update();
        }
    }
}