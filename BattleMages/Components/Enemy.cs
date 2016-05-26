using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public abstract class Enemy : Component
    {
        //Component caching
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Transform transform;
        private Character character;
        private Collider collider;


        protected float attackRange;
        protected float targetingRange;
        protected int damage;
        protected int health;
        protected float attackSpeed;
        protected float cooldownTimer;
        protected List<IBehaviour> behaviours = new List<IBehaviour>();
        public int Damage { get { return damage; } }
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

        protected Enemy(GameObject gameObject) : base(gameObject)
        {
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<AnimationDoneMsg>(AnimationDone);
        }

        public void TakeDamage(int points)
        {
            health -= points;
            if (health <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
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
        private void Update(UpdateMsg msg)
        {
            Move();
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
        }
    }
}