using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Idle : IStrategy
    {
        private Animator animator;

        public Idle(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(MovingDirection moveDirection, FacingDirection direction)
        {
            switch (direction)
            {
                case FacingDirection.Front:
                    animator.PlayAnimation("IdleFront");
                    break;

                case FacingDirection.Back:
                    animator.PlayAnimation("IdleBack");
                    break;

                case FacingDirection.Left:
                    animator.PlayAnimation("IdleLeft");
                    break;

                case FacingDirection.Right:
                    animator.PlayAnimation("IdleRight");
                    break;
            }
        }
    }
}
