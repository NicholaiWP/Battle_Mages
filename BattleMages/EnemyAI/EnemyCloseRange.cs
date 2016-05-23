using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class EnemyCloseRange : EnemyAI
    {
        public EnemyCloseRange(Character character, Enemy enemy, Transform transform, bool implementDodge) :
            base(character, enemy, transform, implementDodge)
        {
            attackRange = 30;
            targetingRange = 500 * enemy.Level;
        }

        public override void Attack()
        {
            foreach (GameObject player in GameWorld.CurrentScene.ActiveObjects)
            {
                if (player.GetComponent<Player>() != null)
                {
                    player.GetComponent<Player>().DealDamage(enemy.Damage);
                    enemy.IsAttacking = true;
                    break;
                }
                else
                {
                    enemy.IsAttacking = false;
                }
            }
        }

        public override void Targeting()
        {
            foreach (GameObject potentialTarget in GameWorld.CurrentScene.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();
                    if (lengthToTarget <= targetingRange && lengthToTarget >= attackRange)
                    {
                        character.Up = transform.Position.Y - 10 > potentialTarget.Transform.Position.Y + 10;
                        character.Down = transform.Position.Y + 10 < potentialTarget.Transform.Position.Y - 10;
                        character.Left = transform.Position.X - 10 > potentialTarget.Transform.Position.X + 10;
                        character.Right = transform.Position.X + 10 < potentialTarget.Transform.Position.X - 10;
                    }
                    break;
                }
            }
        }
    }
}