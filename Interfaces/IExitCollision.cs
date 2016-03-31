using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public interface IExitCollision
    {
        /// <summary>
        /// Method for when colliders doesnt collide
        /// </summary>
        /// <param name="other"></param>
        void OnCollisionExit(Collider other);
    }
}