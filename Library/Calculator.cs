using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class Calculator
    {
        public float WidthScaleCalculate(float resolutionWidth)
        {
            float widthScale = resolutionWidth / 1366;
            return widthScale;
        }

        public float HeightScaleCalculate(float resolutionHeight)
        {
            float heightScale = resolutionHeight / 768;
            return heightScale;
        }

        private Vector2 ThisPosition(Vector2 position)
        {
            Vector2 realPosition = Vector2.Transform(position, GameWorld.Instance.Camera.ViewMatrix);
            return realPosition;
        }

        public Rectangle ThisRectangle(Vector2 position, int width, int height)
        {
            Vector2 realPosition = ThisPosition(position);
            Rectangle rectangle = new Rectangle((int)realPosition.X, (int)realPosition.Y, width, height);
            return rectangle;
        }
    }
}