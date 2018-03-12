using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.utils
{
    public class vector2
    {
        public int x;
        public int y;

        public vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public vector2()
        {
            x = 0;
            y = 0;
        }

        public override string ToString()
        {
            return "{ " + x + ", " + y + " }";
        }

        public override bool Equals(object obj)
        {
            return (x == (obj as vector2).x && y == (obj as vector2).y);
        }

        public override int GetHashCode()
        {
            return x << 16 & (y << 16) >> 16;
        }
    }
}
