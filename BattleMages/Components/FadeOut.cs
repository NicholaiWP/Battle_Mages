using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    class FadeOut : Component
    {
        readonly float time;
        float a = 1;

        SpriteRenderer spriteRenderer;

        public FadeOut(float time)
        {
            this.time = time;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void Initialize(InitializeMsg msg)
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
        }

        private void Update(UpdateMsg msg)
        {
            a -= GameWorld.DeltaTime / time;
            spriteRenderer.Opacity = a;
            if (a <= 0)
            {
                GameWorld.Scene.RemoveObject(GameObject);
            }
        }
    }
}
