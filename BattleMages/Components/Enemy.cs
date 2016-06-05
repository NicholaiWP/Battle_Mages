using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public abstract class Enemy : Component
    {
        //Component caching
        private SpriteRenderer spriteRenderer;

        private Transform transform;
        private Collider collider;
        protected Animator animator;
        protected Character character;
        protected bool canMove;
        protected float attackRange;
        protected float targetingRange;
        protected int damage;
        protected int health;
        protected int moneyAmount;
        protected float attackSpeed;
        protected float cooldownTimer;
        protected List<IBehaviour> behaviours = new List<IBehaviour>();
        private float red;

        public int Damage { get { return damage; } }
        public int Health { get { return health; } }
        public bool IsAlreadyBurned { get; set; }
        public float CooldownTimer { get { return cooldownTimer; } }

        protected int MoneyAmount
        {
            get { return moneyAmount; }
            set { moneyAmount = value; }
        }

        protected float MoveSpeed
        {
            get { return character.MoveSpeed; }
            set { character.MoveSpeed = value; }
        }

        protected float MoveAccel
        {
            get { return character.MoveAccel; }
            set { character.MoveAccel = value; }
        }

        protected Enemy()
        {
            canMove = true;
            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        public void TakeDamage(int points)
        {
            if (health > 0)
            {
                GameWorld.Scene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, points.ToString()));
                health -= points;
                red = 1;

                if (health <= 0)
                {
                    //Spawns a number of coins when enemy dies
                    for (int i = 0; i < moneyAmount; i++)
                    {
                        GameWorld.Scene.AddObject(ObjectBuilder.BuildCoin(transform.Position));
                    }
                    canMove = false;
                    GameObject.RemoveComponent<Collider>();
                    animator.PlayAnimation("Death" + character.FDirection.ToString());
                }
            }
        }

        protected virtual void PreInitialize(PreInitializeMsg msg)
        {
        }

        protected virtual void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();

            //TODO: Create animations here
        }

        protected virtual void Update(UpdateMsg msg)
        {
            if (canMove)
            {
                Move();
            }

            if (red > 0)
            {
                red -= GameWorld.DeltaTime * 10;
                spriteRenderer.Color = new Color(1f, 1f - red, 1f - red);
            }
        }

        private void Move()
        {
            foreach (IBehaviour behaviour in behaviours)
            {
                behaviour.ExecuteBehaviour();
            }

            character.Movement();
            character.MoveDirection = Vector2.Zero;
        }

        private void AnimationDone(AnimationDoneMsg msg)
        {
            if (msg.AnimationName == "DeathRight" || msg.AnimationName == "DeathLeft")
            {
                GameObject.RemoveComponent<Animator>();
                GameObject.RemoveComponent<SpriteRenderer>();
                GameWorld.Scene.RemoveObject(GameObject);
            }

            animator.PlayAnimation("Idle" + character.FDirection);
        }
    }
}