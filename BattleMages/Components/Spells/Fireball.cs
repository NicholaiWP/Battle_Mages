﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Fireball : Spell, ICanBeDrawn, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Collider collider;

        public Fireball(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Damage = 10;
            CooldownTime = 0.3f;
            ApplyRunes();

            var diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 120f;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/fireball");

            collider = new Collider(GameObject, new Vector2(8, 8));
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DealDamage(Damage);
                    GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }
        }
    }
}