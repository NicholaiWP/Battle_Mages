using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class PlayerBuilder : ICanBuild
    {
        private GameObject gameObject;

        public PlayerBuilder()
        {
        }

        public void BuildGameObject(Vector2 position)
        {
            gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/apple", 0));
            gameObject.AddComponent(new Character(gameObject));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Player(gameObject));
            //go.AddComponent(new Collider(go));
        }

        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}