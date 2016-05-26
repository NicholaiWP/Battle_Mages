using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Wave
    {
        public List<Vector2> positions = new List<Vector2>();
        public List<Enemy> enemies = new List<Enemy>();

        public Wave(List<Vector2> positions, List<Enemy> enemies)
        {
            this.positions = positions;
            this.enemies = enemies;
        }
    }
}