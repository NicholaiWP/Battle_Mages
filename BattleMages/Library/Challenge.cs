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
        public delegate WaveController WaveMaker(List<Wave> waves);

        private Dictionary<string, List<Wave>> allWaves = new Dictionary<string, List<Wave>>();
        private WaveMaker waveMaker;

        public Challenge(WaveMaker waveMaker)
        {
            this.waveMaker = waveMaker;

            allWaves.Add("Normal", new List<Wave> { new Wave(new List<Vector2> { new Vector2(300, 0),
                new Vector2(0, 300),
                    new Vector2(-300, 0), new Vector2(0, -300) },
                    new List<Enemy> {new Golem(), new Golem(), new Golem(), new Golem()})});
        }

        public WaveController MakeWaveController(string challengeLevel)
        {
            List<Wave> waves = new List<Wave>(allWaves[challengeLevel]);
            return waveMaker(waves);
        }
    }
}