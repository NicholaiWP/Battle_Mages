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
        private IStrategy strategy;
        private FacingDirection fDirection;
        private MovingDirection mDirection;

        public Enemy(GameObject gameObject) : base(gameObject)
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
            CreateAnimation();
        }

        private void CreateAnimation()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            foreach (GameObject go in GameWorld.Instance.ActiveObjects)
            {
                if (go.Components.Contains((Player)go.GetComponent("Player")))
                {
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