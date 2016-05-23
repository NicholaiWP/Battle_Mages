using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Enemy : Component, ICanBeLoaded, ICanUpdate, ICanBeAnimated
    {
        private int level = 1;
        private EnemyType type;
        private EnemyAI enemyAI;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Transform transform;
        private Character character;
        private Collider collider;
        private int damage;
        private int health;
        private float attackSpeed;
        private bool dodge;

        //properties
        public int Level { get { return level; } }

        public bool IsAttacking { get; set; }
        public int Damage { get { return damage; } set { damage = value; } }
        public int Health { get { return health; } set { health = value; } }
        public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

        public Enemy(GameObject gameObject, int startHealth, EnemyType type, bool dodge) : base(gameObject)
        {
            IsAttacking = false;
            damage = 10;
            attackSpeed = 0;
            health = startHealth;
            this.type = type;
            this.dodge = dodge;
        }

        public void DealDamage(int points)
        {
            health -= points;
            if (health <= 0)
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }

        public void LoadContent(ContentManager content)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();
            switch (type)
            {
                case EnemyType.CloseRange:
                    enemyAI = new EnemyCloseRange(character, this, transform, dodge);
                    break;

                case EnemyType.Ranged:
                    enemyAI = new EnemyRanged(character, this, transform, dodge);
                    break;

                case EnemyType.DodgingCloseRange:
                    break;

                case EnemyType.DodgingRanged:
                    break;

                case EnemyType.Boss:
                    break;
            }
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
        public void Update()
        {
            Move();
        }

        private void Move()
        {
            if (enemyAI.InAttackRange() && attackSpeed <= 0)
            {
                enemyAI.Attack();
                attackSpeed = 3;
            }
            else
            {
                attackSpeed -= GameWorld.DeltaTime;
                IsAttacking = false;
            }
            if (dodge)
            {
                enemyAI.Dodge();
            }

            if (!enemyAI.IsDodging)
            {
                enemyAI.Targeting();
            }
            character.Movement();
            character.Up = false;
            character.Down = false;
            character.Right = false;
            character.Left = false;
            enemyAI.IsDodging = false;
        }

        public void OnAnimationDone(string animationsName)
        {
        }
    }
}