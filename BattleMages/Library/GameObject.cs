using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public sealed class GameObject
    {
        private List<Component> components = new List<Component>();
        private List<Component> componentsToAdd = new List<Component>();
        private List<Component> componentsToRemove = new List<Component>();

        //Transform component is cached in a property for easy access because all GameObjects have one
        public Transform Transform { get; private set; }

        /// <summary>
        /// Constructer of the gameobject
        /// </summary>
        /// <param name="position"></param>
        public GameObject(Vector2 position)
        {
            Transform = new Transform(this, position);
            AddComponent(Transform);
        }

        /// <summary>
        /// Method for adding components to the list
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(Component component)
        {
            if (!components.Any(a => a.GetType() == component.GetType()))
            {
                componentsToAdd.Add(component);
            }
            else
            {
                throw new InvalidOperationException("The game object already contains a component of this type.");
            }
        }

        /// <summary>
        /// Method for removing a component from the list
        /// </summary>
        /// <param name="componentName"></param>
        public void RemoveComponent<T>() where T : Component
        {
            componentsToRemove.Add(GetComponent<T>());
        }

        /// <summary>
        /// Method for getting a specific component by its type
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            return components.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Forwards a message to all components on this game object.
        /// </summary>
        /// <typeparam name="T">Type of message (No need to type this out, it is inferred from the 'message' parameter)</typeparam>
        /// <param name="message">Message to forward</param>
        public void SendMessage<T>(T message) where T : Msg
        {
            foreach (Component comp in components)
                comp.SendMessage(message);

            if (message is UpdateMsg)
            {
                foreach (Component comp in componentsToAdd)
                    components.Add(comp);

                foreach (Component comp in componentsToAdd)
                    comp.SendMessage(new InitializeMsg());

                foreach (Component comp in componentsToRemove)
                    components.Remove(comp);

                componentsToAdd.Clear();
                componentsToRemove.Clear();
            }
        }
    }
}