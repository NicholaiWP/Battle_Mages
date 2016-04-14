using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class ViewCalculator
    {
        public float CalculateWidthScale(float resolutionWidth)
        {
            float widthScale = resolutionWidth / 1366;
            return widthScale;
        }

        public float CalculateHeightScale(float resolutionHeight)
        {
            float heightScale = resolutionHeight / 768;
            return heightScale;
        }

        public Rectangle CalculateRectangle(Vector2 position, int width, int height)
        {
            Vector2 realPosition = CalculatePosition(position);
            int realWidth = CalculateRectangleWidth(width);
            int realHeight = CalculateRectangleHeight(height);
            Rectangle rectangle = new Rectangle((int)realPosition.X, (int)realPosition.Y, realWidth, realHeight);
            return rectangle;
        }

        private Vector2 CalculatePosition(Vector2 position)
        {
            Vector2 realPosition = Vector2.Transform(position, Matrix.Invert(GameWorld.Instance.Camera.ViewMatrix));
            return realPosition;
        }

        private int CalculateRectangleWidth(int width)
        {
            float realWidth = width / GameWorld.Instance.MenuScreenManager.scale.X;
            return (int)realWidth;
        }

        private int CalculateRectangleHeight(int height)
        {
            float realHeight = height / GameWorld.Instance.MenuScreenManager.scale.Y;
            return (int)realHeight;
        }
    }
}