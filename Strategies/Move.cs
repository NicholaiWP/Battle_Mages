using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Move : IStrategy
    {
        private Animator animator;
        private Transform transform;
        private float speed;

        public Move(Animator animator, Transform transform, float speed)
        {
            this.animator = animator;
            this.transform = transform;
            this.speed = speed;
        }

        public void Execute(MovingDirection moveDirection, FacingDirection direction)
        {
            Vector2 translation = Vector2.Zero;
            switch (moveDirection)
            {
                case MovingDirection.UpLeft:
                    //animator.PlayAnimation("WalkLeft");
                    translation -= new Vector2(1, 1);
                    break;

                case MovingDirection.UpRight:
                    //animator.PlayAnimation("WalkRight");
                    translation += new Vector2(1, -1);
                    break;

                case MovingDirection.Up:
                    //animator.PlayAnimation("WalkBack");
                    translation -= new Vector2(0, 1);
                    break;

                case MovingDirection.Left:
                    //animator.PlayAnimation("WalkLeft");
                    translation -= new Vector2(1, 0);
                    break;

                case MovingDirection.Right:
                    //animator.PlayAnimation("WalkRight");
                    translation += new Vector2(1, 0);
                    break;

                case MovingDirection.DownLeft:
                    //animator.PlayAnimation("WalkLeft");
                    translation += new Vector2(-1, 1);
                    break;

                case MovingDirection.DownRight:
                    //animator.PlayAnimation("WalkRight");
                    translation += new Vector2(1, 1);
                    break;

                case MovingDirection.Down:
                    //animator.PlayAnimation("WalkFront");
                    translation += new Vector2(0, 1);
                    break;
            }
            transform.Translate(translation * GameWorld.GetInstance.GetDeltaTime * speed);
        }
    }
}