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

        public Character(GameObject gameObject) : base(gameObject)
        {
        }

        public void Move()
        {
            if (GameObject.GetComponent<Player>() != null)
            {
                if (strategy == null)
                {
                    strategy = new Strategy(GameObject.GetComponent<Animator>(),
                        GameObject.Transform, 100);
                }
                PlayerMovement();
            }
            else if (GameObject.GetComponent<Enemy>() != null)
            {
                if (enemyAI == null)
                {
                    enemyAI = new EnemyRanged(GameObject.GetComponent<Animator>(),
                        GameObject.GetComponent<Enemy>(),
                        GameObject.Transform);
                }
                enemyAI.Targeting();
            }
        }

        private void PlayerMovement()
        {
            KeyboardState kbState = Keyboard.GetState();

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