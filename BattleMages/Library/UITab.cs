using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    /// <summary>
    /// Used to position UI elements in a group and to remove all elements in the group at once.
    /// </summary>
    public class UITab
    {
        private Scene scene;
        private List<GameObject> objects = new List<GameObject>();

        private Vector2 position;
        private Vector2 size;

        public Vector2 Size { get { return size; } }
        public Vector2 TopLeft { get { return position; } }
        public Vector2 TopRight { get { return position + new Vector2(size.X, 0); } }
        public Vector2 BotLeft { get { return position + new Vector2(0, size.Y); } }
        public Vector2 BotRight { get { return position + size; } }

        public UITab(Scene scene, Vector2 position, Vector2 size)
        {
            this.scene = scene;
            this.position = position;
            this.size = size;
        }

        public void AddObject(GameObject obj)
        {
            objects.Add(obj);
            scene.AddObject(obj);
        }

        public void Clear()
        {
            foreach (GameObject obj in objects)
            {
                scene.RemoveObject(obj);
            }
            objects.Clear();
        }
    }
}