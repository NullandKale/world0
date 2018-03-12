using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace world0Server.netCode
{
    public class serveThread
    {
        public int id;
        private Thread t;

        public serveThread(int id)
        {
            this.id = id;
            t = new Thread(new ThreadStart(run));
        }

        public void start()
        {
            Console.WriteLine("Starting Serve Thread: " + id);
            t.Start();
        }

        public void waitStop()
        {
            t.Join();
            Console.WriteLine("Stopped Serve Thread: " + id);
        }

        public void run()
        {
            while (serveManager.run)
            {
                Socket soc = serveManager.tcpListener.AcceptSocket();
                Console.WriteLine("Connected To: " + soc.RemoteEndPoint);
                client.clientProcessor clientPro = null;

                try
                {
                    Stream s = new NetworkStream(soc);
                    clientPro = new client.clientProcessor(s);
                    clientPro.run();
                    s.Close();
                }
                catch (Exception e)
                {
                    if (clientPro != null)
                    {
                        clientPro.destroy();
                    }
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Connection Terminated with: " + soc.RemoteEndPoint);
                soc.Close();
            }
        }
    }
}
