﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Animation
    {
        //Fields
        private float fps;

        private Vector2 offset;

        //Array
        private Rectangle[] frames;

        //Properties
        public float GetFps { get { return fps; } }

        public Vector2 GetOffset { get { return offset; } }
        public Rectangle[] GetFrames { get { return frames; } }

        /// <summary>
        /// The Constructer for the animation
        /// </summary>
        /// <param name="framesCount"></param>
        /// <param name="yPos"></param>
        /// <param name="xStartFrame"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fps"></param>
        /// <param name="offset"></param>
        public Animation(int framesCount, int yPos, int xStartFrame, int width, int height, float fps, Vector2 offset)
        {
            this.frames = new Rectangle[framesCount];
            this.offset = offset;
            this.fps = fps;

            for (int i = 0; i < framesCount; i++)
            {
                GetFrames[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
            }
        }
    }
}