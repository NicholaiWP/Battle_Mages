using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    public enum FacingDirection
    {
        Left, Right, Back, Front
    }

    public class Character : Component
    {
        private FacingDirection fDirection;

        //bool properties
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public float MoveSpeed { get; set; } = 100;

        public Character(GameObject gameObject) : base(gameObject)
        {
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

            float moveDist = GameWorld.DeltaTime * MoveSpeed;

            Collider collider = GameObject.GetComponent<Collider>();
            bool collisionLeft = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(-moveDist, 0), true);
            bool collisionRight = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(moveDist, 0), true);
            bool collisionUp = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(0, -moveDist), true);
            bool collisionDown = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(0, moveDist), true);

            Vector2 translation = Vector2.Zero;
            if (Left && !collisionLeft)
                translation += new Vector2(-1, 0);
            if (Right && !collisionRight)
                translation += new Vector2(1, 0);
            if (Up && !collisionUp)
                translation += new Vector2(0, -1);
            if (Down && !collisionDown)
                translation += new Vector2(0, 1);

            GameObject.Transform.Translate(translation * moveDist);

            //Limiting position to circle
            GameObject.Transform.Position = Utils.LimitToCircle(GameObject.Transform.Position, Vector2.Zero, 320);
        }
    }
}