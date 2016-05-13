using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battle_Mages
{
    public class LobbyScene : Scene
    {
        private Texture2D lobbyTexture;
        private Vector2 lobbyPosition;
        private KeyboardState keyState;
        private PausedGameScreen pgs;
        private bool paused;
        private bool canChangePause;

        public LobbyScene()
        {
            lobbyPosition = new Vector2(-100, -100);
            pgs = new PausedGameScreen();

        }

        public override void LoadContent(ContentManager content)
        {
            lobbyTexture = content.Load<Texture2D>("Images/BMtavern");
            objectsToAdd.Add(ObjectBuilder.BuildPlayer(new Vector2(lobbyTexture.Width / 2 - 100, lobbyTexture.Height - 120)));
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
            keyState = new KeyboardState();
            if (keyState.IsKeyUp(Keys.P))
            {
                canChangePause = true;
            }
            if (keyState.IsKeyDown(Keys.P) && !paused && canChangePause)
            {
                paused = true;
                canChangePause = false;
            }
            else if (keyState.IsKeyDown(Keys.P) && paused && canChangePause)
            {
                paused = false;
                canChangePause = false;
            }

            if (paused)
            {
                pgs.Update();
            }
            else
            {
                GameWorld.Camera.Update(GameWorld.Instance.DeltaTime);
                foreach (GameObject gameObject in ActiveObjects)
                {
                    gameObject.Update();
                }
            }
        }

        public override void Draw(Drawer drawer)
        {
            SpriteBatch spriteBatch = drawer[DrawLayer.Background];
            spriteBatch.Draw(lobbyTexture, lobbyPosition, Color.White);
            foreach (GameObject go in ActiveObjects)
            {
                go.Draw(drawer);
            }
        }
    }
}
