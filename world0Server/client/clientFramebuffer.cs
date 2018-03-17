using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using world0Server.utils;

namespace world0Server.client
{
    public class clientFramebuffer
    {
        public bool dirty;
        private char[][] framebuffer;
        private vector2 size;
        private vector2 screenPos;

        public clientFramebuffer(vector2 size)
        {
            this.dirty = true;
            this.size = size;
            screenPos = new vector2();
            initFrameBuffer();
            clearFrameBuffer();
        }

        private void initFrameBuffer()
        {
            framebuffer = new char[size.y][];
            for (int i = 0; i < size.y; i++)
            {
                framebuffer[i] = new char[size.x];
            }
        }

        private void clearFrameBuffer()
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    framebuffer[y][x] = new char();
                    framebuffer[y][x] = ' ';
                }
            }
        }

        public void updateScreenPos(vector2 pcPos)
        {
            this.screenPos = new vector2(pcPos.x - (size.x / 2) + 1, pcPos.y - (size.y / 2));

            if (screenPos.x < 0)
            {
                screenPos.x = 0;
            }

            if (screenPos.y < 0)
            {
                screenPos.y = 0;
            }

            if (screenPos.x + size.x + 1 > Program.getWorld().tiles.GetLength(0))
            {
                screenPos.x = (short)(Program.getWorld().tiles.GetLength(0) - size.x);
            }

            if (screenPos.y + size.y + 1 > Program.getWorld().tiles.GetLength(1))
            {
                screenPos.y = (short)(Program.getWorld().tiles.GetLength(1) - size.y);
            }
        }

        public void update()
        {
            world.worldStore world0 = Program.getWorld();
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    char tile = world0.getTile(new vector2(x + screenPos.x, y + screenPos.y)).getTexel();
                    if(tile != framebuffer[y][x])
                    {
                        framebuffer[y][x] = tile;
                        dirty = true;
                    }
                }
            }
        }

        public List<char[]> getScreen()
        {
            dirty = false;
            
            List<char[]> toReturn = new List<char[]>();

            for (int y = 0; y < size.y; y++)
            {
                char[] temp = framebuffer[y];
                toReturn.Add(temp);
            }

            return toReturn;
        }
    }
}
