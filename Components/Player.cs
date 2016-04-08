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
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private MovingDirection mDirection;
        private FacingDirection fDirection;
        private IStrategy strategy;

        public Player(GameObject gameObject) : base(gameObject)
        {
            moveSpeed = 100;
            mDirection = MovingDirection.Idle;
            fDirection = FacingDirection.Front;
        }

        public void LoadContent(ContentManager content)
        {
            animator = (Animator)GetGameObject.GetComponent("Animator");
            spriteRenderer = (SpriteRenderer)GetGameObject.GetComponent("SpirteRenderer");
            transform = GetGameObject.Transform;
            CreateAnimation();
        }

        private void CreateAnimation()
        {
        }

        public void Update()
        {
            MoveInformation();
        }

        private void MoveInformation()
        {
            strategy = new Move(animator, transform, moveSpeed);

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                mDirection = MovingDirection.UpRight;
                fDirection = FacingDirection.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                mDirection = MovingDirection.UpLeft;
                fDirection = FacingDirection.Left;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                mDirection = MovingDirection.DownRight;
                fDirection = FacingDirection.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                mDirection = MovingDirection.DownLeft;
                fDirection = FacingDirection.Left;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                SoundManager.Instance.PlaySound("FireBall");
                mDirection = MovingDirection.Up;
                fDirection = FacingDirection.Back;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                mDirection = MovingDirection.Left;
                fDirection = FacingDirection.Left;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                mDirection = MovingDirection.Down;
                fDirection = FacingDirection.Front;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                mDirection = MovingDirection.Right;
                fDirection = FacingDirection.Right;
            }
            else
            {
                mDirection = MovingDirection.Idle;
            }

            if (mDirection == MovingDirection.Idle)
            {
                strategy = new Idle(animator);
            }
            strategy.Execute(mDirection, fDirection);
        }
    }
}