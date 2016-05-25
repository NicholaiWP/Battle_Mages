using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Golem : Enemy
    {
        public Golem(GameObject gameObject) : base(gameObject)
        {
            health = 240;
            damage = 25;
            cooldownTimer = 3.5f;
            attackSpeed = 0;
            targetingRange = 700;
            attackRange = 25;
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
        }
    }
}