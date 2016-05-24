using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Hunt : Component, IBehaviour

    {
        private Transform transform;
        private Character character;
        private Enemy enemy;
        private int closeRange;
        private float attackTimer;

        public Hunt(GameObject gameObject) : base(gameObject)
        {
            closeRange = 25;
            attackTimer = 0;

            Listen<InitializeMsg>(Initialize);
        }

        private void Initialize(InitializeMsg msg)
        {
            transform = GameObject.Transform;
            character = GameObject.GetComponent<Character>();
            enemy = GameObject.GetComponent<Enemy>();
        }

        public void ExecuteBehaviour(float attackRange, float targetingRange)
        {
            if (attackTimer >= 0)
                attackTimer -= GameWorld.DeltaTime;

            foreach (GameObject potentialTarget in GameWorld.CurrentScene.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();

                    if (lengthToTarget <= targetingRange && !InAttackRange(lengthToTarget, attackRange))
                    {
                        Vector2 movement = Vector2.Zero;
                        if (transform.Position.Y - 10 > potentialTarget.Transform.Position.Y + 10)
                            movement.Y -= 1;
                        if (transform.Position.Y + 10 < potentialTarget.Transform.Position.Y - 10)
                            movement.Y += 1;
                        if (transform.Position.X > potentialTarget.Transform.Position.X)
                            movement.X -= 1;
                        if (transform.Position.X < potentialTarget.Transform.Position.X)
                            movement.X += 1;
                        character.MoveDirection = movement;
                    }
                    else if (InAttackRange(lengthToTarget, attackRange))
                    {
                        if (attackRange == closeRange)
                        {
                            CloseAttack(potentialTarget);
                        }
                        else if (attackRange > closeRange)
                        {
                            RangeAttack(potentialTarget);
                        }
                    }
                    break;
                }
            }
        }

        private void RangeAttack(GameObject potentialTarget)
        {
            if (attackTimer <= 0)
            {
                attackTimer = 2;
                GameObject projectile = new GameObject(transform.Position);
                projectile.AddComponent(new Projectile(projectile, enemy, potentialTarget.Transform.Position));
                GameWorld.CurrentScene.AddObject(projectile);
            }
        }

        private void CloseAttack(GameObject potentialTarget)
        {
            if (attackTimer <= 0)
            {
                potentialTarget.GetComponent<Player>().TakeDamage(enemy.Damage);
                attackTimer = 4;
            }
        }

        private bool InAttackRange(float lengthToTarget, float attackRange)
        {
            if (lengthToTarget <= attackRange)
            {
                return true;
            }
            return false;
        }
    }
}