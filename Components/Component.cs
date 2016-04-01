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
        /// Constructor for the component with a gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}