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
            bool staticIP = false;
            while(true)
            {
                Console.WriteLine(version);
                Console.Write("Enter Server IP: ");
                string ip = "98.246.224.38";
                if (!staticIP)
                {
                    string temp = Console.ReadLine();
                    IPAddress tempAddr = null;
                    if(IPAddress.TryParse(temp, out tempAddr))
                    {
                        ip = temp;
                    }
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