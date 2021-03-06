﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public static class Utils
    {
        public const float AreaSize = 320;

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

        public static bool ContainsSubstring(string haystack, string needle)
        {
            if (haystack == null)
            {
                return false;
            }
            return haystack.Substring(0, 4).Contains(needle);
        }

        public static string WarpText(string text, int width, SpriteFont font)
        {
            int lastWhitespace = 0;
            Vector2 currentTargetSize;
            StringBuilder output = new StringBuilder();

            string newline = Environment.NewLine;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (char.IsWhiteSpace(c))
                    lastWhitespace = output.Length;
                output.Append(c);
                currentTargetSize = font.MeasureString(output);
                if (currentTargetSize.X > width)
                {
                    output.Insert(lastWhitespace, newline);
                    output.Remove(lastWhitespace + newline.Length, 1);
                }
            }

            return output.ToString();
        }

        public static Vector2 HalfTexSize(Texture2D tex)
        {
            return new Vector2(tex.Width, tex.Height) / 2f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifeToHeal"></param>
        public static void HealOnHit(int lifeToHeal)
        {
            foreach (GameObject go in GameWorld.Scene.ActiveObjects)
            {
                var player = go.GetComponent<Player>();
                if(player != null)
                {
                    int chance = GameWorld.Random.Next(1, 101);
                    if(chance <= 18)
                    {
                        go.GetComponent<Player>().Heal(lifeToHeal);
                    }
                }
            }
        }
    }
}