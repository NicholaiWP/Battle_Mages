using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Component
    {
        //Fields
        private GameObject gameObject;

        //Property
        public GameObject GetGameObject
        {
            get
            {
                return gameObject;
            }
        }

        /// <summary>
        /// One of the constructors for the component
        /// </summary>
        public Component()
        {
        }

        /// <summary>
        /// One of the constructors for the component with a gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}