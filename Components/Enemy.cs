using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Enemy : Component, ICanBeLoaded, ICanUpdate, ICanBeAnimated, IEnterCollision, IExitCollision,
        IStayOnCollision
    {
        private float moveSpeed;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Transform transform;
        private FacingDirection fDirection;
        private MovingDirection mDirection;
        private float targetingRange;
        private float attackingRange;
        private float attackSpeed;
        private float attackDamage;
        private float health;
        private Strategy strategy;

        public Enemy(GameObject gameObject, int lvl, bool isRanged) : base(gameObject)
        {
            moveSpeed = 100;
            mDirection = MovingDirection.Idle;
            fDirection = FacingDirection.Front;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GameObject.GetComponent("Animator");
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpirteRenderer");
            transform = GameObject.Transform;
            strategy = new Strategy(animator, transform, moveSpeed);
            CreateAnimation();
        }

        private void CreateAnimation()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            foreach (GameObject potentialTarget in GameWorld.Instance.ActiveObjects)
            {
                if (potentialTarget.Components.Contains((Player)potentialTarget.GetComponent("Player")))
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();
                    if (lengthToTarget <= attackingRange)
                    {
                        strategy.Attack(fDirection, attackingRange);
                    }
                    else if (lengthToTarget <= targetingRange)
                    {
                        if (potentialTarget.Transform.Position.X > transform.Position.X &&
                        potentialTarget.Transform.Position.Y > transform.Position.Y)
                        {
                            mDirection = MovingDirection.DownRight;
                            fDirection = FacingDirection.Right;
                        }
                        strategy.Move(mDirection);
                    }
                    else
                    {
                        strategy.Idle(fDirection);
                    }
                    break;
                }
            }
        }

        public void OnAnimationDone(string animationsName)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionEnter(Collider other)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionStay(Collider other)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionExit(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}