using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public interface ICanUpdate
    {
        /// <summary>
        /// Method for updating, things such as position
        /// </summary>
        void Update();
    }
}