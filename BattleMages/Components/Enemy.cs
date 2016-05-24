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
        private float attackRange;
        private float targetingRange;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Transform transform;
        private Character character;
        private Collider collider;
        private int damage;
        private int health;
        private int potentialBehaviours;
        private float attackSpeed;
        private Dictionary<int, IBehaviour> behaviours = new Dictionary<int, IBehaviour>();
        public int Damage { get { return damage; } set { damage = value; } }
        public int Health { get { return health; } set { health = value; } }
        public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

        public Enemy(GameObject gameObject, int startHealth, float attackRange, float targetingRange) : base(gameObject)
        {
            damage = 10;
            attackSpeed = 0;
            health = startHealth;
            this.attackRange = attackRange;
            this.targetingRange = targetingRange;
            potentialBehaviours = 1;
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

            for (int i = 0; i < potentialBehaviours; i++)
            {
                if (GameObject.GetComponent<Hunt>() != null)
                {
                    behaviours.Add(i, GameObject.GetComponent<Hunt>());
                    GameObject.RemoveComponent<Hunt>();
                }
                GameObject.Update();
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
            for (int i = 0; i < behaviours.Count; i++)
            {
                var behaviour = behaviours.FirstOrDefault(x => x.Key == i).Value;
                if (behaviour != null)
                    behaviour.ExecuteBehaviour(targetingRange, attackRange);
            }

            character.Movement();
            character.MoveDirection = Vector2.Zero;
        }

        public void OnAnimationDone(string animationsName)
        {
        }
    }
}