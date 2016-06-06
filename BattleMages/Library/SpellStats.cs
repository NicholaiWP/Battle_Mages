using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public struct SpellStats
    {
        public float Damage { get; set; }
        public float ManaCost { get; set; }
        public float CooldownTime { get; set; }
        public float Range { get; set; }
        // public float EffectTime { get; set; }
    }
}