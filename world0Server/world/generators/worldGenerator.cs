using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using world0Server.utils;

namespace world0Server.world.generators
{
    public class worldGenerator
    {
        public vector2 size;
        private int rngSeed;
        private Random rng;

        private Tile air = new Tile(new vector2(), new tileEntities.tileEntity(' ', false), tileType.air);
        private Tile water = new Tile(new vector2(), new tileEntities.tileEntity('~', true), tileType.water);
        private Tile grass = new Tile(new vector2(), new tileEntities.tileEntity('_', false), tileType.grass);
        private Tile stone = new Tile(new vector2(), new tileEntities.tileEntity('#', true), tileType.stone);
        private Tile sand = new Tile(new vector2(), new tileEntities.tileEntity('s', false), tileType.sand);
        private Tile tree = new Tile(new vector2(), new tileEntities.tileEntity('T', true), tileType.tree);

        public worldGenerator(int seed, vector2 size)
        {
            rngSeed = seed;
            rng = new Random(rngSeed);
            this.size = size;

        }

        public worldStore generate()
        {
            Console.WriteLine("Generating World with seed of: " + rngSeed);

            worldStore workingWorld = new worldStore(size);

            Console.Write("Tile Generation...");
            tileType[,] tiles = genTiles();
            placePlants(tiles);
            Console.WriteLine(" done");

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    switch (tiles[x, y])
                    {
                        case tileType.stone:
                            workingWorld.tiles[x, y] = Tile.duplicate(stone, new vector2(x, y));
                            break;
                        case tileType.grass:
                            workingWorld.tiles[x, y] = Tile.duplicate(grass, new vector2(x, y));
                            break;
                        case tileType.water:
                            workingWorld.tiles[x, y] = Tile.duplicate(water, new vector2(x, y));
                            break;
                        case tileType.sand:
                            workingWorld.tiles[x, y] = Tile.duplicate(sand, new vector2(x, y));
                            break;
                        case tileType.tree:
                            workingWorld.tiles[x, y] = Tile.duplicate(tree, new vector2(x, y));
                            break;
                        default:
                            workingWorld.tiles[x, y] = Tile.duplicate(air, new vector2(x, y));
                            break;
                    }
                }
            }

            return workingWorld;
        }

        private tileType[,] genTiles()
        {
            tileType[,] tiles = new tileType[size.x, size.y];

            Simplex.Noise.Seed = rngSeed;
            float scale = 0.005f;

            float[,] noiseMap = Simplex.Noise.Calc2D(size.x, size.y, scale);
            vector2f range = getRange(noiseMap);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    tiles[x, y] = tileType.water;

                    if (isInHeight(noiseMap[x, y], range.y, 0.75f))
                    {
                        tiles[x, y] = tileType.sand;
                    }
                    if (isInHeight(noiseMap[x, y], range.y, 0.65f))
                    {
                        tiles[x, y] = tileType.grass;
                    }
                    if (isInHeight(noiseMap[x, y], range.y, 0.05f))
                    {
                        tiles[x, y] = tileType.stone;
                    }
                }
            }

            return tiles;
        }

        private void placePlants(tileType[,] initialTiles)
        {
            int forestCountX = 5;
            int forestCountY = 5;
            int treesPerForest = 96;
            int rMax = 32;
            float yStretch = 0.5f;

            for(int i = 0; i < forestCountX; i++)
            {
                for(int j = 0; j < forestCountY; j++)
                {
                    int xCenter = rng.Next(0, size.x);
                    int yCenter = rng.Next(0, size.y);

                    for(int x = 0; x < treesPerForest; x++)
                    {
                        double r = Math.Sqrt((double)rng.Next() / int.MaxValue) * rMax;
                        double theta = (double)rng.Next() / int.MaxValue * 2 * Math.PI;

                        int xPos = xCenter + (int)(r * Math.Cos(theta));
                        int yPos = yCenter + (int)(r * Math.Sin(theta) * yStretch);

                        if (xPos < 0)
                        {
                            xPos = 0;
                        }

                        if(yPos < 0)
                        {
                            yPos = 0;
                        }

                        if(xPos > size.x - 1)
                        {
                            xPos = size.x - 1;
                        }

                        if(yPos > size.y - 1)
                        {
                            yPos = size.y - 1;
                        }
                        if(initialTiles[xPos, yPos] == tileType.grass)
                        {
                            initialTiles[xPos, yPos] = tileType.tree;
                        }
                    }
                }
            }
        }

        private bool isInHeight(float value, float max, float percent)
        {
            return util.isWithin(value, max - (max * (percent / 2)), (max * (percent / 2)));
        }

        private vector2f getRange(float[,] noiseMap)
        {
            float max = float.MinValue;
            float min = float.MaxValue;

            for(int i = 0; i < noiseMap.GetLength(0); i++)
            {
                for(int j = 0; j < noiseMap.GetLength(1); j++)
                {
                    if(noiseMap[i,j] > max)
                    {
                        max = noiseMap[i, j];
                    }

                    if(noiseMap[i,j] < min)
                    {
                        min = noiseMap[i, j];
                    }
                }
            }

            return new vector2f(min,max);
        }
    }

    public enum tileType
    {
        air,
        water,
        sand,
        grass,
        stone,
        tree
    }

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
            lock(tiles) lock(size)
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

            for(int i = offset.x - range; i <= offset.x + range; i++)
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

            for(int y = center.y; y < center.y + ySize; y++)
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
                    lock(tiles[x,y])
                    {
                        //tiles[x, y].ground.update(tiles[x, y]);
                        if(tiles[x,y].tEntity != null && !tiles[x,y].tEntity.isEmpty)
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
                    for(int i = 0; i < progressBarCharCount - 1; i++)
                    {
                        if(i < charCount)
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
