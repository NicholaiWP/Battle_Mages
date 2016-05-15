using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class GameScene : Scene
    {
        private KeyboardState keyState;
        private PausedGameScreen pgs;

        private bool canChangePause;

        public GameScene()
        {
            pgs = new PausedGameScreen();
            Paused = false;
            canChangePause = true;
            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer(ellipse, "Images/BMarena"));
            objectsToAdd.Add(ellipse);
            objectsToAdd.Add(ObjectBuilder.BuildPlayer(Vector2.Zero));
            objectsToAdd.Add(ObjectBuilder.BuildEnemy(new Vector2(50, 50)));

            var ingameUI = new GameObject(new Vector2(100, 100));
            ingameUI.AddComponent(new IngameUI(ingameUI));
            objectsToAdd.Add(ingameUI);
            var wall = new GameObject(new Vector2(0, -100));
            wall.AddComponent(new Collider(wall, new Vector2(128, 32)));
            objectsToAdd.Add(wall);
            ProcessObjectLists();
            foreach (GameObject gameObject in ActiveObjects)
            {
                gameObject.Update();
            }

            foreach (GameObject gameObject in ActiveObjects)
            {
                if (gameObject.GetComponent<Player>() != null)
                {
                    GameWorld.Camera.Target = gameObject.Transform;
                    break;
                }
            }
        }

        public override void Update()
        {
            keyState = Keyboard.GetState();
            if (keyState.IsKeyUp(Keys.P))
            {
                canChangePause = true;
            }
            if (keyState.IsKeyDown(Keys.P) && !Paused && canChangePause)
            {
                Paused = true;
                canChangePause = false;
            }
            else if (keyState.IsKeyDown(Keys.P) && Paused && canChangePause)
            {
                Paused = false;
                canChangePause = false;
            }

            if (Paused)
            {
                pgs.Update();
            }
            else
            {
                GameWorld.Camera.Update(GameWorld.DeltaTime);
                foreach (GameObject gameObject in ActiveObjects)
                {
                    gameObject.Update();
                }
            }
        }

        public override void Draw(Drawer drawer)
        {
            if (Paused)
            {
                pgs.Draw(drawer);
            }
            else
            {
                foreach (GameObject gameObject in ActiveObjects)
                {
                    gameObject.Draw(drawer);
                }
            }
        }
    }
}