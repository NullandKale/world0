using System;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace world0Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Write("Enter Server IP: ");
                IPAddress addr = IPAddress.Parse("192.168.1.126");
                try
                {
                    addr = IPAddress.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
                Console.WriteLine("\nConnecting to: " + addr);
                TcpClient client = new TcpClient(addr.ToString(), 51234);

                try
                {
                    Stream s = client.GetStream();
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);
                    sw.AutoFlush = true;
                    bool stayOpen = true;
                    while (stayOpen)
                    {
                        string readLine = "";
                        while (readLine != "<end>")
                        {
                            readLine = sr.ReadLine();
                            if (readLine != null && readLine != "<end>")
                            {
                                Console.WriteLine(readLine);
                            }

                            if (readLine == "<quit>")
                            {
                                stayOpen = false;
                            }
                        }
                        Console.Write("->");
                        string message = Console.ReadLine();
                        sw.WriteLine(message);
                    }
                    s.Close();
                }
                finally
                {
                    client.Close();
                }
            }
        }

        static void doTextMode()
        {

        }

        static void doLineMode()
        {

        }
    }
}