using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    class SpellBookUI : Component, ICanBeDrawn, ICanBeLoaded
    {
        private Texture2D background;
        private Texture2D spellCircle;
        private Texture2D runeDscWindow;
        private Texture2D rune;
        

        public SpellBookUI(GameObject gameObject) : base(gameObject)
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            //place foreach loop for runes here
            //place foreach loop for spells here 
            
            background = content.Load<Texture2D>("images/BMspellBookbg");

        }
        public void Draw (Drawer drawer)
        {

            drawer[DrawLayer.UI].Draw(background, position: GameWorld.Camera.Position);

        }
    }
}
