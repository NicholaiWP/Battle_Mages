using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public interface ICanBeAnimated
    {
        /// <summary>
        /// Method for when an animation ends
        /// </summary>
        /// <param name="animationsName"></param>
        void OnAnimationDone(string animationsName);
    }
}