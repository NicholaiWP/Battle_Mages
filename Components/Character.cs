using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Character : Component
    {
        private EnemyAI enemyAI;
        private MovingDirection mDirection;
        private FacingDirection fDirection;
        private Strategy strategy;
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public Character(GameObject gameObject) : base(gameObject)
        {
        }

        public void Load()
        {
            strategy = new Strategy(GameObject.GetComponent<Animator>(),
                GameObject.Transform, 100);

            if (GameObject.GetComponent<Enemy>() != null)
            {
                {
                    enemyAI = new EnemyRanged(this, GameObject.GetComponent<Enemy>(),
                        GameObject.Transform);
                }
            }
        }

        public void PlayerMove()
        {
            KeyboardState kbState = Keyboard.GetState();

            Up = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Up));
            Down = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Down));
            Left = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Left));
            Right = kbState.IsKeyDown(GameWorld.PlayerControls.GetBinding(PlayerBind.Right));

            Movement();
        }

        public void EnemyMove()
        {
            enemyAI.Targeting();
            Movement();
            Up = false;
            Down = false;
            Right = false;
            Left = false;
        }

        private void Movement()
        {
            if (Up && Right)
            {
                mDirection = MovingDirection.UpRight;
                fDirection = FacingDirection.Right;
            }
            else if (Up && Left)
            {
                mDirection = MovingDirection.UpLeft;
                fDirection = FacingDirection.Left;
            }
            else if (Down && Right)
            {
                mDirection = MovingDirection.DownRight;
                fDirection = FacingDirection.Right;
            }
            else if (Down && Left)
            {
                mDirection = MovingDirection.DownLeft;
                fDirection = FacingDirection.Left;
            }
            else if (Up)
            {
                mDirection = MovingDirection.Up;
                fDirection = FacingDirection.Back;
            }
            else if (Left)
            {
                mDirection = MovingDirection.Left;
                fDirection = FacingDirection.Left;
            }
            else if (Down)
            {
                mDirection = MovingDirection.Down;
                fDirection = FacingDirection.Front;
            }
            else if (Right)
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