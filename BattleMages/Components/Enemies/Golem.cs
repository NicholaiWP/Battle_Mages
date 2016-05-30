using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Golem : Enemy
    {
        public Golem()
        {
            health = 1;
            damage = 25;
            cooldownTimer = 3.5f;
            attackSpeed = 0;
            targetingRange = 700;
            attackRange = 25;
        }

        protected override void PreInitialize(PreInitializeMsg msg)
        {
            base.PreInitialize(msg);
            GameObject.AddComponent(new SpriteRenderer("Enemy Images/golemEnemy"));
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            MoveSpeed = 20;
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
            behaviours.Add(new AreaAttack(this, 85, 0.8f, 0.6f, 3f, 80, 25));
        }
    }
}