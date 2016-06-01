using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Attack : IBehaviour
    {
        private Transform transform;
        private Enemy enemy;
        private Character character;
        private int closeRange;
        private float attackTimer;
        private float attackRange;
        private float attackDelay;
        private Animator animator;

        public Attack(Enemy enemy, float attackRange, float targetingRange)
        {
            attackDelay = 0.2f;
            closeRange = 45;
            attackTimer = 0;
            this.enemy = enemy;
            this.attackRange = attackRange;
            transform = enemy.GameObject.Transform;
            character = enemy.GameObject.GetComponent<Character>();
            animator = enemy.GameObject.GetComponent<Animator>();
        }

        public void ExecuteBehaviour()
        {
            if (attackTimer >= 0)
                attackTimer -= GameWorld.DeltaTime;
            else
            {
                foreach (GameObject potentialTarget in GameWorld.Scene.ActiveObjects)
                {
                    if (potentialTarget.GetComponent<Player>() != null)
                    {
                        Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                        float lengthToTarget = vecToTarget.Length();
                        if (InAttackRange(lengthToTarget))
                        {
                            if (attackRange <= closeRange)
                            {
                                CloseAttack(potentialTarget);
                            }
                            else
                            {
                                RangeAttack(potentialTarget);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void RangeAttack(GameObject potentialTarget)
        {
            animator.PlayAnimation("Attack" + character.FDirection.ToString());
            attackTimer = enemy.CooldownTimer;
            GameObject projectile = new GameObject(transform.Position);
            projectile.AddComponent(new Projectile(enemy, potentialTarget.Transform.Position));
            GameWorld.Scene.AddObject(projectile);
        }

        private void CloseAttack(GameObject potentialTarget)
        {
            if (animator != null)
            {
                animator.PlayAnimation("Attack" + character.FDirection.ToString());
            }
            if (attackDelay <= 0)
            {
                attackDelay = 0.2f;
                potentialTarget.GetComponent<Player>().TakeDamage(enemy.Damage);
                attackTimer = enemy.CooldownTimer;
            }
            else
            {
                attackDelay -= GameWorld.DeltaTime;
            }
        }

        private bool InAttackRange(float lengthToTarget)
        {
            if (lengthToTarget <= attackRange)
            {
                return true;
            }
            return false;
        }
    }
}