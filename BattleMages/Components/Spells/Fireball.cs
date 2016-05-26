﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Fireball : Spell
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Collider collider;
        private Vector2 diff;


        public Fireball(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Damage = 15;
            CooldownTime = 0.8f;
            ManaCost = 15;
            ApplyAttributeRunes();

            diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 150;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/fireball");

            collider = new Collider(GameObject, new Vector2(8, 8));
            GameObject.AddComponent(collider);
            GameWorld.SoundManager.PlaySound("fireball");
            GameWorld.SoundManager.SoundVolume = 0.9f;

            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Draw(DrawMsg msg)
        {
            msg.Drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        private void Update(UpdateMsg msg)
        {

            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                    enemy.Onfire(6);

                    GameWorld.Scene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.Scene.RemoveObject(GameObject);
                }
            }

            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320))
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }
    }
}