using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public Vector2 MoveDirection;

        public float MoveSpeed { get; set; } = 100;

        public Vector2 Velocity { get; private set; }

        public Character(GameObject gameObject) : base(gameObject)
        {
        }

        public void Movement()
        {
            if (MoveDirection != Vector2.Zero)
                MoveDirection.Normalize(); //Make sure MoveDirection is normalized

            //TODO: Use this for setting the FacingDirection
            //float angle = (float)Math.Atan2(MoveDirection.X, MoveDirection.Y);
            //System.Diagnostics.Debug.WriteLine(angle);

            /*if (Right)
                fDirection = FacingDirection.Right;
            else if (Left)
                fDirection = FacingDirection.Left;
            else if (Up)
                fDirection = FacingDirection.Back;
            else if (Down)
                fDirection = FacingDirection.Front;*/

            Collider collider = GameObject.GetComponent<Collider>();

            //'translation' is the exact movement the character will perform
            Vector2 translation = MoveDirection * MoveSpeed * GameWorld.DeltaTime;

            //Collision checking
            float xDist = Math.Abs(translation.X);
            float yDist = Math.Abs(translation.Y);

            bool collisionLeft = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(-xDist, 0), true);
            bool collisionRight = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(xDist, 0), true);
            bool collisionUp = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(0, -yDist), true);
            bool collisionDown = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(0, yDist), true);

            //Limit the translation based on horizontal collisions
            if ((translation.X > 0 && collisionRight) || (translation.X < 0 && collisionLeft))
                translation.X = 0;
            //Same for vertical collisions
            if ((translation.Y > 0 && collisionDown) || (translation.Y < 0 && collisionUp))
                translation.Y = 0;

            GameObject.Transform.Translate(translation);
            Velocity = translation;

            //Limiting position to circle
            GameObject.Transform.Position = Utils.LimitToCircle(GameObject.Transform.Position, Vector2.Zero, 320);
        }
    }
}