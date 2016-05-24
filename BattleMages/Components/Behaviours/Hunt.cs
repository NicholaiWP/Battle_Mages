﻿using Microsoft.Xna.Framework;
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
        private int closeRange;
        private float attackTimer;

        public Hunt(Enemy enemy)
        {
            closeRange = 25;
            attackTimer = 0;
            this.enemy = enemy;
            transform = enemy.GameObject.Transform;
            character = enemy.GameObject.GetComponent<Character>();
        }

        public void ExecuteBehaviour(float attackRange, float targetingRange)
        {
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
                    break;
                }
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