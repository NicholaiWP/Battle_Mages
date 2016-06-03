using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class ShowSaving : Component
    {
        private Animator animator;

        public ShowSaving()
        {
            Listen<PreInitializeMsg>(Preinitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        private void Preinitialize(PreInitializeMsg message)
        {
            GameObject.AddComponent(new SpriteRenderer("Textures/Player/PlayerSheet", true)
            { Rectangle = new Rectangle(0, 0, 32, 32) });
        }

        private void Initialize(InitializeMsg message)
        {
            GameObject.Transform.Position = new Vector2(GameWorld.Camera.Position.X + GameWorld.GameWidth / 2 - 16,
                GameWorld.Camera.Position.Y + GameWorld.GameHeight / 2 - 16);
            animator = GameObject.GetComponent<Animator>();
            animator.CreateAnimation("Saving", new Animation(priority: 0, framesCount: 25, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 12, offset: Vector2.Zero));
        }

        private void Update(UpdateMsg message)
        {
            GameObject.Transform.Position = new Vector2(GameWorld.Camera.Position.X + GameWorld.GameWidth / 2 - 16,
                GameWorld.Camera.Position.Y + GameWorld.GameHeight / 2 - 16);
            animator.PlayAnimation("Saving");
        }

        private void AnimationDone(AnimationDoneMsg message)
        {
            GameWorld.Scene.RemoveObject(GameObject);
        }
    }
}