using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    internal class Fireball : Spell
    {
        public Fireball(GameObject go, RuneInfo[] runes) : base(go, runes)
        {
            Damage = 120120301;
            ApplyRunes();
        }
    }
}