using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public static class ObjectBuilder
    {
        public static GameObject BuildPlayer(Vector2 position, bool canUseSpells)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/GdMageBM"));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Player(gameObject, canUseSpells));
            gameObject.AddComponent(new Collider(gameObject, new Vector2(32, 32)));
            gameObject.AddComponent(new Character(gameObject));
            return gameObject;
        }

        public static GameObject BuildEnemy(Vector2 position, EnemyType type)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM"));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Character(gameObject) { MoveSpeed = 40 });
            gameObject.AddComponent(new Enemy(gameObject, 100, type));
            gameObject.AddComponent(new Collider(gameObject, new Vector2(32, 32)));
            return gameObject;
        }

        public static GameObject BuildFlyingLabelText(Vector2 position, string text)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new FloatingText(gameObject, text));
            return gameObject;
        }

        public static GameObject BuildInvisibleWall(Vector2 position, Vector2 size)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new Collider(gameObject, size, true));
            return gameObject;
        }

        public static GameObject BuildSpell(Vector2 position, SpellInfo baseSpell, SpellCreationParams creationParams, out float cooldownTime)
        {
            GameObject gameObject = new GameObject(position);
            Spell s = baseSpell.CreateSpell(gameObject, creationParams);
            gameObject.AddComponent(s);
            cooldownTime = s.CooldownTime;
            return gameObject;
        }
    }
}