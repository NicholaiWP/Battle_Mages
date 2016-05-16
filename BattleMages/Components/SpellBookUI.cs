using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages.Components
{
    class SpellBookUI : Component, ICanBeDrawn, ICanBeLoaded
    {
        private Texture2D background;

        public SpellBookUI(GameObject gameObject) : base(gameObject)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
             
        }
        public void Draw (Drawer drawer)
        {

        }
    }
}
