using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    /// <summary>
    /// Stores information about a rune that can be applied to a base spell to alter its stats.
    /// </summary>
    public class AttributeRune
    {
        public delegate void RuneAction(Spell targetSpell);
        private RuneAction action;

        public string Name { get; }
        public string Description { get; }
        public string TextureName { get; }
        public int RuneCost { get; set; }
        public int CurrentMoney { get; set; } = 1000;
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Creates a new attribute rune with a name, a description, and a delegate method to call when applying the rune to a spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="action"></param>
        public AttributeRune(string name, string description, string textureName, RuneAction action)
        {
            Name = name;
            Description = description;
            TextureName = textureName;
            this.action = action;
        }

        /// <summary>
        /// Apples the stat changes associated with this rune to the target spell.
        /// </summary>
        /// <param name="targetSpell"></param>
        public void ApplyChanges(Spell targetSpell)
        {
            action(targetSpell);
        }

        public void LoadContent()
        {
            Texture = GameWorld.Load<Texture2D>("Rune Images/" + TextureName);
        }
    }
}