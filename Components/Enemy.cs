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
        private int level;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Transform transform;
        private FacingDirection fDirection;
        private MovingDirection mDirection;
        private Character character;
        public int Level { get { return level; } }

        public Enemy(GameObject gameObject) : base(gameObject)
        {
            mDirection = MovingDirection.Idle;
            fDirection = FacingDirection.Front;
            level = 1;
        }

        public void LoadContent(ContentManager content)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            character.Load();
            CreateAnimation();
        }

        private void CreateAnimation()
        {
            //throw new NotImplementedException();
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
            character.EnemyAI.Targeting();
            character.Movement();
            character.Up = false;
            character.Down = false;
            character.Right = false;
            character.Left = false;
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