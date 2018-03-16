using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using world0Server.utils;

namespace world0Server.world
{
    public class worldStore
    {
        public Tile[,] tiles;

        private vector2 size;

        public worldStore(vector2 size)
        {
            this.size = size;
            tiles = new Tile[size.x, size.y];
        }

        public Tile getTile(vector2 pos)
        {
            lock (tiles) lock (size)
                {
                    if (util.isInRange(pos.x, -1, size.x) && util.isInRange(pos.y, -1, size.y))
                    {
                        return tiles[pos.x, pos.y];
                    }
                    else
                    {
                        return null;
                    }
                }
        }

        public Tile[,] getTiles(vector2 offset, int range)
        {
            Tile[,] temp = new Tile[2 * range + 1, 2 * range + 1];

            for (int i = offset.x - range; i <= offset.x + range; i++)
            {
                for (int j = offset.y - range; j <= offset.y + range; j++)
                {
                    temp[offset.x - i, offset.y - j] = getTile(new vector2(i, j));
                }
            }

            return temp;
        }

        public List<char[]> getLines(vector2 center, int xSize, int ySize)
        {
            List<char[]> toReturn = new List<char[]>();

            for (int y = center.y; y < center.y + ySize; y++)
            {
                char[] temp = new char[xSize];
                for (int x = center.x; x < center.x + xSize; x++)
                {
                    temp[x - center.x] = getTile(new vector2(x, y)).getTexel();
                }
                toReturn.Add(temp);
            }

            return toReturn;
        }

        public void startSimulation()
        {
            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();

            long timeSum = 0;
            long timeCount = 0;

            while (Program.RUN)
            {
                s.Start();

                simulate(false);

                s.Stop();
                timeCount++;
                timeSum += s.ElapsedMilliseconds;
                s.Reset();

                Program.toDisplay += (1000f / ((float)timeSum / (float)timeCount)).ToString("0.0") + "TPS ";
            }
        }

        public void simulate(bool showProgressBar)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    lock (tiles[x, y])
                    {
                        //tiles[x, y].ground.update(tiles[x, y]);
                        if (tiles[x, y].tEntity != null && !tiles[x, y].tEntity.isEmpty)
                        {
                            tiles[x, y].tEntity.update(tiles[x, y]);
                        }
                    }
                }
                if (showProgressBar)
                {
                    int progressBarCharCount = 20;
                    int totalIterations = tiles.GetLength(0);
                    int charCount = x / (totalIterations / progressBarCharCount);

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(0, (Console.WindowHeight - 1));
                    Console.Write("Progress: [");
                    for (int i = 0; i < progressBarCharCount - 1; i++)
                    {
                        if (i < charCount)
                        {
                            Console.Write('#');
                        }
                        else
                        {
                            Console.Write(' ');
                        }
                    }
                    Console.Write("]");
                }
            }
        }
    }
}
