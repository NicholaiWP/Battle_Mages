using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Projectile : Component, IEnterCollision
    {
        public Projectile(GameObject gameObject) : base(gameObject)
        {
        }

        public void OnCollisionEnter(Collider other)
        {
        }
    }
}