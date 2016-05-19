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
        private Vector2 lobbyPosition;
        private KeyboardState keyState;

        public LobbyScene()
        {
            var content = GameWorld.Instance.Content;
            lobbyPosition = new Vector2(-100, -100);
            lobbyTexture = content.Load<Texture2D>("Images/BMtavern");

            //Making the player
            AddObject(ObjectBuilder.BuildPlayer(new Vector2(lobbyTexture.Width / 2 - 100, lobbyTexture.Height - 120)));
            ProcessObjectLists();

            //Sets the sound volume low
            GameWorld.SoundManager.SoundVolume = 0.01f;

            //Updating the gameobjects once to add the components
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
            drawer[DrawLayer.Background].Draw(lobbyTexture, lobbyPosition, Color.White);

            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}