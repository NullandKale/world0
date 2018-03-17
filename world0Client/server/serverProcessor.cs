﻿using System;
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
        private double averageLatency = 0;

        private Dictionary<int, char[]> screen;

        public ServerProcessor(Server s)
        {
            this.s = s;
            stayOpen = true;
            screen = new Dictionary<int, char[]>();
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
                        Console.Clear();
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

                frameTimeAccumulator += utils.time.asMilliseconds() - startTime;
                frameTimeCounter++;

                averageLatency = ((double)frameTimeAccumulator / frameTimeCounter) / 2;
                s.sw.WriteLine(message);

            }
        }

        


        public string doTextMode(List<string> messages)
        {
            string toReturn = "<NULL>";
            bool getIn = false;
            foreach (string message in messages)
            {
                switch (message)
                {
                    case "<line>":
                        s.clientMode = clientMode.lineGraphicsMode;
                        break;
                    case "<GXSZ>":
                        toReturn = Console.WindowWidth.ToString();
                        getIn = false;
                        break;
                    case "<GYSZ>":
                        toReturn = Console.WindowHeight.ToString();
                        getIn = false;
                        break;
                    default:
                        Console.WriteLine(message);
                        s.dirtyConsole = true;
                        getIn = true;
                        break;
                }
            }

            if(getIn)
            {
                toReturn = Console.ReadLine();
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

            List<string> linesToDraw = new List<string>();

            for (int i = 0; i < messages.Count; i++)
            {
                switch (messages[i].Substring(0,6))
                {
                    case "<text>":
                        s.clientMode = clientMode.textMode;
                        break;
                    case "<CD00>":
                        if(messages[i].Length > 7)
                        {
                            linesToDraw.Add(messages[i].Substring(7));
                        }
                        break;
                    case "<STLN>":
                        string[] split = messages[i].Substring(7).Split(',');
                        int lineNum = int.Parse(split[0]);
                        Console.SetCursorPosition(0, lineNum);
                        Console.WriteLine(split[1]);
                        break;
                    case "<CLEA>":
                        s.dirtyConsole = true;
                        break;
                    case "<GTIN>":
                        toReturn = "<GTIN>" + spinWait(1);
                        break;
                    case "<noop>":
                        toReturn = "<noop>";
                        break;
                    default:
                        break;
                }
            }

            if(linesToDraw.Count > 0)
            {
                Console.SetCursorPosition(0, 0);

                for (int i = 0; i < linesToDraw.Count; i++)
                {
                    Console.WriteLine(linesToDraw[i]);
                }

                Console.Write("AOWL: " + String.Format("{0:0}MS       ", averageLatency));
            }


            return toReturn;
        }

        private char spinWait(long waitTimeMS)
        {
            long startTime = utils.time.asMilliseconds();
            
            while(true)
            {
                if(utils.time.asMilliseconds() - startTime > waitTimeMS)
                {
                    return '\0';
                }
                else
                {
                    if(Console.KeyAvailable)
                    {
                        char toReturn = Console.ReadKey().KeyChar;
                        while(Console.KeyAvailable)
                        {
                            Console.ReadKey();
                        }
                        return toReturn;
                    }
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
