using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battle_Mages
{
    internal enum DrawLayer
    {
        Background = -1,
        Gameplay = 0,
        Foreground = 1,
        UI = 2,
        AboveUI = 3
    }

    /// <summary>
    /// A "drawer" of SpriteBatches to "draw" to
    /// </summary>
    internal class Drawer
    {
        private Dictionary<DrawLayer, SpriteBatch> layers = new Dictionary<DrawLayer, SpriteBatch>();
        private GraphicsDevice graphics;
        private Matrix matrix;

        public Drawer(GraphicsDevice graphics, Matrix matrix)
        {
            this.graphics = graphics;
            this.matrix = matrix;
        }

        public SpriteBatch this[DrawLayer layer]
        {
            get
            {
                if (!layers.ContainsKey(layer))
                {
                    SpriteBatch batch = new SpriteBatch(graphics);
                    batch.Begin(SpriteSortMode.Deferred, transformMatrix: matrix);
                    layers.Add(layer, new SpriteBatch(graphics));
                }
                return layers[layer];
            }
        }

        public void End()
        {
            foreach (SpriteBatch batch in layers.Values)
            {
                batch.End();
            }
        }
    }
}
