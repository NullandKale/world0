using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace world0Server.client
{
    public class clientInfo
    {
        public string userName;

        public clientMode mode = clientMode.textMode;
        public List<char[]> frameBuffer;


        private string passcode;

        public clientInfo(string userName, string passcode, int xSize, int ySize)
        {
            this.userName = userName;
            this.passcode = passcode;
            initFrameBuffer(xSize, ySize);
        }

        public void initFrameBuffer(int xSize, int ySize)
        {
            frameBuffer = new List<char[]>();

            for (int y = 0; y < ySize; y++)
            {
                char[] temp = new char[xSize];

                for (int x = 0; x < xSize; x++)
                {
                    if ((x % 2 == 0 && y % 2 == 1) || x % 2 == 1 && y % 2 == 0)
                    {
                        temp[x] = 'X';
                    }
                    else
                    {
                        temp[x] = '-';
                    }
                }

                frameBuffer.Add(temp);
            }
        }

        public bool checkPasscode(string isPasscode)
        {
            return passcode == isPasscode;
        }

        public override string ToString()
        {
            return userName + "," + passcode;
        }
    }

    public enum clientMode
    {
        textMode,
        lineGraphicsMode,

    }
}
