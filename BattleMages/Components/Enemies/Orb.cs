using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Orb : Enemy
    {
        public Orb()
        {
            health = 85;
            damage = 10;
            cooldownTimer = 2;
            attackSpeed = 0;
            targetingRange = 750;
            attackRange = 300;
        }

        protected override void PreInitialize(PreInitializeMsg msg)
        {
            base.PreInitialize(msg);
            GameObject.AddComponent(new SpriteRenderer("Textures/Enemies/orbSpriteSheet", true)
            { Rectangle = new Rectangle(0, 0, 32, 32) });
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            MoveSpeed = 95;
            MoveAccel = 200;
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
            behaviours.Add(new Dodging(this, 96));

            animator.CreateAnimation("IdleRight", new Animation(priority: 2, framesCount: 6, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 6, offset: Vector2.Zero));
            animator.CreateAnimation("IdleLeft", new Animation(priority: 2, framesCount: 6, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 6, offset: Vector2.Zero));
            animator.CreateAnimation("AttackRight", new Animation(priority: 1, framesCount: 7, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 10, offset: Vector2.Zero));
            animator.CreateAnimation("AttackLeft", new Animation(priority: 1, framesCount: 7, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 10, offset: Vector2.Zero));
            animator.CreateAnimation("DeathRight", new Animation(priority: 0, framesCount: 17, yPos: 128, xStartFrame: 0,
                width: 32, height: 32, fps: 10, offset: Vector2.Zero));
            animator.CreateAnimation("DeathLeft", new Animation(priority: 0, framesCount: 17, yPos: 160, xStartFrame: 0,
                width: 32, height: 32, fps: 10, offset: Vector2.Zero));
        }

        protected override void Update(UpdateMsg msg)
        {
            foreach (GameObject go in GameWorld.Scene.ActiveObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    if (go.Transform.Position.X > GameObject.Transform.Position.X)
                    {
                        character.FDirection = FacingDirection.Right;
                        animator.PlayAnimation("IdleRight");
                    }
                    else if (go.Transform.Position.X < GameObject.Transform.Position.X)
                    {
                        character.FDirection = FacingDirection.Left;
                        animator.PlayAnimation("IdleLeft");
                    }
                }
            }
            base.Update(msg);
        }
    }
}