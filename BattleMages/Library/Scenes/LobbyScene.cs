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
            AddObject(ObjectBuilder.BuildPlayer(new Vector2(lobbyTexture.Width / 2 - 100, lobbyTexture.Height - 120)));
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