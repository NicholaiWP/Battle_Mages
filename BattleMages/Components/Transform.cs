using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
    }
}