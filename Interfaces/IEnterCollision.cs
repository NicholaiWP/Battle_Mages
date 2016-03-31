using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public interface IEnterCollision
    {
        /// <summary>
        /// Method for when collisionboxes enter collision with eachother
        /// </summary>
        /// <param name="other"></param>
        void OnCollisionEnter(Collider other);
    }
}