using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class Coin : Component
    {
        private int value = 1;
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Collider collider;
        private Character character;
        private Animator animator;

        public Coin()
        {
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        protected virtual void PreInitialize(PreInitializeMsg msg)
        {
        }

        protected virtual void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();
            character = GameObject.GetComponent<Character>();
        }

        public void Spread()
        {
            character.Movement();
        }

        private void Update(UpdateMsg Update)
        {
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                Player player = other.GameObject.GetComponent<Player>();
                if(player != null)
                {
                    GameWorld.SoundManager.PlaySound("GetCoinSound", false);
                    player.Currency += value;
                    GameWorld.Scene.RemoveObject(GameObject);
                }

            }
        }
    }
}
