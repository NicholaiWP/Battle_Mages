using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public static GameObject BuildEnemy(Vector2 position, Enemy enemy)
        {
            GameObject gameObject = new GameObject(position);

            if (enemy is Slime)
            {
                gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM"));
            }
            else if (enemy is Golem)
            {
                gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM"));
            }
            else if (enemy is Orb)
            {
                gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM"));
            }

            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Character(gameObject) { MoveSpeed = 40 });

            //Using the activator to create a new instance of the same type as the enemy with the gameobject,
            //so the enemy which the gameObject adds will have a connection to the gameObject
            var alikeEnemy = Activator.CreateInstance(enemy.GetType(), gameObject);

            gameObject.AddComponent(alikeEnemy as Component);
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

        public static GameObject BuildSpell(Vector2 position, SpellInfo baseSpell, SpellCreationParams creationParams, out float cooldownTime, out float manaCost)
        {
            GameObject gameObject = new GameObject(position);
            Spell s = baseSpell.CreateSpell(gameObject, creationParams);
            gameObject.AddComponent(s);
            cooldownTime = s.CooldownTime;
            manaCost = s.ManaCost;
            return gameObject;
        }

        public static GameObject BuildButton(Vector2 position, Texture2D normalTex, Texture2D hoverTex, Button.ClickDelegate onClick, Button.ClickDelegate onRightClick = null, bool wiggle = false)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new Button(gameObject, normalTex, hoverTex, onClick, onRightClick, wiggle));
            return gameObject;
        }
    }
}