using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
                ShowNetworkTraffic();
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

        private static void ShowNetworkTraffic()
        {
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0];
            PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

            float kbs = 0;
            float kbr = 0;
            float seconds = 0;

            for (int i = 0; i < 25; i++)
            {
                kbs += (performanceCounterSent.NextValue() / 1024);
                kbr += (performanceCounterReceived.NextValue() / 1024);
                seconds += 0.5f;
                Thread.Sleep(500);
            }

            toDisplay = "" + (kbs / seconds) + "kb/s Sent " + (kbr / seconds) + "kb/s Received";
        }

        public static world.worldStore getWorld()
        {
            return worldMan.wStore;
        }
    }
}
