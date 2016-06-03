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
        protected float attackSpeed;
        protected float cooldownTimer;
        protected List<IBehaviour> behaviours = new List<IBehaviour>();
        public int Damage { get { return damage; } }
        public int Health { get { return health; } }
        public bool IsAlreadyBurned { get; set; } 
        public float CooldownTimer { get { return cooldownTimer; } }
      

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
            }

            if (health <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
                for (int i = 0; i < 10; i++)
                {
                    GameWorld.Scene.AddObject(ObjectBuilder.BuildCoin(transform.Position));
                }
                canMove = false;
                GameObject.RemoveComponent<Collider>();
                animator.PlayAnimation("Death" + character.FDirection.ToString());
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

        /// <summary>
        /// in this method the enemy searches in the if statement after its potential target,
        /// which in this case is the gameobject with the component "player".
        /// When found the distance between the enemy and the player is calculated.
        /// If the distance is less or equal to the corresponding enemy attack range,
        /// the enemy will attempt to attack the player,
        /// if the player isnt in the enemy's range the enemy will be put into its idle state.
        /// </summary>
        protected virtual void Update(UpdateMsg msg)
        {
            if (canMove)
            {
                Move();
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