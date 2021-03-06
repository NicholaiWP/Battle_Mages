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
        private Vector2 velocity;
        private Collider collider;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Vector2 diff;
        private SpellCreationParams p;
        private float distanceTravelled;

        public Fireball(SpellCreationParams p) : base(p)
        {
            this.p = p;
            spriteRenderer = new SpriteRenderer("Textures/Spells/fireball-Sheet", true) { Rectangle = new Rectangle(0, 0, 8, 8) };
            animator = new Animator();
            animator.CreateAnimation("", new Animation(0, 6, 0, 0, 8, 8, 15, Vector2.Zero));
            collider = new Collider(new Vector2(8, 8));
            GameWorld.SoundManager.PlaySound("fireball", volume: 0.9f);

            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void PreInitialize(PreInitializeMsg msg)
        {
            GameObject.AddComponent(spriteRenderer);
            GameObject.AddComponent(collider);
            GameObject.AddComponent(animator);
        }

        private void Initialize(InitializeMsg msg)
        {
            diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 125;
        }

        private void Update(UpdateMsg msg)
        {
            animator.PlayAnimation("", (float)Math.Atan2(velocity.Y, velocity.X));

            Vector2 translation = velocity * GameWorld.DeltaTime;
            GameObject.Transform.Position += translation;
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)Stats.Damage);
                    Utils.HealOnHit((int)(Stats.Damage * Stats.LifeSteal));
                    //damage of the burn effect
                    Onfire(3, enemy);
                    GameWorld.Scene.RemoveObject(GameObject);
                    break;
                }
            }

            distanceTravelled += translation.Length();

            if (!Utils.InsideCircle(GameObject.Transform.Position, Vector2.Zero, 320) || distanceTravelled > Stats.Range)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }

        private void Onfire(int burnPoints, Enemy enemy)
        {
            //timer dmg timer, når nul..if the timer in update is <= 0, enemy.takedamge. add gameObject to show

            int chance = GameWorld.Random.Next(1, 101);

            if (chance <= 25 && !enemy.IsAlreadyBurned) // probability of 25%
            {
                enemy.IsAlreadyBurned = true;
                GameWorld.SoundManager.PlaySound("BurnSound");
                GameObject go = new GameObject(enemy.GameObject.Transform.Position);
                go.AddComponent(new Burn(enemy, burnPoints));
                GameWorld.Scene.AddObject(go);
            }
        }
    }
}