using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class Dodging : IBehaviour
    {
        private Enemy enemy;
        private Collider collider;
        private Character character;
        private float currentLengthToSpell;
        private float previousLengthToSpell;

        Collider tracking;
        Vector2 latestTrackedPos;
        Vector2 dodgeDir;

        public Dodging(Enemy enemy)
        {
            this.enemy = enemy;
            collider = enemy.GameObject.GetComponent<Collider>();
            character = enemy.GameObject.GetComponent<Character>();
        }

        public void ExecuteBehaviour()
        {
            IEnumerable<Collider> collidersInScene = GameWorld.CurrentScene.ActiveObjects //Find all game objects
                .Where(a => a.GetComponent<Spell>() != null) //That have a spell component
                .Select(a => a.GetComponent<Collider>()) //Get their colliders
                .Where(a => a != null); //Filter out those who don't have any

            //Get the closest one
            Collider closestCollider = collidersInScene.OrderBy(a => (a.GameObject.Transform.Position - enemy.GameObject.Transform.Position).Length()).FirstOrDefault();

            if (closestCollider != null)
            {
                if (tracking == closestCollider)
                {
                    float currentDist = (enemy.GameObject.Transform.Position - closestCollider.GameObject.Transform.Position).Length();
                    float lastDist = (enemy.GameObject.Transform.Position - latestTrackedPos).Length();

                    if (currentDist <= lastDist && currentDist < 128)
                    {
                        character.MoveDirection = dodgeDir;
                    }
                }
                else
                {
                    tracking = closestCollider;
                    Random r = new Random();
                    dodgeDir = new Vector2((float)(r.NextDouble() - 0.5), (float)(r.NextDouble() - 0.5));
                    dodgeDir.Normalize();
                }
                latestTrackedPos = closestCollider.GameObject.Transform.Position;
            }
            previousLengthToSpell = currentLengthToSpell;
            currentLengthToSpell = 0;
        }
    }
}