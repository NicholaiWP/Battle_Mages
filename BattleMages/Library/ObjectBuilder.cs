using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public static class ObjectBuilder
    {
        public static GameObject BuildPlayer(Vector2 position, bool canUseSpells)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new SpriteRenderer("Textures/Player/PlayerSheet", true)
            { Rectangle = new Rectangle(0, 0, 32, 32) });
            gameObject.AddComponent(new Animator());
            gameObject.AddComponent(new Player(canUseSpells));
            gameObject.AddComponent(new Collider(new Vector2(16, 16)) { Offset = new Vector2(0, 8) });
            gameObject.AddComponent(new Character());
            return gameObject;
        }

        public static GameObject BuildEnemy(Vector2 position, Enemy enemy)
        {
            GameObject gameObject = new GameObject(position);

            gameObject.AddComponent(new Animator());
            gameObject.AddComponent(new Character() { MoveSpeed = 40 });
            gameObject.AddComponent(enemy);
            gameObject.AddComponent(new Collider(new Vector2(32, 32)));
            return gameObject;
        }

        public static GameObject BuildCoin(Vector2 position)
        {
            GameObject gameObject = new GameObject(position);

            gameObject.AddComponent(new Collider(new Vector2(12, 12), false));
            gameObject.AddComponent(new SpriteRenderer("Textures/Misc/Coin", layerToUse: DrawLayer.OnGround));
            gameObject.AddComponent(new Animator());
            gameObject.AddComponent(new Coin());
            return gameObject;
        }

        public static GameObject BuildFlyingLabelText(Vector2 position, string text)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new FloatingText(text));
            return gameObject;
        }

        public static GameObject BuildInvisibleWall(Vector2 position, Vector2 size)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new Collider(size, true));
            return gameObject;
        }

        public static GameObject BuildButton(Vector2 position, Texture2D normalTex, Texture2D hoverTex, Button.ClickDelegate onClick, Button.ClickDelegate onRightClick = null, bool wiggle = false)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new Button(normalTex, hoverTex, onClick, onRightClick, wiggle));
            return gameObject;
        }

        public static GameObject BuildDraggable(string tag, Vector2 position, Texture2D normalTex, Texture2D hoverTex, Action onDragStart)
        {
            GameObject gameObject = new GameObject(position);
            gameObject.AddComponent(new DragDropItem(tag, normalTex, hoverTex, onDragStart));
            return gameObject;
        }
    }
}