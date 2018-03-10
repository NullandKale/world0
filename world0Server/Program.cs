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
        static void Main(string[] args)
        {
            serveManager netMan = new serveManager();
            netMan.start();
            netMan.waitStop();
        }
    }
}
