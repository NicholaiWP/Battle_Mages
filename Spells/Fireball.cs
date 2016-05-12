using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battle_Mages
{
    internal class Fireball : Spell, ICanBeDrawn, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;

        public Fireball(GameObject go, Vector2 targetPos, RuneInfo[] runes) : base(go, runes)
        {
            Damage = 10;
            ApplyRunes();

            var diff = targetPos - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 100f;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Images/fireball");
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.Instance.DeltaTime;
        }
    }
}