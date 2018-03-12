using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.utils
{
    public static class util
    {
        public static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static bool isInRange(float value, float min, float max)
        {
            return (value > min && value < max);
        }

        public static bool isWithin(float value, float center, float change)
        {
            return (value < center + change && value > center - change);
        }
    }



    public class texel
    {
        public char c;
        public ConsoleColor fg;
        public ConsoleColor bg;

        public texel(char t, ConsoleColor fGroundColor, ConsoleColor bGroundColor)
        {
            c = t;
            fg = fGroundColor;
            bg = bGroundColor;
        }
    }

    public class vector2f
    {
        public float x;
        public float y;

        public vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
