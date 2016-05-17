﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public static class ObjectBuilder
    {
        public static GameObject BuildPlayer(Vector2 position)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/GdMageBM"));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Player(gameObject));
            gameObject.AddComponent(new Collider(gameObject, new Vector2(32, 32)));
            gameObject.AddComponent(new Character(gameObject));
            return gameObject;
        }

        public static GameObject BuildEnemy(Vector2 position)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM"));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Character(gameObject) { MoveSpeed = 40 });
            gameObject.AddComponent(new Enemy(gameObject));
            gameObject.AddComponent(new Collider(gameObject, new Vector2(32, 32)));
            return gameObject;
        }
    }
}