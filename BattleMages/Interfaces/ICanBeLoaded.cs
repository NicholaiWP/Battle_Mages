using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public interface ICanBeLoaded
    {
        /// <summary>
        /// Method to load starting content only called once
        /// </summary>
        /// <param name="content"></param>
        void LoadContent(ContentManager content);
    }
}