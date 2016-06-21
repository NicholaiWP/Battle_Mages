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
        public delegate List<Enemy> EnemyListDelegate();

        private EnemyListDelegate enemyListCreater;
        public List<Vector2> positions = new List<Vector2>();

        public List<Enemy> Enemies
        {
            get
            {
                return enemyListCreater?.Invoke();
            }
        }

        public Wave(List<Vector2> positions, EnemyListDelegate enemyListCreator)
        {
            this.positions = positions;
            this.enemyListCreater = enemyListCreator;
        }
    }
}