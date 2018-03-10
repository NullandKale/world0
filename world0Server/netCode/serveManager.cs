using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using world0Server.netCode;

namespace world0Server
{
    public class serveManager
    {
        public static bool run = true;
        public static TcpListener tcpListener;
        public int port = 51234;
        public int MAXSUPPORTEDCLIENTS = 10;
        public List<serveThread> serveThreads;

        private static serveManager server;

        public serveManager()
        {
            singletonCheck();
            netCodeINIT();
            initServeThreads();
        }

        public serveManager(int port, int maxClients)
        {
            singletonCheck();
            netCodeINIT();
            initServeThreads();
        }

        public void start()
        {
            for (int i = 0; i < MAXSUPPORTEDCLIENTS; i++)
            {
                serveThreads[i].start();
            }
        }

        public void waitStop()
        {
            for (int i = 0; i < MAXSUPPORTEDCLIENTS; i++)
            {
                serveThreads[i].waitStop();
            }
        }

        private void singletonCheck()
        {
            if (server == null)
            {
                server = this;
            }
            else
            {
                throw new Exception("ERROR: Singleton Exception @ serverManager");
            }
        }

        private void netCodeINIT()
        {
            IPAddress addrs = IPAddress.Any;
            tcpListener = new TcpListener(addrs, port);
            tcpListener.Start();
            Console.WriteLine("Server Started on IP: " + addrs + ":" + port);
        }

        private void initServeThreads()
        {
            serveThreads = new List<serveThread>();

            for (int i = 0; i < MAXSUPPORTEDCLIENTS; i++)
            {
                serveThreads.Add(new serveThread(i));
            }
        }
    }
}
