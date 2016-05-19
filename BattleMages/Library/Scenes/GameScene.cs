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
        private Player player;
        private GameObject gameObject;


        public GameScene()
        {
            //Creating the brackground for the arena and adding it to the list
            var ellipse = new GameObject(Vector2.Zero);
            ellipse.AddComponent(new SpriteRenderer(ellipse, "Images/BMarena"));
            AddObject(ellipse);

            //Making a player with the ObjectBuilder
            AddObject(ObjectBuilder.BuildPlayer(Vector2.Zero));
            AddObject(ObjectBuilder.BuildEnemy(new Vector2(50, 50)));

            var ingameUI = new GameObject(new Vector2(100, 100));
            ingameUI.AddComponent(new IngameUI(ingameUI));
            AddObject(ingameUI);

            var wall = new GameObject(new Vector2(0, -100));
            wall.AddComponent(new Collider(wall, new Vector2(128, 32), true));
            AddObject(wall);

            //Processing the lists
            ProcessObjectLists();

            //Making one update for all GameObjects in the ActiveObjects list,
            //so the components are added to the GameObject´s components list.
            foreach (GameObject go in ActiveObjects)
            {
                go.Update();
            }

            //Finding the GameObject with the player component, so the camera can target it.
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

            //If the key P is down then we change to the pause scene
            if (keyState.IsKeyDown(Keys.P))
            {
                GameWorld.ChangeScene(new PauseScene(this));
            }
            GameWorld.Camera.Update(GameWorld.DeltaTime);

            foreach (GameObject go in ActiveObjects)
            {
                go.Update();
            }

            player = new Player(gameObject);


            
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