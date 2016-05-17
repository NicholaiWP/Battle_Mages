﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Enemy : Component, ICanBeLoaded, ICanUpdate, ICanBeAnimated, IEnterCollision, IExitCollision,
        IStayOnCollision
    {
        private int level = 1;
        private EnemyAI enemyAI;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Transform transform;
        private Character character;
        private int damage;
        private float attackSpeed;

        //properties
        public int Level { get { return level; } }
        public bool IsAttacking { get; set; }
        public int Damage { get { return damage; } set { damage = value; } }
        public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

        public Enemy(GameObject gameObject) : base(gameObject)
        {
            IsAttacking = false;
            damage = 10;
            attackSpeed = 5;
        }

        public void LoadContent(ContentManager content)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            enemyAI = new EnemyCloseRange(character, this, transform);
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
                IsAttacking = true;
                attackSpeed = 5000000;
            }
            else
            {
                attackSpeed -= GameWorld.DeltaTime;
                IsAttacking = false;
            }
            enemyAI.Targeting();
            character.Movement();
            character.Up = false;
            character.Down = false;
            character.Right = false;
            character.Left = false;
        }

        public void OnAnimationDone(string animationsName)
        {
        }

        public void OnCollisionEnter(Collider other)
        {
           
        }

        public void OnCollisionStay(Collider other)
        {
        }

        public void OnCollisionExit(Collider other)
        {
        }
    }
}