using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace world0Client.server
{
    public class serverManager
    {
        private static TcpClient client;
        public static Server get(string ipString, out Server toReturn)
        {
            if(client == null)
            {
                IPAddress addr = IPAddress.Parse(ipString);
                Console.WriteLine("\nConnecting to: " + addr);
                client = new TcpClient(addr.ToString(), 51234);
                toReturn = new Server(addr.ToString(), client.GetStream());
                return toReturn;
            }
            toReturn = null;
            return null;
        }

        public static void disconnect()
        {
            client.Close();
            client = null;
        }
    }

    public class Server
    {
        public string ipString;
        private Stream s;
        public StreamReader sr;
        public StreamWriter sw;
        public clientMode clientMode = clientMode.textMode;
        public bool dirtyConsole;

        public Server(string ip, Stream s)
        {
            ipString = ip;
            this.s = s;
            sr = new StreamReader(s);
            sw = new StreamWriter(s);
            sw.AutoFlush = true;
        }

        public void disconnect()
        {
            s.Close();
        }
    }

    public enum clientMode
    {
        textMode,
        lineGraphicsMode,

    }
}
