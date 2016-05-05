using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Player : Component, ICanUpdate, ICanBeLoaded
    {
        private float moveSpeed;
        private Animator animator;
        private Character character;
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        public MovingDirection MDirection { get; set; }
        public FacingDirection FDirection { get; set; }

        public Player(GameObject gameObject) : base(gameObject)
        {
            moveSpeed = 100;
            MDirection = MovingDirection.Idle;
            FDirection = FacingDirection.Front;
        }

        public void LoadContent(ContentManager content)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            character = GameObject.GetComponent<Character>();
            transform = GameObject.Transform;
            CreateAnimation();
        }

        private void CreateAnimation()
        {
            //animator.CreateAnimation("MoveUp", new Animation())
        }

        public void Update()
        {
            character.Move();
        }
    }
}