using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public class Character : Component
    {
        private FacingDirection fDirection;
        private IStrategy walkStrategy;
        private IStrategy idleStrategy;

        //bool properties
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
            if (Right)
                fDirection = FacingDirection.Right;
            else if (Left)
                fDirection = FacingDirection.Left;
            else if (Up)
                fDirection = FacingDirection.Back;
            else if (Down)
                fDirection = FacingDirection.Front;
            else
                idleStrategy.Execute(Left, Right, Up, Down, fDirection);

            walkStrategy.Execute(Left, Right, Up, Down, fDirection);

            //Testing limiting position to circle
            GameObject.Transform.Position = Utils.LimitToCircle(GameObject.Transform.Position, Vector2.Zero, 320);
        }
    }
}
