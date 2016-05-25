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
        private Animator animator;

        public Attack(Enemy enemy, float attackRange, float targetingRange)
        {
            closeRange = 35;
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

            foreach (GameObject potentialTarget in GameWorld.CurrentScene.ActiveObjects)
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

        private void RangeAttack(GameObject potentialTarget)
        {
            if (attackTimer <= 0)
            {
                attackTimer = enemy.CooldownTimer;
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
                attackTimer = enemy.CooldownTimer;
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