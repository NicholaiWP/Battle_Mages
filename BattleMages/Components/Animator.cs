﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class Animator : Component
    {
        //Fields
        private float timeElapsed;

        private int currentIndex;
        private string animationName;
        private float fps;

        //Components
        private SpriteRenderer spriteRenderer;

        //Dictionary
        private Dictionary<string, Animation> animations;

        //Array
        private Rectangle[] frames;

        /// <summary>
        /// The animator´s constructor
        /// </summary>
        /// <param name="gameObject"></param>
        public Animator()
        {
            animations = new Dictionary<string, Animation>();
            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
        }

        private void Initialize(InitializeMsg message)
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Creating the different animations
        /// </summary>
        /// <param name="name"></param>
        /// <param name="animation"></param>
        public void CreateAnimation(string name, Animation animation)
        {
            animations.Add(name, animation);
        }

        /// <summary>
        /// Updating animations
        /// </summary>
        private void Update(UpdateMsg msg)
        {
            /*timeElapsed += GameWorld.GetInstance.GetDeltaTime;

            currentIndex = (int)(timeElapsed * animations[animationName].GetFps);

            if (currentIndex >= frames.Length)
            {
                GetGameObject.OnAnimationDone(animationName);
                timeElapsed = 0;
                currentIndex = 0;
            }

            spriteRenderer.Rectangle = frames[currentIndex];
            */
        }

        /// <summary>
        /// This method is for playing an animation by its name
        /// </summary>
        /// <param name="animationName"></param>
        public void PlayAnimation(string animationName)
        {
            //Checks if it’s a new animation
            if (this.animationName != animationName)
            {
                //Sets the rectangles
                this.frames = animations[animationName].GetFrames;
                //Resets the rectangle
                this.spriteRenderer.Rectangle = frames[0];
                //Sets the offset
                this.spriteRenderer.Offset = animations[animationName].GetOffset;
                //Sets the animation name
                this.animationName = animationName;
                //Sets the fps
                this.fps = animations[animationName].GetFps;
                //Resets the animation
                timeElapsed = 0;
                currentIndex = 0;
            }
        }
    }
}