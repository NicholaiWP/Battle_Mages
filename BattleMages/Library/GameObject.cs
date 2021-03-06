﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
        /// Constructor of the gameobject
        /// </summary>
        /// <param name="position"></param>
        public GameObject(Vector2 position)
        {
            Transform = new Transform(position);
            AddComponent(Transform);
        }

        /// <summary>
        /// Method for adding components to the list
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(Component component)
        {
            if (!components.Any(a => a.GetType() == component.GetType()) &&
                !componentsToAdd.Any(a => a.GetType() == component.GetType()))
            {
                componentsToAdd.Add(component);
                component.GameObject = this;
                component.SendMessage(new PreInitializeMsg());
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
        }

        public void ProcessComponents()
        {
            //Add components to be added
            //Step 1: Add to component list
            foreach (Component comp in componentsToAdd)
                components.Add(comp);

            List<Component> componentsToInitialize = new List<Component>(componentsToAdd);
            //Step 2: Initialize
            foreach (Component comp in componentsToInitialize)
                comp.SendMessage(new InitializeMsg());

            //Remove components to be removed
            foreach (Component comp in componentsToRemove)
                components.Remove(comp);

            componentsToAdd.Clear();
            componentsToRemove.Clear();
        }
    }
}