using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public abstract class Scene
    {
        private List<GameObject> activeObjects = new List<GameObject>();
        private List<GameObject> objectsToAdd = new List<GameObject>();
        private List<GameObject> objectsToRemove = new List<GameObject>();
        protected SpriteFont fontBM;
        protected Texture2D background;

        /// <summary>
        /// Returns a readonly list of objects currently active in the scene.
        /// </summary>
        public ReadOnlyCollection<GameObject> ActiveObjects
        {
            get { return activeObjects.AsReadOnly(); }
        }

        public Scene()
        {
        }

        public abstract void Update();

        public abstract void Draw(Drawer drawer);

        /// <summary>
        /// Marks an object for addition to ActiveObjects.
        /// </summary>
        /// <param name="go"></param>
        public void AddObject(GameObject go)
        {
            objectsToAdd.Add(go);
        }

        /// <summary>
        /// Marks an object for removal from ActiveObjects.
        /// </summary>
        /// <param name="go"></param>
        public void RemoveObject(GameObject go)
        {
            objectsToRemove.Add(go);
        }

        /// <summary>
        /// Processes objects marked with AddObject or RemoveObject.
        /// The objects will be added/removed from ActiveObjects.
        /// </summary>
        public void ProcessObjectLists()
        {
            //Additions
            foreach (GameObject gameObject in objectsToAdd)
                activeObjects.Add(gameObject);

            objectsToAdd.Clear();

            //Removal
            foreach (GameObject gameObject in objectsToRemove)
                activeObjects.Remove(gameObject);

            foreach (GameObject gameObject in objectsToRemove)
                gameObject.OnDestroy();

            objectsToRemove.Clear();
        }
    }
}