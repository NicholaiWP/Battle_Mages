using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Scene
    {
        private List<GameObject> activeObjects = new List<GameObject>();
        private List<GameObject> objectsToAdd = new List<GameObject>();
        private List<GameObject> objectsToRemove = new List<GameObject>();

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
            foreach (GameObject gameObject in objectsToAdd)
            {
                activeObjects.Add(gameObject);
            }
            objectsToAdd.Clear();

            foreach (GameObject gameObject in objectsToRemove)
            {
                activeObjects.Remove(gameObject);
            }
            objectsToRemove.Clear();
        }

        public void DrawObjects(Drawer target)
        {
            foreach (GameObject go in activeObjects)
                go.Draw(target);
        }
    }
}