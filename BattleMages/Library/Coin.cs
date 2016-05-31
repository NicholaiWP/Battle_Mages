using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages.Library
{
    public abstract class Coin : Component
    {
        private int value;
        private Texture2D coinSpr;
        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Collider collider;

        public int Value { get; set; }

        public Coin()
        {
            value = 10;

            spriteRenderer = new SpriteRenderer("Images/Coin");
            collider = new Collider(new Vector2(8, 8));
        }

        protected virtual void Initialize(InitializeMsg msg)
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            transform = GameObject.Transform;
            collider = GameObject.GetComponent<Collider>();
        }

        public void Update()
        {
            //Collision check with player
            //collider.CheckCollision(Player)
        }
    }
}
