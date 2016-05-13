using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Idle : IStrategy
    {
        private Animator animator;

        public Idle(Animator animator)
        {
            this.animator = animator;
        }

        public void Execute(bool movingLeft, bool movingRight, bool movingUp, bool movingDown, FacingDirection fDirection)
        {
            switch (fDirection)
            {
                case FacingDirection.Left:
                    // animator.PlayAnimation("IdleLeft");
                    break;

                case FacingDirection.Right:
                    //animator.PlayAnimation("IdleRight");
                    break;

                case FacingDirection.Back:
                    // animator.PlayAnimation("IdleBack");
                    break;

                case FacingDirection.Front:
                    // animator.PlayAnimation("IdleFront");
                    break;
            }
        }
    }
}