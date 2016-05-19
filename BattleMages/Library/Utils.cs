using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public static class Utils
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

        private static Vector2 CalculatePosition(Vector2 position)
        {
            Vector2 realPosition = Vector2.Transform(position, GameWorld.Camera.WorldMatrix);
            return realPosition;
        }

        public static bool InsideEllipse(Vector2 pos, Vector2 ellipsePos, float ellipseWidth, float ellipseHeight)
        {
            float dist = ((float)Math.Pow(pos.X - ellipsePos.X, 2) / (float)Math.Pow(ellipseWidth, 2)) +
                ((float)Math.Pow(pos.Y - ellipsePos.Y, 2) / (float)Math.Pow(ellipseHeight, 2));

            return dist < 1f;
        }

        public static Vector2 LimitToCircle(Vector2 pos, Vector2 circlePos, float circleRadius)
        {
            Vector2 rPos = pos - circlePos;
            float mag = rPos.Length();
            if (mag > circleRadius)
            {
                Vector2 unit = new Vector2(rPos.X, rPos.Y);
                unit.Normalize();
                return (unit * circleRadius) + circlePos;
            }
            return pos;
        }

        public static Vector2 RotationPos(Vector2 mousePos, float rotationDegrees)
        {
            var m = Matrix.CreateRotationZ(MathHelper.ToRadians(rotationDegrees));
            return Vector2.TransformNormal(mousePos, m);
        }
    }
}