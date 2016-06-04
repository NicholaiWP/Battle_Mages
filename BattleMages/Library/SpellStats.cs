using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public struct SpellStats
    {
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public float CooldownTime { get; set; }
        public float Range { get; set; }
    }
}