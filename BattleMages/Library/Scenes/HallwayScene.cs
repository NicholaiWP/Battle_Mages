using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    internal class HallwayScene : Scene
    {
        private Texture2D lobbyTexture;
        private Vector2 lobbyTexturePosition;
        private KeyboardState keyState;

        public HallwayScene()
        {
            var content = GameWorld.Instance.Content;
            lobbyTexturePosition = new Vector2(-32, -360 / 2);
            lobbyTexture = content.Load<Texture2D>("Backgrounds/Hallway");

            //Side walls
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, 180 + 8), new Vector2(64, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, -180 + 64 - 8), new Vector2(64, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(-32 - 8, 0), new Vector2(16, 360 + 32)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(32 + 8, 0), new Vector2(16, 360 + 32)));

            //Door trigger
            GameObject doorTriggerGameObject = new GameObject(new Vector2(0, -180 + 64 - 64 / 2));
            doorTriggerGameObject.AddComponent(new Collider(doorTriggerGameObject, new Vector2(64, 64)));
            doorTriggerGameObject.AddComponent(new Interactable(doorTriggerGameObject, () => { GameWorld.ChangeScene(new GameScene()); }));
            AddObject(doorTriggerGameObject);

            //Sets sound volume semi-low
            GameWorld.SoundManager.AmbienceVolume = 0.02f;

            //Player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(new Vector2(0, 180 - 32), false);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;

            //Get all objects on the list before the first run of Update()
            ProcessObjectLists();
        }
        
        public override void Update()
        {
            //Plays ambience sound looped using SoundManager
            GameWorld.SoundManager.PlaySound("AmbienceSound");
            
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

            //Turns volume of ambience up or down depending on the position of Camera
            if (GameWorld.Camera.Position.Y < 0)
            {
                GameWorld.SoundManager.AmbienceVolume += 0.03f * GameWorld.DeltaTime;
            }
            if (GameWorld.Camera.Position.Y > 0)
            {
                GameWorld.SoundManager.AmbienceVolume -= 0.03f * GameWorld.DeltaTime;
            }
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(lobbyTexture, lobbyTexturePosition, Color.White);

            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}