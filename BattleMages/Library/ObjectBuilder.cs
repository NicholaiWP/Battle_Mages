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

        public static GameObject BuildEnemy(Vector2 position, float targetingRange, float attackRange, List<IBehaviour> behaviours)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer(gameObject, "Images/EvilMageBM"));
            gameObject.AddComponent(new Animator(gameObject));
            gameObject.AddComponent(new Character(gameObject) { MoveSpeed = 40 });

            //All behaviours in the list have a null gameObject
            foreach (IBehaviour behaviour in behaviours)
            {
                //Using the activator to create a new instance of the same type as the behaviour with the gameobject,
                //so the behaviour which the gameObject adds will have a connection to the gameObject
                var alikeBehaviour = Activator.CreateInstance(behaviour.GetType(), gameObject);
                if (behaviour is Component)
                {
                    //Adding the alikeBehaviour to the gameObject
                    gameObject.AddComponent(alikeBehaviour as Component);
                }
            }

            gameObject.AddComponent(new Enemy(gameObject, 100, targetingRange, attackRange));
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

        public static GameObject BuildButton(Vector2 position, Texture2D normalTex, Texture2D hoverTex, Button.ClickDelegate onClick, Button.ClickDelegate onRightClick = null, bool wiggle = false)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new Button(gameObject, normalTex, hoverTex, onClick, onRightClick, wiggle));
            return gameObject;
        }
    }
}