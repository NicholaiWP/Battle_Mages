using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class Boss : Enemy
    {

        public Boss()
        {
            //Attributes
            moneyAmount = 500;
            MaxHealth = 1500;
            damage = 30;
            cooldownTimer = 2f;
            attackSpeed = 0;
            targetingRange = 800;
            attackRange = 350;          
        }

        protected override void PreInitialize(PreInitializeMsg msg)
        {
            base.PreInitialize(msg);
            //GameObject.AddComponent(new SpriteRenderer("Textures/Enemies/golemSpriteSheet", true)
            //{ Rectangle = new Rectangle(0, 0, 32, 32) });
        }

        protected override void Initialize(InitializeMsg msg)
        {
            base.Initialize(msg);
            MoveSpeed = 50;
            MoveAccel = 60;
            //might need some new behaviours
            behaviours.Add(new Hunt(this, attackRange, targetingRange));
            behaviours.Add(new Attack(this, attackRange, targetingRange));
            behaviours.Add(new Dodging(this, 100));

            //Add animaton below
        }

        protected override void Update(UpdateMsg msg)
        {
            base.Update(msg);

            foreach (GameObject go in GameWorld.Scene.ActiveObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    if (go.Transform.Position.X > GameObject.Transform.Position.X)
                    {
                        character.FDirection = FacingDirection.Right;

                    }
                    else if (go.Transform.Position.X < GameObject.Transform.Position.X)
                    {
                        character.FDirection = FacingDirection.Left;

                    }
                }
            }
        }
    }
}
