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
        public bool isCollideable;

        public tileEntity(char t, bool collides)
        {
            isEmpty = true;
            tex = t;
            isCollideable = collides;
        }

        public virtual void update(Tile tile)
        {

        }

        public static tileEntity duplicate(tileEntity tEntity)
        {
            return new tileEntity(tEntity.tex, tEntity.isCollideable);
        }
    }
}
