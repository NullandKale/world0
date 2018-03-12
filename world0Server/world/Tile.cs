using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using world0Server.utils;

namespace world0Server.world
{
    public class Tile
    {
        public generators.tileType type;
        public vector2 worldPos;
        public tileEntities.tileEntity ground;
        public tileEntities.tileEntity tEntity;

        public Tile(vector2 worldPos, tileEntities.tileEntity tEntity, generators.tileType t)
        {
            type = t;
            this.worldPos = worldPos;
            this.ground = tEntity;
        }

        public char getTexel()
        {
            if (tEntity != null)
            {
                return tEntity.tex;
            }
            else
            {
                return ground.tex;
            }
        }

        public static Tile duplicate(Tile t, vector2 pos)
        {
            return new Tile(pos, tileEntities.tileEntity.duplicate(t.ground), t.type);
        }
    }
}
