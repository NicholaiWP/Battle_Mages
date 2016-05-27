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
        Left, Right, Up, Down
    }

    public class Character : Component
    {
        private FacingDirection fDirection;
        private Collider collider;

        /// <summary>
        /// The direction this character should move in.
        /// </summary>
        public Vector2 MoveDirection { get; set; }

        /// <summary>
        /// Max move speed of this character in units/second
        /// </summary>
        public float MoveSpeed { get; set; } = 100;

        /// <summary>
        /// Number of units/second this character accelerates with to reach its max speed
        /// </summary>
        public float MoveAccel { get; set; } = 1000;

        /// <summary>
        /// Current velocity of this character in units/second
        /// </summary>
        public Vector2 Velocity { get; private set; }

        public FacingDirection FDirection { set { fDirection = value; } }

        public Character()
        {
            Listen<InitializeMsg>(Initialize);
            fDirection = FacingDirection.Down;
        }

        private void Initialize(InitializeMsg msg)
        {
            collider = GameObject.GetComponent<Collider>();
        }

        public void Movement()
        {
            if (MoveDirection != Vector2.Zero)
                MoveDirection.Normalize(); //Make sure MoveDirection is normalized

            //Move velocity towards target velocity using MoveAccel
            Vector2 targetVelocity = MoveDirection * MoveSpeed;

            if (Velocity != targetVelocity)
            {
                Vector2 start = Velocity;
                float dist = Vector2.Distance(start, targetVelocity);

                Velocity += Vector2.Normalize(targetVelocity - Velocity) * MoveAccel * GameWorld.DeltaTime;
                if (Vector2.Distance(start, Velocity) > dist)
                {
                    Velocity = targetVelocity;
                }
            }

            //'translation' is the exact movement the character will perform
            Vector2 translation = Velocity * GameWorld.DeltaTime;

            //Collision checking
            float xDist = Math.Abs(translation.X);
            float yDist = Math.Abs(translation.Y);

            bool collisionLeft = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(-xDist, 0), true);
            bool collisionRight = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(xDist, 0), true);
            bool collisionUp = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(0, -yDist), true);
            bool collisionDown = collider.CheckCollisionAtPosition(GameObject.Transform.Position + new Vector2(0, yDist), true);

            //Limit the translation (along with the velocity) based on horizontal collisions
            if ((translation.X > 0 && collisionRight) || (translation.X < 0 && collisionLeft))
            {
                translation.X = 0;
                Velocity = new Vector2(0, Velocity.Y);
            }
            //Same for vertical collisions
            if ((translation.Y > 0 && collisionDown) || (translation.Y < 0 && collisionUp))
            {
                translation.Y = 0;
                Velocity = new Vector2(Velocity.X, 0);
            }

            if (translation == Vector2.Zero)
            {
                if (GameObject.GetComponent<Animator>() != null && GameObject.GetComponent<Player>() != null)
                {
                    GameObject.GetComponent<Animator>().PlayAnimation("Idle" + fDirection.ToString());
                }
            }
            else
            {
                if (GameObject.GetComponent<Player>() != null)
                    GameObject.GetComponent<Animator>().PlayAnimation("Walk" + fDirection.ToString());
            }

            GameObject.Transform.Translate(translation);

            //Limiting position to circle
            GameObject.Transform.Position = Utils.LimitToCircle(GameObject.Transform.Position, Vector2.Zero, 320);
        }
    }
}