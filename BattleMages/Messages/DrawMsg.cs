using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    /// <summary>
    /// This message is sent to all objects when they should be drawn to the screen. Use the 'Drawer' property.
    /// </summary>
    public class DrawMsg : Msg
    {
        /// <summary>
        /// Use this for drawing
        /// </summary>
        public Drawer Drawer { get; }

        public DrawMsg(Drawer drawer)
        {
            Drawer = drawer;
        }
    }
}
