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
    public class DeathScene : Scene
    {
        private Texture2D deathTexture;
        private Vector2 deathPos;
        private KeyboardState keyState;
        private MenuScene mScene;

        public DeathScene()
        {
            var content = GameWorld.Instance.Content;
            Vector2 middle = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            deathPos = middle;
            deathTexture = content.Load<Texture2D>("Textures/Backgrounds/Death");
        }

        public override void Update()
        {
            keyState = Keyboard.GetState();
            //if "R" is pressed, return to the menu
            if (keyState.IsKeyDown(Keys.R))
            {
                GameWorld.ChangeScene(new LobbyScene());
            }

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(deathTexture, deathPos, Color.White);

            base.Draw(drawer);
        }
    }
}