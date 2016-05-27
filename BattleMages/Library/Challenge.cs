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

            allWaves.Add("Normal", new List<Wave> { new Wave(new List<Vector2> { new Vector2(25,30),
                new Vector2(-20, -30), new Vector2(120, 90)},
               new List<Enemy> { new Golem(), new Orb(), new Slime() }),
                new Wave(new List<Vector2> { new Vector2(300, 0),
                new Vector2(0, 300),
                    new Vector2(-300, 0), new Vector2(0, -300) },
                    new List<Enemy> {new Golem(), new Golem(), new Golem(), new Golem()}),
            new Wave(new List<Vector2> { new Vector2(200,0), new Vector2(200, 10), new Vector2(200, -10),
                new Vector2(200, 20), new Vector2(200,-20), new Vector2(210,0),
                new Vector2(190,0), new Vector2(220,0), new Vector2(180,0)},
            new List<Enemy> { new Orb(), new Orb(), new Orb(), new Orb(), new Orb(),
            new Orb(),new Orb(),new Orb(),new Orb() })});
        }

        public WaveController MakeWaveController(string challengeLevel)
        {
            List<Wave> waves = new List<Wave>(allWaves[challengeLevel]);
            return waveMaker(waves);
        }
    }
}