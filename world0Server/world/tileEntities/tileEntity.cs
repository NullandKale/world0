using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.world.tileEntities
{
    public class tileEntity
    {
        public bool isEmpty;
        public char tex;

        public tileEntity(char t)
        {
            isEmpty = true;
            tex = t;
        }

        public virtual void update(Tile tile)
        {

        }

        public static tileEntity duplicate(tileEntity tEntity)
        {
            return new tileEntity(tEntity.tex);
        }
    }
}
