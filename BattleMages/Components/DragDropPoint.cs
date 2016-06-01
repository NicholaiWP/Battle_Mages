using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    internal class DragDropPoint : Component
    {
        private string tag;
        private Texture2D normalTex;
        private Texture2D highlightTex;
        private Texture2D hoverTex;
        private Action onDrop;
        private bool hovering;

        public string Tag { get { return tag; } }

        public bool Highlighted { get; set; }
        public bool Hovering { get { return hovering; } }

        public DragDropPoint(string tag, Texture2D normalTex, Texture2D highlightTex, Texture2D hoverTex, Action onDrop)
        {
            this.tag = tag;
            this.normalTex = normalTex;
            this.highlightTex = highlightTex;
            this.hoverTex = hoverTex;
            this.onDrop = onDrop;

            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Update(UpdateMsg msg)
        {
            hovering = false;
            if (Highlighted)
            {
                Rectangle rectangle = new Rectangle((GameObject.Transform.Position - Utils.HalfTexSize(normalTex)).ToPoint(), normalTex.Bounds.Size);
                if (rectangle.Contains(GameWorld.Cursor.Position))
                {
                    hovering = true;
                }
            }
        }

        private void Draw(DrawMsg msg)
        {
            Texture2D texToUse = normalTex;
            if (Highlighted)
            {
                texToUse = highlightTex;
                if (hovering)
                {
                    texToUse = hoverTex;
                }
            }
            msg.Drawer[DrawLayer.UI].Draw(texToUse, position: GameObject.Transform.Position - Utils.HalfTexSize(texToUse));
        }

        public void Activate()
        {
            onDrop?.Invoke();
        }
    }
}