using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Challenge
    {
        private List<Wave> waves;

        public Challenge(List<Wave> waves)
        {
            this.waves = waves;
        }

        public WaveController MakeWaveController()
        {
            return new WaveController(waves);
        }
    }
}