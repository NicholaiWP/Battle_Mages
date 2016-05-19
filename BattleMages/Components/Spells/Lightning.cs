using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    class Lightning : Spell, ICanBeDrawn, ICanBeAnimated, ICanUpdate
    {
        private Texture2D sprite;
        private Vector2 velocity;
        private Vector2 LightningPos;

        public Lightning(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Damage = 25;
            CooldownTime = 0.8f;
            ApplyRunes();
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Spell Images/Lightning_bigger");

            //LightningPos = p.AimTarget - GameObject.Transform.Position;
            //LightningPos.Normalize();
            
            // else if (canUseSpells && isFromSky == true && mState.LeftButton == ButtonState.Pressed && spellCooldownTimer <= 0)
            //{
            //    GameObject spellLightning = new GameObject(transform.Position);
            //    Spell sp = baseSpell.CreateSpell(spellLightning, new SpellCreationParams(runes,
            //    GameWorld.Camera.Position, lightningPosition));
            //    spellLightning.AddComponent(s);
            //    GameWorld.CurrentScene.AddObject(spellLightning);
            //    spellCooldownTimer = s.CooldownTime;
            //}

            //sets the speed of the spell
            velocity = LightningPos * 400f;
        }

        public void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Gameplay].Draw(sprite, GameObject.Transform.Position, Color.White);
        }

        public void OnAnimationDone(string animationsName)
        {
            
        }

        public void Update()
        {
            GameObject.Transform.Position += velocity * GameWorld.DeltaTime;
        }
    }
}
