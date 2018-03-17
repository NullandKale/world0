using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            sw.WriteLine("<line>");
            cInfo.mode = clientMode.lineGraphicsMode;
            sw.WriteLine("<end>");
        }

        public void destroy()
        {
            if(cInfo != null)
            {
                cInfo.destroy();
            }
        }

        public void run()
        {
            while (true)
            {

                string message = sr.ReadLine();

                if (message == null || message == "qqq" || !cInfo.run)
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
                    sw.WriteLine("<line>");
                    cInfo.mode = clientMode.lineGraphicsMode;
                    break;
                default:
                    sw.WriteLine("Text Mode: Server Received " + message + " ");
                    sw.WriteLine("To Enter Line Graphics Mode Enter: <line>");
                    break;
            }
        }

        private void doLineGraphicsMode(string message)
        {
            if(message.Length >= 6)
            {
                switch (message.Substring(0, 6))
                {
                    case "<text>":
                        sw.WriteLine("Entering Text Graphics Mode.");
                        sw.WriteLine("<text>");
                        cInfo.mode = clientMode.textMode;
                        break;
                    case "<noop>":
                        //partialScreenUpdate();
                        fullScreenUpdate();
                        sw.WriteLine("<GTIN>");
                        break;
                    case "<GTIN>":
                        if (message.Length > 6 && message.ElementAt(6) != '\0')
                        {
                            cInfo.enterCommnad(message.ElementAt(6));
                        }
                        sw.WriteLine("<noop>");
                        break;
                    default:
                        sw.WriteLine("<noop>");
                        break;
                }
            }
        }

        private void fullScreenUpdate()
        {
            cInfo.framebuffer.update();

            if(cInfo.framebuffer.dirty)
            {
                List<char[]> screen = cInfo.framebuffer.getScreen();
                string toWrite = "";

                for (int i = 0; i < screen.Count; i++)
                {
                    toWrite += new String(screen[i]) + "\n<CD00>";
                }

                if (toWrite != "")
                {
                    sw.WriteLine("<CD00>" + toWrite);
                }
            }
        }
    }
}
