using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Storm : Spell
    {
        private float radius;
        private float existenceTimer;
        private float lightningTimer;

        public Storm(GameObject go, SpellCreationParams p) : base(go, p)
        {
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Update(UpdateMsg message)
        {
            if (existenceTimer <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
            else
            {
                existenceTimer -= GameWorld.DeltaTime;
            }

            if (lightningTimer <= 0)
            {
                lightningTimer = 1;
                Vector2 center = Vector2.Zero;
                foreach (GameObject go in GameWorld.Scene.ActiveObjects)
                {
                    if (go.GetComponent<Player>() != null)
                    {
                        center += go.Transform.Position;
                    }
                }
            }
        }

        private void Draw(DrawMsg message)
        {
            throw new NotImplementedException();
        }
    }
}