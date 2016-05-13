using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class RuneInfo
    {
        public delegate void RuneAction(Spell targetSpell);

        private RuneAction action;

        public string Name { get; }
        public string Description { get; }

        public RuneInfo(string name, string description, RuneAction action)
        {
            Name = name;
            Description = description;
            this.action = action;
        }

        public void ApplyChanges(Spell targetSpell)
        {
            action(targetSpell);
        }
    }
}