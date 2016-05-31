using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Hunt : IBehaviour
    {
        private Transform transform;
        private Enemy enemy;
        private Character character;
        private float attackRange;
        private float targetingRange;

        public Hunt(Enemy enemy, float attackRange, float targetingRange)
        {
            this.enemy = enemy;
            this.attackRange = attackRange;
            this.targetingRange = targetingRange;
            transform = enemy.GameObject.Transform;
            character = enemy.GameObject.GetComponent<Character>();
        }

        public void ExecuteBehaviour()
        {
            foreach (GameObject potentialTarget in GameWorld.Scene.ActiveObjects)
            {
                if (potentialTarget.GetComponent<Player>() != null)
                {
                    Vector2 vecToTarget = Vector2.Subtract(transform.Position, potentialTarget.Transform.Position);
                    float lengthToTarget = vecToTarget.Length();

                    if (lengthToTarget <= targetingRange && !InAttackRange(lengthToTarget))
                    {
                        Vector2 movement = Vector2.Zero;
                        if (transform.Position.Y - 1 > potentialTarget.Transform.Position.Y + 1)
                            movement.Y -= 1;
                        if (transform.Position.Y + 1 < potentialTarget.Transform.Position.Y - 1)
                            movement.Y += 1;
                        if (transform.Position.X - 1 > potentialTarget.Transform.Position.X + 1)
                        {
                            movement.X -= 1;
                            if (enemy.GameObject.GetComponent<Character>() != null)
                                enemy.GameObject.GetComponent<Character>().FDirection = FacingDirection.Left;
                        }
                        if (transform.Position.X + 1 < potentialTarget.Transform.Position.X - 1)
                        {
                            movement.X += 1;
                            if (enemy.GameObject.GetComponent<Character>() != null)
                                enemy.GameObject.GetComponent<Character>().FDirection = FacingDirection.Right;
                        }
                        character.MoveDirection = movement;
                    }
                    break;
                }
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