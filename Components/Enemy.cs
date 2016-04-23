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

        /// <summary>
        /// in this method the enemy searches in the if statement after its potential target,
        /// which in this case is the gameobject with the component "player".
        /// When found the distance between the enemy and the player is calculated.
        /// If the distance is less or equal to the corresponding enemy attack range,
        /// the enemy will attempt to attack the player,
        /// if the player isnt in the enemy's range the enemy will be put into its idle state.
        /// </summary>
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
                        if (attackSpeed <= 0)
                        {
                            attackSpeed = 5;
                            strategy.Attack(fDirection, attackingRange);
                        }
                        else
                        {
                            attackSpeed -= GameWorld.Instance.DeltaTime;
                            strategy.Idle(fDirection);
                        }
                    }
                    else if (lengthToTarget <= targetingRange)
                    {
                        bool up = transform.Position.Y > potentialTarget.Transform.Position.Y;
                        bool down = transform.Position.Y < potentialTarget.Transform.Position.Y;

                        if (potentialTarget.Transform.Position.X > transform.Position.X &&
                        potentialTarget.Transform.Position.Y > transform.Position.Y)
                        {
                            mDirection = MovingDirection.DownRight;
                            fDirection = FacingDirection.Right;
                        }
                        else
                        {
                            mDirection = MovingDirection.Idle;
                            strategy.Idle(fDirection);
                        }
                        strategy.Move(mDirection);
                    }
                    else
                    {
                        attackSpeed -= GameWorld.Instance.DeltaTime;
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