using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public static class ScaleCalculator
    {
        public static float WidthScaleCalculate(float resolutionWidth)
        {
            float widthScale = resolutionWidth / 1366;
            return widthScale;
        }

        public static float HeightSclaeCalculate(float resolutionHeight)
        {
            float heightScale = resolutionHeight / 766;
            return heightScale;
        }
    }
}