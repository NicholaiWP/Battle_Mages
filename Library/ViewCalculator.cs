using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public static class ViewCalculator
    {
        public static float CalculateWidthScale(float resolutionWidth)
        {
            float widthScale = resolutionWidth / GameWorld.GameWidth;
            return widthScale;
        }

        public static float CalculateHeightScale(float resolutionHeight)
        {
            float heightScale = resolutionHeight / GameWorld.GameHeight;
            return heightScale;
        }

        public static Rectangle CalculateRectangle(Vector2 position, int width, int height)
        {
            Vector2 realPosition = CalculatePosition(position);
            int realWidth = CalculateRectangleWidth(width);
            int realHeight = CalculateRectangleHeight(height);
            Rectangle rectangle = new Rectangle((int)realPosition.X, (int)realPosition.Y, realWidth, realHeight);
            return rectangle;
        }

        private static Vector2 CalculatePosition(Vector2 position)
        {
            Vector2 realPosition = Vector2.Transform(position, GameWorld.Camera.WorldMatrix);
            return realPosition;
        }

        private static int CalculateRectangleWidth(int width)
        {
            float realWidth = width / GameWorld.MenuScreenManager.ScalingVector.X;
            return (int)realWidth;
        }

        private static int CalculateRectangleHeight(int height)
        {
            float realHeight = height / GameWorld.MenuScreenManager.ScalingVector.Y;
            return (int)realHeight;
        }
    }
}