using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class EarthSpikes : Spell, ICanBeDrawn, ICanUpdate
    {
        private Collider collider;
        private Texture2D sprite;
        private Vector2 position;

        public EarthSpikes(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Damage = 8;
            CooldownTime = 2;
            ApplyRunes();
            position = p.AimTarget;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/ice");
            collider = new Collider(GameObject, new Vector2(8, 8));
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Draw(Drawer drawer)
        {
            throw new NotImplementedException();
        }
    }
}