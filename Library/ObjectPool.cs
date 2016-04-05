using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public static class ObjectPool
    {
        //Fields
        private static ICanBuild builder = new EnemyBuilder();

        private static List<GameObject> inactiveEnemies = new List<GameObject>();
        private static Director director = new Director(builder);
        private static Random random = new Random();
        private static GameObject gameObject = new GameObject(Vector2.Zero);
        public static List<GameObject> activeEmemies = new List<GameObject>();

        public static GameObject Create()
        {
            if (inactiveEnemies.Count > 0)
            {
                gameObject = inactiveEnemies[0];
                activeEmemies.Add(gameObject);
                inactiveEnemies.RemoveAt(0);
                GameWorld.GetInstance.collidersToAdd.Add((Collider)gameObject.GetComponent("Collider"));
                return gameObject;
            }
            else
            {
                gameObject = director.Construct((new Vector2(random.Next(0, 300), random.Next(0, 400))));
                activeEmemies.Add(gameObject);
                return gameObject;
            }
        }

        public static void ReleaseObject(GameObject gameObject)
        {
            CleanUp(gameObject);
            activeEmemies.Remove(gameObject);
            inactiveEnemies.Add(gameObject);
        }

        private static void CleanUp(GameObject gameObject)
        {
            GameWorld.GetInstance.objectsToRemove.Add(gameObject);
            GameWorld.GetInstance.collidersToRemove.Add((Collider)gameObject.GetComponent("Collider"));
            gameObject.Transform.Position = new Vector2(random.Next(0, 300), random.Next(0, 400));
        }
    }
}