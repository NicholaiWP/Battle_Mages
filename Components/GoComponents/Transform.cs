using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
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
        public Transform(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            this.position = position;
        }

        /// <summary>
        /// Method for moving the gameobject, by changing its position
        /// </summary>
        /// <param name="tranlation"></param>
        public void Translate(Vector2 tranlation)
        {
            position += tranlation;
        }
    }
}