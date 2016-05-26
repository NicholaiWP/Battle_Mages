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

        public static bool InsideCircle(Vector2 currentPos, Vector2 midPos, float circleRadius)
        {
            bool isObjectInside;
            Vector2 vec = Vector2.Subtract(currentPos, midPos);
            float lengthOfVec = vec.Length();
            if (lengthOfVec >= circleRadius)
            {
                isObjectInside = false;
            }
            else
            {
                isObjectInside = true;
            }
            return isObjectInside;
        }

        public static Vector2 PosInsideCircle(Vector2 midPos, float circleRadius)
        {
            Vector2 pos = Vector2.Zero;

            return pos;
        }

        public static Vector2 RotateVector(Vector2 vector, float rotationDegrees)
        {
            float angleTowardsTarget = (float)Math.Atan2(vector.Y, vector.X);
            float angleCw = angleTowardsTarget + MathHelper.ToRadians(rotationDegrees);
            return new Vector2((float)Math.Cos(angleCw), (float)Math.Sin(angleCw));
        }
    }
}