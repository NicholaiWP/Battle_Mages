using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class Golem : Enemy
    {
        public Golem()
        {
            moneyAmount = 10;
            MaxHealth = 100;
            damage = 25;
            cooldownTimer = 3.5f;
            attackSpeed = 0;
            targetingRange = 700;
            attackRange = 16;
        }

        protected override void PreInitialize(PreInitializeMsg msg)
        {
            base.PreInitialize(msg);
            GameObject.AddComponent(new SpriteRenderer("Textures/Enemies/golemSpriteSheet", true)
            { Rectangle = new Rectangle(0, 0, 32, 32) });
            GameObject.AddComponent(new Animator());
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            MoveSpeed = 20;
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new AreaAttack(this, 85, 1f, 0.6f, 3f, 80, 25));
            behaviours.Add(new Attack(this, attackRange, targetingRange));

            animator.CreateAnimation("GroundRight", new Animation(priority: 1, framesCount: 30, yPos: 0, xStartFrame: 0, width: 32,
                height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("GroundLeft", new Animation(priority: 1, framesCount: 30, yPos: 32, xStartFrame: 0, width: 32,
                height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("DeathLeft", new Animation(priority: 0, framesCount: 20, yPos: 64, xStartFrame: 0, width: 32,
                height: 32, fps: 10, offset: Vector2.Zero));
            animator.CreateAnimation("DeathRight", new Animation(priority: 0, framesCount: 20, yPos: 96, xStartFrame: 0, width: 32,
                height: 32, fps: 10, offset: Vector2.Zero));
            animator.CreateAnimation("AttackLeft", new Animation(priority: 2, framesCount: 12, yPos: 128, xStartFrame: 0, width: 32,
                height: 32, fps: 12, offset: Vector2.Zero));
            animator.CreateAnimation("AttackRight", new Animation(priority: 2, framesCount: 12, yPos: 160, xStartFrame: 0, width: 32,
                height: 32, fps: 12, offset: Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(priority: 3, framesCount: 19, yPos: 192, xStartFrame: 0, width: 32,
                height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(priority: 3, framesCount: 19, yPos: 224, xStartFrame: 0, width: 32,
                height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("IdleLeft", new Animation(priority: 4, framesCount: 1, yPos: 192, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));
            animator.CreateAnimation("IdleRight", new Animation(priority: 4, framesCount: 1, yPos: 224, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));
        }
    }
}