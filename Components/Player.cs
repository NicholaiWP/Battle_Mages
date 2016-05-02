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
        private Strategy strategy;

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
            strategy = new Strategy(animator, transform, moveSpeed);
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
            KeyboardState kbState = Keyboard.GetState();
            /*Keys[] keyPressed = kbState.GetPressedKeys();
            if (keyPressed.Length == 1)
            {
                GameWorld.Instance.playerControls.PlayerKeys["UpKey"] =
                    GameWorld.Instance.playerControls.ChangeBinding(GameWorld.Instance.playerControls.PlayerKeys["UpKey"],
                    keyPressed[0]);
            }*/
            bool up = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up));
            bool down = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down));
            bool left = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left));
            bool right = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right));

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
                strategy.Idle(fDirection);
            }
            strategy.Move(mDirection);
        }
    }
}