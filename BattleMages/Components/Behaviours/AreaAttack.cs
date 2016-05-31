using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class AreaAttack : IBehaviour
    {
        private enum AtkState
        {
            Ready,
            Beginning,
            Attacking,
            Recharging,
        }

        private Enemy enemy;

        //Timing
        private readonly float attackDelay;

        private readonly float attackTime;
        private readonly float rechargeTime;

        //Range to be inside for attack to start
        private readonly float maxRange;

        //Size of area to damage in
        private float damageRange;

        //Damage to give
        private int damage;

        //Current state of attack
        private AtkState state;

        private float timer;

        private Character character;
        private float oldMoveSpeed;

        public AreaAttack(Enemy enemy, float maxRange, float attackDelay, float attackTime, float rechargeTime, float damageRange, int damage)
        {
            this.enemy = enemy;
            this.maxRange = maxRange;
            this.attackDelay = attackDelay;
            this.attackTime = attackTime;
            this.rechargeTime = rechargeTime;
            this.damageRange = damageRange;
            this.damage = damage;
            character = enemy.GameObject.GetComponent<Character>();
        }

        public void ExecuteBehaviour()
        {
            if (state == AtkState.Ready)
            {
                Player player;
                if (PlayerInRange(maxRange, out player))
                {
                    state = AtkState.Beginning;
                    timer = attackDelay;
                    oldMoveSpeed = character.MoveSpeed;
                    character.MoveSpeed = 0;
                }
            }

            if (timer > 0)
            {
                timer -= GameWorld.DeltaTime;
                if (timer <= 0)
                {
                    switch (state)
                    {
                        case AtkState.Beginning:
                            //ATTACK!
                            Player player;
                            if (PlayerInRange(damageRange, out player))
                            {
                                player.TakeDamage(damage);
                            }
                            Random randy = new Random();
                            for (int i = 0; i < 16; i++)
                            {
                                GameObject dustGo = new GameObject(enemy.GameObject.Transform.Position + new Vector2((float)randy.NextDouble() - 0.5f, (float)randy.NextDouble() - 0.5f) * damageRange);
                                dustGo.AddComponent(new SpriteRenderer("Textures/Misc/Dust"));
                                dustGo.AddComponent(new FadeOut(1));
                                GameWorld.Scene.AddObject(dustGo);
                            }
                            state = AtkState.Attacking;
                            timer = attackTime;
                            break;

                        case AtkState.Attacking:
                            state = AtkState.Recharging;
                            timer = rechargeTime;
                            character.MoveSpeed = oldMoveSpeed;
                            break;

                        case AtkState.Recharging:
                            state = AtkState.Ready;
                            break;
                    }
                }
            }
        }

        private bool PlayerInRange(float range, out Player player)
        {
            player = GameWorld.Scene.ActiveObjects.Select(a => a.GetComponent<Player>()).Where(a => a != null).FirstOrDefault();
            if (player != null)
            {
                float dist = Vector2.Distance(enemy.GameObject.Transform.Position, player.GameObject.Transform.Position);
                if (dist < range)
                {
                    return true;
                }
            }
            return false;
        }
    }
}