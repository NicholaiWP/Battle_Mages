using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    /// <summary>
    /// This message is sent to a game object if it has an Animator attached and its animation is done playing.
    /// </summary>
    public class AnimationDoneMsg : Msg
    {
        /// <summary>
        /// Name of animation that's done playing
        /// </summary>
        public string AnimationName { get; }

        public AnimationDoneMsg(string animationName)
        {
            AnimationName = animationName;
        }
    }
}
