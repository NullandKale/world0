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
                string messageOutBeginning = "Message out: ";
                string messageEnd = ":-" + soc.RemoteEndPoint;
                Console.WriteLine("Connected: " + soc.RemoteEndPoint);

                try
                {
                    Stream s = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);

                    sw.AutoFlush = true;
                    sw.WriteLine("Welcome to World0");

                    client.clientInfo cInfo = client.clientManager.userBuilder(sr, sw);
                    Console.WriteLine(messageOutBeginning + "Hello " + cInfo.userName + messageEnd);
                    sw.WriteLine("Login Successful, " + cInfo.userName);
                    sw.WriteLine("<end>");

                    client.clientManager.saveCSV();

                    while (true)
                    {
                        string message = sr.ReadLine();
                        Console.WriteLine("Message in: " + message);
                        if (message == null || message == "qqq")
                        {
                            sw.WriteLine("<quit>");
                            sw.WriteLine("<end>");
                            break;
                        }
                        else
                        {
                            if (serveManager.run)
                            {
                                sw.WriteLine("I recieved: " + message);
                                sw.WriteLine("<end>");
                            }
                            else
                            {
                                sw.WriteLine("<quit>");
                                sw.WriteLine("<end>");
                                break;
                            }
                        }
                    }
                    s.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Connection Terminated with: " + soc.RemoteEndPoint);
                soc.Close();
            }
        }
    }
}
