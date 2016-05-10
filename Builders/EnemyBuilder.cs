using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class EnemyBuilder : ICanBuild
    {
        private GameObject gameObject;

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM", 0));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Character(gameObject));
            gameObject.AddComponent(new Enemy(gameObject));
            gameObject.AddComponent(new Collider(gameObject, new Vector2(32, 32)));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}