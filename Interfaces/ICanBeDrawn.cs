using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Battle_Mages
{
    public interface ICanBeDrawn
    {
        /// <summary>
        /// Method to draw a texture
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(Drawer drawer);
    }
}
