using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{

    class Burn : Component
    {
        private int burnDmg;
        private float burnTimer;
        private Enemy enemy;
        private float burnDuration;
        private Animator animator;

        public Burn(Enemy enemy, int burnDmg)
        {
            this.enemy = enemy;
            this.burnDmg = burnDmg;
            burnDuration = 6;
            burnTimer = 0;
            Listen<PreInitializeMsg>(PreInitialize);
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }


        private void PreInitialize(PreInitializeMsg preMsg)
        {

            GameObject.AddComponent(new SpriteRenderer("Textures/Misc/burnEffect-Sheet", true)
            { Rectangle = new Rectangle(0, 0, 32, 32) });
            GameObject.AddComponent(new Animator());
        }

        private void Initialize(InitializeMsg initMsg)
        {
          
            animator = GameObject.GetComponent<Animator>();

            //animation
            animator.CreateAnimation("burn", new Animation(priority: 0, framesCount: 10, yPos: 0, xStartFrame: 0, width: 32,
            height: 32, fps: 20, offset: Vector2.Zero));
          
        }



        private void Update(UpdateMsg msg)
        {          
            animator.PlayAnimation("burn");
            GameObject.Transform.Position = enemy.GameObject.Transform.Position;
            if(burnDuration > 0 && burnTimer <= 0)
            {
                burnTimer = 0.5f;
                if(enemy.Health < 0)
                {
                    GameWorld.Scene.RemoveObject(GameObject);
                }
                enemy.TakeDamage(burnDmg);
            }
            else if(burnDuration > 0 && burnTimer > 0)
            {
                burnTimer -= GameWorld.DeltaTime;
            }
            else
            {
                enemy.IsAlreadyBurned = false;
                GameWorld.Scene.RemoveObject(GameObject);
            }
            burnDuration -= GameWorld.DeltaTime;
        }
    }
}
