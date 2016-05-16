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

        private bool canChangePause;

        public GameScene()
        {
            canChangePause = true;
            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer(ellipse, "Images/BMarena"));
            AddObject(ellipse);
            AddObject(ObjectBuilder.BuildPlayer(Vector2.Zero));
            AddObject(ObjectBuilder.BuildEnemy(new Vector2(50, 50)));

            var ingameUI = new GameObject(new Vector2(100, 100));
            ingameUI.AddComponent(new IngameUI(ingameUI));
            AddObject(ingameUI);
            var wall = new GameObject(new Vector2(0, -100));
            wall.AddComponent(new Collider(wall, new Vector2(128, 32)));
            AddObject(wall);
            ProcessObjectLists();

            foreach (GameObject gameObject in ActiveObjects)
            {
                gameObject.Update();
            }

            foreach (GameObject go in ActiveObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    GameWorld.Camera.Target = go.Transform;
                    break;
                }
            }
        }

        public override void Update()
        {
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.P))
            {
                GameWorld.ChangeScene(new PauseScene(this));
            }
            GameWorld.Camera.Update(GameWorld.DeltaTime);
            foreach (GameObject gameObject in ActiveObjects)
            {
                gameObject.Update();
            }
        }

        public override void Draw(Drawer drawer)
        {
            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}