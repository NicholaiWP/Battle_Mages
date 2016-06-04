using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleMages
{
    public class Animator : Component
    {
        //Fields
        private float timeElapsed;

        private int currentIndex;
        private string animationName;
        private float fps;
        private bool playing;
        private bool looping;

        //Components
        private SpriteRenderer spriteRenderer;

        //Dictionary
        private Dictionary<string, Animation> animations;

        //Array
        private Rectangle[] frames;

        public ReadOnlyDictionary<string, Animation> Animations
        {
            get { return new ReadOnlyDictionary<string, Animation>(animations); }
        }

        public string PlayingAnimationName { get { return animationName; } }
        public int CurrentIndex { get { return currentIndex; } }

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
            if (playing)
            {
                timeElapsed += GameWorld.DeltaTime;
                if (animationName != null)
                    currentIndex = (int)(timeElapsed * animations[animationName].Fps);
                if (frames != null)
                {
                    if (currentIndex >= frames.Length)
                    {
                        GameObject.SendMessage(new AnimationDoneMsg(animationName));
                        if (looping)
                        {
                            timeElapsed = 0;
                            currentIndex = 0;
                        }
                        else
                        {
                            playing = false;
                        }
                    }

                    if (currentIndex < frames.Length)
                        spriteRenderer.Rectangle = frames[currentIndex];
                }
            }
        }

        /// <summary>
        /// This method is for playing an animation by its name
        /// </summary>
        /// <param name="animationName"></param>
        public void PlayAnimation(string animationName, float rotationRadians = 0, bool looping = true)
        {
            this.looping = looping;
            this.playing = true;

            if (this.animationName == null || animations[this.animationName].Priority >= animations[animationName].Priority ||
                currentIndex >= frames.Length)
            {
                spriteRenderer.Rotation = rotationRadians;

                //Checks if it’s a new animation
                if (this.animationName != animationName)
                {
                    //Sets the rectangles
                    this.frames = animations[animationName].Frames;
                    //Resets the rectangle
                    this.spriteRenderer.Rectangle = frames[0];
                    //Sets the offset
                    this.spriteRenderer.Offset = animations[animationName].Offset;
                    //Sets the animation name
                    this.animationName = animationName;
                    //Sets the fps
                    this.fps = animations[animationName].Fps;
                    //Resets the animation
                    timeElapsed = 0;
                    currentIndex = 0;
                }
            }
        }
    }
}