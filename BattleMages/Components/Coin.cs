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
        private Collider collider;
        private Animator animator;
        private Vector2 pos;
        private Vector2 velocity;
        private float height = 1;
        private float fallSpeed;

        public Coin()
        {
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void Initialize(InitializeMsg msg)
        {
            animator = GameObject.GetComponent<Animator>();
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            collider = GameObject.GetComponent<Collider>();

            pos = GameObject.Transform.Position;

            fallSpeed = (float)GameWorld.Random.NextDouble() * 40 - 100;
            velocity = new Vector2((float)GameWorld.Random.NextDouble() - 0.5f, (float)GameWorld.Random.NextDouble() - 0.5f) * 120f;
        }

        private void Update(UpdateMsg msg)
        {
            if (height > 0f)
            {
                fallSpeed += GameWorld.DeltaTime * 400f;

                height -= fallSpeed * GameWorld.DeltaTime;

                if (height <= 0)
                {
                    height = 0;
                    fallSpeed = 0;
                    velocity = Vector2.Zero;
                    GameWorld.SoundManager.PlaySound("CoinDropSound", false);
                }
            }

            pos += velocity * GameWorld.DeltaTime;

            GameObject.Transform.Position = pos + new Vector2(0, -height);

            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                Player player = other.GameObject.GetComponent<Player>();
                if (player != null)
                {
                    GameWorld.SoundManager.PlaySound("GetCoinSound", false);
                    player.Currency += value;
                    GameWorld.Scene.RemoveObject(GameObject);
                }
            }
        }
    }
}