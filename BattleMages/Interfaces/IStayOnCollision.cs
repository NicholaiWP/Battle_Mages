using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public interface IStayOnCollision
    {
        /// <summary>
        /// Method for when colliders collide continuesly
        /// </summary>
        /// <param name="other"></param>
        void OnCollisionStay(Collider other);
    }
}