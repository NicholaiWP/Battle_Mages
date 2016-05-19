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
    public class LobbyScene : Scene
    {
        private Texture2D lobbyTexture;
        private Texture2D lobbyTextureForeground;
        private Vector2 lobbyTexturePosition;
        private KeyboardState keyState;

        public LobbyScene()
        {
            var content = GameWorld.Instance.Content;
            lobbyTexturePosition = new Vector2(-160, -270);
            lobbyTexture = content.Load<Texture2D>("Backgrounds/Tavern");
            lobbyTextureForeground = content.Load<Texture2D>("Backgrounds/TavernLighting");

            //Side walls
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, 90 + 8), new Vector2(320, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, -90 - 8), new Vector2(320, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(-160 - 8, 0), new Vector2(16, 180 + 32)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(160 + 8, 0), new Vector2(16, 180 + 32)));

            //Updating the gameobjects once to add the components
            foreach (GameObject go in ActiveObjects)
            {
                go.Update();
            }
            //Door trigger
            GameObject doorTriggerGameObject = new GameObject(new Vector2(0, -90 - 98 / 2));
            doorTriggerGameObject.AddComponent(new Collider(doorTriggerGameObject, new Vector2(38, 98)));
            doorTriggerGameObject.AddComponent(new Interactable(doorTriggerGameObject, () => { GameWorld.ChangeScene(new HallwayScene()); }));
            AddObject(doorTriggerGameObject);

            //Sets the sound volume for this scene
            GameWorld.SoundManager.AmbienceVolume = 0.02f;

            //Player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(Vector2.Zero, false);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;

            //Get all objects on the list before the first run of Update()
            ProcessObjectLists();
        }

        public override void Update()
        {
            //Playing ambient sound in low volume using SoundManager
            GameWorld.SoundManager.PlaySound("ambience");

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
            drawer[DrawLayer.Background].Draw(lobbyTexture, lobbyTexturePosition, Color.White);
            drawer[DrawLayer.Foreground].Draw(lobbyTextureForeground, lobbyTexturePosition, Color.White);

            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}