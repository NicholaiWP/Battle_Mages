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
            if (GameObject != null)
                GameObject.AddComponent(new SpriteRenderer(GameObject, "Enemy Images/golemEnemy"));
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            MoveSpeed = 20;
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
        }
    }
}