using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleMages
{
    internal class Lightning : Spell, ICanBeDrawn, ICanBeAnimated, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Vector2 LightningPos;
        private Collider collider;

        public Lightning(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Damage = 40;
            CooldownTime = 0.9f;
            ApplyRunes();
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/Lightning_bigger");

            LightningPos = p.AimTarget - GameObject.Transform.Position;

            //sets the speed of the spell
            velocity = LightningPos * 400f;

            //adds a collider
            collider = new Collider(GameObject, new Vector2(8, 8));
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position - new Vector2(0, sprite.Height), Color.White);
        }

        public void OnAnimationDone(string animationsName)
        {
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;

            //Finds Enemy component  and if it's not null, deal damage and apply floating damage text
            //to the enemy when the object's collision position is found.
            foreach (var other in collider.GetCollisionsAtPosition(GameObject.Transform.Position))
            {
                var enemy = other.GameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DealDamage(Damage);
                    GameWorld.CurrentScene.AddObject(ObjectBuilder.BuildFlyingLabelText(GameObject.Transform.Position, Damage.ToString()));
                    GameWorld.CurrentScene.RemoveObject(GameObject);
                }
            }
            if (!Utils.InsideCircle(GameObject.Transform.Position, 320))
            {
                GameWorld.CurrentScene.RemoveObject(GameObject);
            }
        }
    }
}