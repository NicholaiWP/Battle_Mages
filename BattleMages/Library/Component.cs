using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Component
    {
        private GameObject gameObject;

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
        }

        /// <summary>
        /// Constructor for the component with a gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual void OnDestroy()
        {
        }
    }
}