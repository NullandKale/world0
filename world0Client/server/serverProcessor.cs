using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Client.server
{
    public class ServerProcessor
    {
        public Server s;

        private bool stayOpen;
        double FPS = 0;

        public ServerProcessor(Server s)
        {
            this.s = s;
            stayOpen = true;
        }

        public void run()
        {
            long frameTimeAccumulator = 0;
            long frameTimeCounter = 0;

            while (stayOpen)
            {
                long startTime = utils.time.asMilliseconds();

                string readLine = "";
                List<string> messages = new List<string>(); 
                while (readLine != "<end>")
                {
                    readLine = s.sr.ReadLine();
                    if (readLine != null && readLine != "<end>")
                    {
                        messages.Add(readLine);
                    }
                    if (readLine == "<quit>")
                    {
                        stayOpen = false;
                    }
                }

                string message = "<NULL>";

                switch (s.clientMode)
                {
                    case clientMode.textMode:
                        message = doTextMode(messages);
                        break;
                    case clientMode.lineGraphicsMode:
                        message = doLineMode(messages);
                        break;
                    default:
                        break;
                }

                if(message == "<NULL>")
                {
                    Console.Write("->");
                    message = Console.ReadLine();
                }

                frameTimeAccumulator += utils.time.asMilliseconds() - startTime;
                frameTimeCounter++;

                FPS = 1d / ((double)frameTimeAccumulator / frameTimeCounter) * 1000d;
                s.sw.WriteLine(message);

            }
        }

        


        public string doTextMode(List<string> messages)
        {
            string toReturn = "<NULL>";

            foreach (string message in messages)
            {
                switch (message)
                {
                    case "<line>":
                        s.clientMode = clientMode.lineGraphicsMode;
                        break;
                    case "<GXSZ>":
                        toReturn = Console.WindowWidth.ToString();
                        break;
                    case "<GYSZ>":
                        toReturn = Console.WindowHeight.ToString();
                        break;
                    default:
                        Console.WriteLine(message);
                        s.dirtyConsole = true;
                        break;
                }
            }

            return toReturn;
        }

        public string doLineMode(List<string> messages)
        {
            if(s.dirtyConsole)
            {
                Console.Clear();
                s.dirtyConsole = false;
            }
            string toReturn = "<NULL>";
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < messages.Count; i++)
            {
                switch (messages[i].Substring(0,6))
                {
                    case "<text>":
                        s.clientMode = clientMode.textMode;
                        break;
                    case "<CD00>":
                        Console.WriteLine(messages[i].Substring(7));
                        break;
                    case "<CLEA>":
                        s.dirtyConsole = true;
                        break;
                    case "<GTIN>":
                        char temp = spinWait(25);
                        if(temp != '\0')
                        {
                            toReturn = "<GTIN>" + temp;
                        }
                        else
                        {
                            toReturn = "<noop>";
                        }
                        break;
                    case "<noop>":
                        toReturn = "<noop>";
                        break;
                    default:
                        break;
                }
            }

            Console.Write("FPS: " + String.Format("{0:0.00}", FPS));


            return toReturn;
        }

        private char spinWait(long waitTimeMS)
        {
            long startTime = System.DateTime.Now.Millisecond;

            while(true)
            {
                if(Console.KeyAvailable)
                {
                    return Console.ReadKey().KeyChar;
                }
                
                if(System.DateTime.Now.Millisecond - startTime > waitTimeMS)
                {
                    return '\0';
                }
            }
        }

        public void disconnect()
        {
            s.disconnect();
            serverManager.disconnect();
        }
    }
}
