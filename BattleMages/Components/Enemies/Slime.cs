using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Slime : Enemy
    {
        public Slime(GameObject gameObject) : base(gameObject)
        {
            health = 75;
            damage = 10;
            cooldownTimer = 1;
            attackSpeed = 0;
            targetingRange = 400;
            attackRange = 25;
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            behaviours.Add(0, new Hunt(this));
        }
    }
}