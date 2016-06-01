using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Transform : Component
    {
        //Fields
        private Vector2 position;

        //Properties
        public Vector2 Position { get { return position; } set { position = value; } }

        /// <summary>
        /// The constructor for the transformer
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="position"></param>
        public Transform(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Method for moving the gameobject, by changing its position
        /// </summary>
        /// <param name="translation"></param>
        public void Translate(Vector2 translation)
        {
            position += translation;
        }

        /// <summary>
        /// Method for moving the gameobject and checking collision with offset, used
        /// for fast movement such as dash.
        /// </summary>
        /// <param name="translation"></param>
        /// <param name="minusOffset"></param>
        /// <param name="plusOffset"></param>
        /// <param name="collider"></param>
        public void Translate(Vector2 translation, Collider collider, float minusOffset, float plusOffset)
        {
            bool collisionLeft = collider.CheckCollisionAtPosition(position + new Vector2(-translation.X - minusOffset, 0), true);
            bool collisionRight = collider.CheckCollisionAtPosition(position + new Vector2(translation.X + plusOffset, 0), true);
            bool collisionUp = collider.CheckCollisionAtPosition(position + new Vector2(0, -translation.Y - minusOffset), true);
            bool collisionDown = collider.CheckCollisionAtPosition(position + new Vector2(0, translation.Y + plusOffset), true);

            if ((translation.X > 0 && collisionRight) || (translation.X < 0 && collisionLeft))
            {
                translation.X = 0;
            }
            if ((translation.Y > 0 && collisionDown) || (translation.Y < 0 && collisionUp))
            {
                translation.Y = 0;
            }

            position += translation;

            position = Utils.LimitToCircle(position, Vector2.Zero, 320);
        }
    }
}