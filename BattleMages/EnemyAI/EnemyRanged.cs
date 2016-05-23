using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class EnemyRanged : EnemyAI
    {
        public EnemyRanged(Character character, Enemy enemy, Transform transform, bool implementDodge) :
            base(character, enemy, transform, implementDodge)
        {
            attackRange = 250 * enemy.Level;
            targetingRange = 500 * enemy.Level;
        }

        public override void Attack()
        {
            GameObject projectile = new GameObject(transform.Position);
            projectile.AddComponent(new Projectile(projectile, enemy));
            GameWorld.CurrentScene.AddObject(projectile);
        }
    }
}