using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.world.tileEntities.level0
{
    public class emptyTile : Tile
    {
        public emptyTile(utils.vector2 worldPos, tileEntity tEntity) : base(worldPos, tEntity, generators.tileType.air)
        {

        }
    }
}
