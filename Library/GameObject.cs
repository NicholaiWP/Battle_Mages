using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battle_Mages
{
    public class GameObject
    {
        //Fields
        private bool isLoaded;

        //Lists
        private List<Component> components = new List<Component>();

        //Properteis
        public Transform Transform { get; set; }

        /// <summary>
        /// Constructer of the gameobject
        /// </summary>
        /// <param name="position"></param>
        public GameObject(Vector2 position)
        {
            Transform = new Transform(this, position);
            components.Add(Transform);
            isLoaded = false;
        }

        /// <summary>
        /// Method for adding components to the list
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        /// <summary>
        /// Method for removing a component from the list
        /// </summary>
        /// <param name="componentName"></param>
        public void RemoveComponent<T>() where T : Component
        {
            components.Remove(GetComponent<T>());
        }

        /// <summary>
        /// Method for getting a specific component by its name
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            return components.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Method to load starting information only components only load once
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            if (!isLoaded)
            {
                foreach (Component comp in components)
                {
                    if (comp is ICanBeLoaded)
                    {
                        (comp as ICanBeLoaded).LoadContent(content);
                    }
                }
                isLoaded = true;
            }
        }

        /// <summary>
        /// Method for updating the gameobject, position etc.
        /// </summary>
        public void Update()
        {
            foreach (Component comp in components)
            {
                if (comp is ICanUpdate)
                {
                    (comp as ICanUpdate).Update();
                }
            }
        }

        /// <summary>
        /// Method for drawing the gameobject on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(Drawer drawer)
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
    }
}
