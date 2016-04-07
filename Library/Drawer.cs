using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battle_Mages
{
    /// <summary>
    /// Represents a layer that can be drawn to. Layers are ordered by their constant value.
    /// </summary>
    public enum DrawLayer
    {
        //The layers will be ordered by the order they are declared in here
        Background,
        Gameplay,
        Foreground,
        UI,
        AboveUI
    }

    /// <summary>
    /// A "drawer" of SpriteBatches to "draw" to.
    /// </summary>
    public class Drawer
    {
        private Dictionary<DrawLayer, SpriteBatch> layers;
        private GraphicsDevice graphics;
        public Matrix Matrix { get; set; } = Matrix.Identity;

        public Drawer(GraphicsDevice graphics)
        {
            this.graphics = graphics;

            //Create a dictionary to hold layers and add all values of the DrawLayer enum
            layers = new Dictionary<DrawLayer, SpriteBatch>();
            foreach (DrawLayer layerId in Enum.GetValues(typeof(DrawLayer)))
                layers.Add(layerId, new SpriteBatch(graphics));
        }

        /// <summary>
        /// Gets the SpriteBatch associated with a layer. Use this to draw on the batch.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public SpriteBatch this[DrawLayer layer]
        {
            get
            {
                return layers[layer];
            }
        }

        public void BeginBatches()
        {
            foreach (SpriteBatch batch in layers.Values)
            {
                batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: Matrix);
            }
        }

        public void EndBatches()
        {
            //End all layers in a loop so they are drawn in order
            foreach (SpriteBatch batch in layers.Values)
            {
                batch.End();
            }
        }
    }
}
