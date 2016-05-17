using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    /// <summary>
    /// Stores information about a rune that can be applied to a base spell to alter its stats.
    /// </summary>
    public class RuneInfo
    {
        public delegate void RuneAction(Spell targetSpell);

        private RuneAction action;

        public string Name { get; }
        public string Description { get; }

        /// <summary>
        /// Creates a new instance of RuneInfo with a name, a description, and a delegate method to call when applying the rune to a spell.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="action"></param>
        public RuneInfo(string name, string description, RuneAction action)
        {
            Name = name;
            Description = description;
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
    }
}