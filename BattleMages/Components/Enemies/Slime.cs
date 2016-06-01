using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Slime : Enemy
    {
        public Slime()
        {
            health = 100;
            damage = 15;
            cooldownTimer = 1;
            attackSpeed = 0;
            targetingRange = 400;
            attackRange = 25;
        }

        protected override void PreInitialize(PreInitializeMsg msg)
        {
            base.PreInitialize(msg);
            GameObject.AddComponent(new SpriteRenderer("Textures/Enemies/slimeSpriteSheet")
            { Rectangle = new Rectangle(0, 0, 32, 32) });
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));

            animator.CreateAnimation("AttackRight", new Animation(priority: 1, framesCount: 14, yPos: 0, xStartFrame: 0,
                width: 32, height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("AttackLeft", new Animation(priority: 1, framesCount: 14, yPos: 32, xStartFrame: 0,
                width: 32, height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("WalkRight", new Animation(priority: 2, framesCount: 10, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("WalkLeft", new Animation(priority: 2, framesCount: 10, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 20, offset: Vector2.Zero));
            animator.CreateAnimation("IdleRight", new Animation(priority: 3, framesCount: 1, yPos: 64, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));
            animator.CreateAnimation("IdleLeft", new Animation(priority: 3, framesCount: 1, yPos: 96, xStartFrame: 0,
                width: 32, height: 32, fps: 1, offset: Vector2.Zero));
            animator.CreateAnimation("DieLeft", new Animation(priority: 0, framesCount: 12, yPos: 128, xStartFrame: 0,
                width: 32, height: 32, fps: 12, offset: Vector2.Zero));
            animator.CreateAnimation("DieRight", new Animation(priority: 0, framesCount: 12, yPos: 160, xStartFrame: 0,
                width: 32, height: 32, fps: 12, offset: Vector2.Zero));
        }
    }
}