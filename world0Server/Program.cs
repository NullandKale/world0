using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace world0Server
{
    class Program
    {
        public static string pathStart = "C:/world0/";
        public static bool RUN = true;
        public static string toDisplay = "";

        private static worldManager worldMan;
        private static serveManager netMan;

        static void Main(string[] args)
        {
            init();

            while(RUN)
            {
                display();
            }

            stop();
        }

        private static void init()
        {
            worldMan = new worldManager();
            worldMan.start();

            netMan = new serveManager();
            netMan.start();
        }

        private static void stop()
        {
            netMan.waitStop();
            worldMan.waitStop();
        }

        private static void display()
        {
            if (toDisplay != "")
            {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(toDisplay);
                Console.SetCursorPosition(x, y);
            }
        }

        public static world.generators.worldStore getWorld()
        {
            return worldMan.wStore;
        }
    }
}
