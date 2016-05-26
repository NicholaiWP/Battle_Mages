using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class GameScene : Scene
    {
        private KeyboardState keyState;
        private GameObject waveController;

        public GameScene()
        {
            //Creating the brackground for the arena and adding it to the list
            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer(ellipse, "Images/BMarena"));
            AddObject(ellipse);

            //Making a player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(Vector2.Zero, true);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;

            //Changes the sound volume
            GameWorld.SoundManager.AmbienceVolume = 0.10f;

            //Music and soundhandling through SoundManager
            GameWorld.SoundManager.PlayMusic("CombatMusic");

            var ingameUI = new GameObject(new Vector2(100, 100));
            ingameUI.AddComponent(new IngameUI(ingameUI));
            AddObject(ingameUI);

            waveController = new GameObject(Vector2.Zero);
            waveController.AddComponent(new WaveController(waveController));
            waveController.SendMessage(new UpdateMsg());
            //Get all objects on the list before the first run of Update()
            base.Update();
        }

        public override void Update()
        {
            MediaPlayer.Volume = 0.01f;
            int enemyCount = 0;

            //Playing ambient sounds using SoundManager
            GameWorld.SoundManager.PlaySound("AmbienceSound");

            keyState = Keyboard.GetState();

            //If the key P is down then we change to the pause scene
            if (keyState.IsKeyDown(Keys.P))
            {
                GameWorld.ChangeScene(new PauseScene(this));
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
                waveController.GetComponent<WaveController>().NewWave(1);
            }

            base.Update();
        }
    }
}