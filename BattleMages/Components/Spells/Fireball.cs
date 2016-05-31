using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class Fireball : Spell
    {
        private Vector2 velocity;
        private Collider collider;
        private SpriteRenderer spriteRenderer;
        private Vector2 diff;
        private SpellCreationParams p;

        public Fireball(SpellCreationParams p) : base(p)
        {
            this.p = p;
            Damage = 12;
            CooldownTime = 0.7f;
            ManaCost = 12;
            ApplyAttributeRunes();
            spriteRenderer = new SpriteRenderer("Textures/Spells/Fireball");
            collider = new Collider(new Vector2(8, 8));
            GameWorld.SoundManager.PlaySound("fireball");
            GameWorld.SoundManager.SoundVolume = 0.9f;

            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void PreInitialize(PreInitializeMsg msg)
        {
            GameObject.AddComponent(spriteRenderer);
            GameObject.AddComponent(collider);
        }

        private void Initialize(InitializeMsg msg)
        {
            diff = p.AimTarget - GameObject.Transform.Position;
            diff.Normalize();
            velocity = diff * 150;
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