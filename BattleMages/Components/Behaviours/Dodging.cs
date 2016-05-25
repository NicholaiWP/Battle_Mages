using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class Dodging : IBehaviour
    {
        private Enemy enemy;
        private Collider collider;
        private float currentLengthToSpell;
        private float previousLengthToSpell;

        public Dodging(Enemy enemy)
        {
            this.enemy = enemy;
            collider = enemy.GameObject.GetComponent<Collider>();
        }

        public void ExecuteBehaviour()
        {
            foreach (Collider col in GetCollidersInScene())
            {
                if (col.GameObject.GetComponent<Spell>() != null)
                {
                }
            }
            previousLengthToSpell = currentLengthToSpell;
            currentLengthToSpell = 0;
        }

        private IEnumerable<Collider> GetCollidersInScene()
        {
            return GameWorld.CurrentScene.ActiveObjects.Select(a => a.GetComponent<Collider>()).Where(a => a != null);
        }
    }
}