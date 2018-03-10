using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.client
{
    public class clientProcessor
    {
        private StreamReader sr;
        private StreamWriter sw;
        private clientInfo cInfo;

        public clientProcessor(Stream s)
        {
            sr = new StreamReader(s);
            sw = new StreamWriter(s);

            sw.AutoFlush = true;
            sw.WriteLine("Welcome to World0");

            cInfo = clientManager.userBuilder(sr, sw);
            sw.WriteLine("Login Successful, " + cInfo.userName);


            sw.WriteLine("<end>");
        }

        public void run()
        {
            while (true)
            {
                string message = sr.ReadLine();

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
                        switch (cInfo.mode)
                        {
                            case clientMode.textMode:
                                doTextMode(message);
                                sw.WriteLine("<end>");
                                break;
                            case clientMode.lineGraphicsMode:
                                doLineGraphicsMode(message);
                                sw.WriteLine("<end>");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        sw.WriteLine("<quit>");
                        sw.WriteLine("<end>");
                        break;
                    }
                }
            }
        }

        private void doTextMode(string message)
        {
            switch (message)
            {
                case "<line>":
                    sw.WriteLine("Entering Line Graphics Mode.");
                    break;
                default:
                    sw.WriteLine("Text Mode: Server Received " + message + " ");
                    sw.WriteLine("To Enter Line Graphics Mode Enter: <line>");
                    break;
            }
        }

        private void doLineGraphicsMode(string message)
        {
            switch (message)
            {
                case "<text>":
                    sw.WriteLine("Entering Line Graphics Mode.");
                    break;
                default:
                    sw.WriteLine("Line Graphics Mode: Server Received " + message);
                    sw.WriteLine("To Enter Line Graphics Mode Enter: <text>");
                    break;
            }
        }
    }
}
