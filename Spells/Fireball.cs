using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    internal class Fireball : Spell
    {
        public Fireball(Rune[] runes) : base(runes)
        {
            Damage = 120120301;
            ApplyRunes();
        }
    }
}