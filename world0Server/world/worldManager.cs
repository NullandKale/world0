using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using world0Server.world.generators;
using world0Server.world;
using world0Server.utils;

namespace world0Server
{
    public class worldManager
    {
        public static vector2 size = new vector2(1000, 1000);
        public worldStore wStore;
        private worldGenerator wGen;

        public worldManager()
        {
            wGen = new worldGenerator(0, size);
            wStore = wGen.generate();
        }

        public void start()
        {

        }

        public void waitStop()
        {

        }
    }
}
