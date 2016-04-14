using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            animator = (Animator)GameObject.GetComponent("Animator");
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpirteRenderer");
            transform = GameObject.Transform;
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

            KeyboardState kbState = Keyboard.GetState();
            bool up = kbState.IsKeyDown(Keys.W);
            bool down = kbState.IsKeyDown(Keys.S);
            bool left = kbState.IsKeyDown(Keys.A);
            bool right = kbState.IsKeyDown(Keys.D);

            if (up && right)
            {
                mDirection = MovingDirection.UpRight;
                fDirection = FacingDirection.Right;
            }
            else if (up && left)
            {
                mDirection = MovingDirection.UpLeft;
                fDirection = FacingDirection.Left;
            }
            else if (down && right)
            {
                mDirection = MovingDirection.DownRight;
                fDirection = FacingDirection.Right;
            }
            else if (down && left)
            {
                mDirection = MovingDirection.DownLeft;
                fDirection = FacingDirection.Left;
            }
            else if (up)
            {
                SoundManager.Instance.PlaySound("FireBall");
                mDirection = MovingDirection.Up;
                fDirection = FacingDirection.Back;
            }
            else if (left)
            {
                mDirection = MovingDirection.Left;
                fDirection = FacingDirection.Left;
            }
            else if (down)
            {
                mDirection = MovingDirection.Down;
                fDirection = FacingDirection.Front;
            }
            else if (right)
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