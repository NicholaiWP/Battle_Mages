using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Character : Component
    {
        private MovingDirection mDirection;
        private FacingDirection fDirection;
        private IStrategy walkStrategy;
        private IStrategy idleStrategy;
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public Character(GameObject gameObject) : base(gameObject)
        {
            walkStrategy = new Walk(GameObject.GetComponent<Animator>(),
                GameObject.Transform, 100);
            idleStrategy = new Idle(GameObject.GetComponent<Animator>());
        }

        public void Movement()
        {
            if (Up && Right)
            {
                mDirection = MovingDirection.UpRight;
                fDirection = FacingDirection.Right;
            }
            else if (Up && Left)
            {
                mDirection = MovingDirection.UpLeft;
                fDirection = FacingDirection.Left;
            }
            else if (Down && Right)
            {
                mDirection = MovingDirection.DownRight;
                fDirection = FacingDirection.Right;
            }
            else if (Down && Left)
            {
                mDirection = MovingDirection.DownLeft;
                fDirection = FacingDirection.Left;
            }
            else if (Up)
            {
                mDirection = MovingDirection.Up;
                fDirection = FacingDirection.Back;
            }
            else if (Left)
            {
                mDirection = MovingDirection.Left;
                fDirection = FacingDirection.Left;
            }
            else if (Down)
            {
                mDirection = MovingDirection.Down;
                fDirection = FacingDirection.Front;
            }
            else if (Right)
            {
                mDirection = MovingDirection.Right;
                fDirection = FacingDirection.Right;
            }
            else
            {
                mDirection = MovingDirection.Idle;
                idleStrategy.Execute(mDirection, fDirection);
            }
            walkStrategy.Execute(mDirection, fDirection);
        }
    }
}