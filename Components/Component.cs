using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Component
    {
        private GameObject gameObject;

        public GameObject GetGameObject
        {
            get
            {
                return gameObject;
            }
        }

        public Component()
        {
        }

        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}