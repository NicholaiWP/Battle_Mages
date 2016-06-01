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
        private GameObject goWaveController;

        private SoundEffectInstance crowdSnd;

        public GameScene(string challengeName)
        {
            //Creating the brackground for the arena and adding it to the list
            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer("Textures/Backgrounds/Arena"));
            AddObject(ellipse);

            //Making a player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(new Vector2(0, -300), true);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;

            //Music and soundhandling through SoundManager
            GameWorld.SoundManager.PlayMusic("CombatMusic");
            crowdSnd = GameWorld.SoundManager.PlaySound("AmbienceSound", true);
            crowdSnd.Volume = 0;

            var ingameUI = new GameObject(new Vector2(100, 100));
            ingameUI.AddComponent(new IngameUI());
            AddObject(ingameUI);

            if (StaticData.challenges.ContainsKey(challengeName))
            {
                WaveController waveController = StaticData.challenges[challengeName].MakeWaveController();
                goWaveController = new GameObject(Vector2.Zero);
                goWaveController.AddComponent(waveController);
                goWaveController.SendMessage(new UpdateMsg());
            }
        }

        public override void Update()
        {
            int enemyCount = 0;

            //Playing ambient sounds using SoundManager
            //GameWorld.SoundManager.PlaySound("AmbienceSound");

            if (crowdSnd.Volume < 1)
                crowdSnd.Volume = MathHelper.Min(crowdSnd.Volume + GameWorld.DeltaTime, 0.5f);

            keyState = Keyboard.GetState();

            //If the key P is down then we change to the pause scene
            if (keyState.IsKeyDown(Keys.Escape))
            {
                crowdSnd.Pause();
                GameWorld.ChangeScene(new PauseScene(this, () => { crowdSnd.Play(); }));
            }

            GameWorld.Camera.Update(GameWorld.DeltaTime);

            foreach (GameObject go in ActiveObjects)
            {
                if (go.GetComponent<Enemy>() != null)
                {
                    enemyCount++;
                }
            }

            if (enemyCount == 0)
            {
                goWaveController.GetComponent<WaveController>().UpdateWave();
            }
            base.Update();
        }
    }
}