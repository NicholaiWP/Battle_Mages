using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class EarthSpikes : Spell
    {
        private Collider collider;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private float timer;
        private float damageTimer;
        private SpellCreationParams p;

        private bool upPlayed;

        public EarthSpikes(SpellCreationParams p) : base(p)
        {
            this.p = p;
            damageTimer = 0;

            GameWorld.SoundManager.PlaySound("Earthspikes");
            GameWorld.SoundManager.SoundVolume = 1f;

            timer = 4;
            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        private void PreInitialize(PreInitializeMsg msg)
        {
            spriteRenderer = new SpriteRenderer("Textures/Spells/EarthSpikes", true) { Rectangle = new Rectangle(0, 0, 32, 32) };
            GameObject.AddComponent(spriteRenderer);

            animator = new Animator();
            animator.CreateAnimation("Up", new Animation(0, 8, 0, 0, 32, 32, 20, Vector2.Zero));
            animator.CreateAnimation("Down", new Animation(0, 8, 0, 8, 32, 32, 20, Vector2.Zero));
            GameObject.AddComponent(animator);

            collider = new Collider(new Vector2(spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height));
            GameObject.AddComponent(collider);
        }

        private void Initialize(InitializeMsg msg)
        {
            GameObject.Transform.Position = p.AimTarget;
        }

        private void Update(UpdateMsg msg)
        {
            if (!upPlayed)
            {
                upPlayed = true;
                animator.PlayAnimation("Up", looping: false);
            }

            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null && damageTimer <= 0)
                {
                    damageTimer = 0.6f;
                    enemy.TakeDamage(Stats.Damage);
                }
            }

            timer -= GameWorld.DeltaTime;
            if (timer <= 0)
            {
                animator.PlayAnimation("Down", looping: false);
            }

            if (damageTimer > 0)
            {
                damageTimer -= GameWorld.DeltaTime;
            }
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
            if (animator.PlayingAnimationName == "Down")
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }
    }
}