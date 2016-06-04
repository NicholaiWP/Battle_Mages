using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class DeathScene : Scene
    {
        //private Texture2D deathTexture;
        //private Vector2 deathPos;
        private Animator playerAnimator;

        private float returnTimer = 6;

        public DeathScene(Vector2 playerPos)
        {
            /*var content = GameWorld.Instance.Content;
            Vector2 middle = GameWorld.Camera.Position - new Vector2((GameWorld.GameWidth / 2), (GameWorld.GameHeight / 2));
            deathPos = middle;
            deathTexture = content.Load<Texture2D>("Textures/Backgrounds/Death");*/

            GameWorld.SoundManager.PlaySound("DeathSound");

            GameObject playerObj = new GameObject(playerPos);
            playerObj.AddComponent(new SpriteRenderer("Textures/Player/PlayerSheet", true) { Rectangle = new Rectangle(0, 0, 32, 32) });
            playerAnimator = new Animator();
            playerObj.AddComponent(playerAnimator);
            playerAnimator.CreateAnimation("Death", new Animation(priority: 0, framesCount: 23, yPos: 384, xStartFrame: 0,
                 width: 32, height: 32, fps: 12, offset: Vector2.Zero));
            AddObject(playerObj);
        }

        public override void Update()
        {
            returnTimer -= GameWorld.DeltaTime;
            //if "R" is pressed, return to the menu
            if (GameWorld.KeyPressed(Keys.R) || returnTimer <= 0)
            {
                GameWorld.ChangeScene(new LobbyScene());
            }

            base.Update();

            playerAnimator.PlayAnimation("Death", looping: false);
        }

        public override void Draw(Drawer drawer)
        {
            //drawer[DrawLayer.Background].Draw(deathTexture, deathPos, Color.White);

            base.Draw(drawer);
        }
    }
}