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
            Console.Write("Enter Server IP: ");
            IPAddress addr = IPAddress.None;
            try
            {
                addr = IPAddress.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            Console.WriteLine("\nConnecting to: " + addr);
            //IPEndPoint endPoint = new IPEndPoint(addr, 51234);
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
                        if (!readLine.Contains("<"))
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
}