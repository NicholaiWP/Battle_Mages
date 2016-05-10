using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Character : Component
    {
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
                fDirection = FacingDirection.Right;
            }
            else if (Up && Left)
            {
                fDirection = FacingDirection.Left;
            }
            else if (Down && Right)
            {
                fDirection = FacingDirection.Right;
            }
            else if (Down && Left)
            {
                fDirection = FacingDirection.Left;
            }
            else if (Up)
            {
                fDirection = FacingDirection.Back;
            }
            else if (Left)
            {
                fDirection = FacingDirection.Left;
            }
            else if (Down)
            {
                fDirection = FacingDirection.Front;
            }
            else if (Right)
            {
                fDirection = FacingDirection.Right;
            }
            else
            {
                idleStrategy.Execute(Left, Right, Up, Down, fDirection);
            }
            walkStrategy.Execute(Left, Right, Up, Down, fDirection);

            //Testing limiting position to circle
            GameObject.Transform.Position = Utils.LimitToCircle(GameObject.Transform.Position, Vector2.Zero, 320);
        }
    }
}