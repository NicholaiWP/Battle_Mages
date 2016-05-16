﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BattleMages
{
    public class GameObject
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
        /// Method for updating the gameobject, position etc.
        /// </summary>
        public virtual void Update()
        {
            foreach (Component comp in components)
            {
                if (comp is ICanUpdate)
                {
                    (comp as ICanUpdate).Update();
                }
            }

            foreach (Component comp in componentsToAdd)
                components.Add(comp);

            foreach (Component comp in componentsToAdd)
                if (comp is ICanBeLoaded)
                    (comp as ICanBeLoaded).LoadContent(GameWorld.Instance.Content);

            foreach (Component comp in componentsToRemove)
                components.Remove(comp);

            componentsToAdd.Clear();
            componentsToRemove.Clear();
        }

        /// <summary>
        /// Method for drawing the gameobject on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(Drawer drawer)
        {
            foreach (Component comp in components)
            {
                if (comp is ICanBeDrawn)
                {
                    (comp as ICanBeDrawn).Draw(drawer);
                }
            }
        }

        public void OnAnimationDone(string animationName)
        {
            foreach (Component component in components)
            {
                //Checks if any components are ICanBeAnimated
                if (component is ICanBeAnimated)
                {
                    //If a component is ICanBeAnimated we call the local implementation of the method
                    (component as ICanBeAnimated).OnAnimationDone(animationName);
                }
            }
        }

        /// <summary>
        /// Method when objects collide
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionEnter(Collider other)
        {
            foreach (Component comp in components)
            {
                if (comp is IEnterCollision)
                {
                    (comp as IEnterCollision).OnCollisionEnter(other);
                }
            }
        }

        /// <summary>
        /// The method for when the objec´s collides with something repeadetly
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionStay(Collider other)
        {
            foreach (Component comp in components)
            {
                if (comp is IStayOnCollision)
                {
                    (comp as IStayOnCollision).OnCollisionStay(other);
                }
            }
        }

        /// <summary>
        /// When objects doesnt collide
        /// </summary>
        /// <param name="other"></param>
        public void OnCollisionExit(Collider other)
        {
            foreach (Component comp in components)
            {
                if (comp is IExitCollision)
                {
                    (comp as IExitCollision).OnCollisionExit(other);
                }
            }
        }

        public void OnDestroy()
        {
            foreach (Component comp in components)
            {
                comp.OnDestroy();
            }
        }
    }
}