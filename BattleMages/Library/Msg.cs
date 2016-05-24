using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    /// <summary>
    /// Delegate to handle a specific type of message.
    /// </summary>
    /// <typeparam name="T">Type of message</typeparam>
    /// <param name="message">Message to handle</param>
    public delegate void MsgHandler<T>(T message) where T : Msg;

    /// <summary>
    /// Base class for all messages that can be sent to objects and components.
    /// </summary>
    public abstract class Msg {}
}
