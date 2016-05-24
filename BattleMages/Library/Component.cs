using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Component
    {
        private GameObject gameObject;

        private Dictionary<Type, object> handlers = new Dictionary<Type, object>();

        public GameObject GameObject
        {
            get
            {
                return gameObject;
            }
        }

        /// <summary>
        /// Constructor for the component with a gameObject
        /// </summary>
        /// <param name="gameObject"></param>
        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        /// <summary>
        /// Forwards this message to any handlers on this component.
        /// </summary>
        /// <typeparam name="T">Type of message (No need to type this out, it is inferred from the 'message' parameter)</typeparam>
        /// <param name="message">Message to forward</param>
        public void SendMessage<T>(T message) where T : Msg
        {
            object result;
            if (handlers.TryGetValue(message.GetType(),out result))
            {
                MsgHandler<T> castedResult = (MsgHandler<T>)result;
                castedResult(message);
            }
        }

        /// <summary>
        /// Adds a message listener for a specific type of message. Any messages of this type will be forwarded to the supplied delegate.
        /// </summary>
        /// <typeparam name="T">Type of message to listen for</typeparam>
        /// <param name="handler">Delegate to be called when a message of this type is recieved</param>
        protected void Listen<T>(MsgHandler<T> handler) where T : Msg
        {
            handlers.Add(typeof(T), handler);
        }
    }
}