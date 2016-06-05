using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class Challenge
    {
        private List<Wave> waves;
        private int baseRuneToUnlock;

        public Challenge(int baseRuneToUnlock, List<Wave> waves)
        {
            this.baseRuneToUnlock = baseRuneToUnlock;
            this.waves = waves;
        }

        public WaveController MakeWaveController()
        {
            return new WaveController(waves, baseRuneToUnlock);
        }
    }
}