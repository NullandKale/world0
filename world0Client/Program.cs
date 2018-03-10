using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using world0Client.server;

namespace world0Client
{
    public class Program
    {
        const string version = "world0Client Alpha v 0.0.1";
        static void Main(string[] args)
        {
            bool staticIP = true;
            while(true)
            {
                Console.WriteLine(version);
                Console.Write("Enter Server IP: ");
                string ip = "192.168.1.126";
                if (!staticIP)
                {
                    ip = Console.ReadLine();
                }
                else
                {
                    Console.ReadLine();
                }

                Server server;
                serverManager.get(ip, out server);
                ServerProcessor processor = new ServerProcessor(server);
                processor.run();
                processor.disconnect();
            }
        }

    }
}