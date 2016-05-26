using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Challenges
    {
        private string name;
        public string Name { get { return name; } }

        public Challenges(string name)
        {
            this.name = name;
        }
    }
}