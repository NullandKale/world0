using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using world0Server.utils;
using world0Server.world;

namespace world0Server.client
{
    public class clientInfo
    {
        public string userName;
        public vector2 pcPos;
        public vector2 screenPos;

        public clientMode mode = clientMode.textMode;
        public List<char[]> frameBuffer;

        private string passcode;

        private inputMode iMode = inputMode.movementMode;
        private string message;

        public clientInfo(string userName, string passcode, int xSize, int ySize)
        {
            this.userName = userName;
            this.passcode = passcode;
            initFrameBuffer(xSize, ySize);
            pcPos = new vector2(10, 10);
            screenPos = new vector2();
            message = "";
        }

        public void destroy()
        {
            displayName(true);
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

        public void enterCommnad(char command)
        {
            switch (iMode)
            {
                case inputMode.movementMode:
                    doMoveMode(command);
                    break;
                case inputMode.textMode:
                    doTextMode(command);
                    break;
                default:
                    break;
            }
        }

        private void doTextMode(char command)
        {
            if (command == '~')
            {
                if (iMode == inputMode.movementMode)
                {
                    iMode = inputMode.textMode;
                }
                else
                {
                    iMode = inputMode.movementMode;
                }
            }
            else
            {
                message += command;
            }

            displayName(false);
        }

        private void doMoveMode(char command)
        {
            vector2 pos = new vector2(pcPos.x, pcPos.y);
            if (command == 'w')
            {
                if (pcPos.y - 1 >= 0 && pcPos.y - 1 < worldManager.size.y)
                {
                    pos.y -= 1;
                }
            }

            if (command == 's')
            {
                if (pcPos.y + 1 >= 0 && pcPos.y + 1 < worldManager.size.y)
                {
                    pos.y += 1;
                }
            }

            if (command == 'a')
            {
                if (pcPos.x - 1 >= 0 && pcPos.x - 1 < worldManager.size.x)
                {
                    pos.x -= 1;
                }
            }

            if (command == 'd')
            {
                if (pcPos.x + 1 >= 0 && pcPos.x + 1 < worldManager.size.x)
                {
                    pos.x += 1;
                }
            }

            if (command == '~')
            {
                if (iMode == inputMode.movementMode)
                {
                    iMode = inputMode.textMode;
                    displayName(true);
                    message = "";
                }
                else
                {
                    iMode = inputMode.movementMode;
                }
            }

            move(pos);
        }

        public void move(vector2 movement)
        {
            displayName(true);
            pcPos = movement;
            screenPos = updateScreenPos();
            displayName(false);
        }

        public vector2 updateScreenPos()
        {
            vector2 newScreenPos = new vector2(pcPos.x - (frameBuffer[0].Length / 2) + 1, pcPos.y - (frameBuffer.Count / 2));

            if (newScreenPos.x < 0)
            {
                newScreenPos.x = 0;
            }

            if (newScreenPos.y < 0)
            {
                newScreenPos.y = 0;
            }

            if (newScreenPos.x + frameBuffer[0].Length + 1 > Program.getWorld().tiles.GetLength(0))
            {
                newScreenPos.x = (short)(Program.getWorld().tiles.GetLength(0) - frameBuffer[0].Length);
            }

            if (newScreenPos.y + frameBuffer.Count + 1 > Program.getWorld().tiles.GetLength(1))
            {
                newScreenPos.y = (short)(Program.getWorld().tiles.GetLength(1) - frameBuffer.Count);
            }

            return newScreenPos;
        }


        public void displayName(bool clear)
        {
            if(clear)
            {
                for (int i = 0; i < userName.Length; i++)
                {
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - userName.Length / 2 + i, pcPos.y + 1));
                    if(temp != null)
                    {
                        if (temp.tEntity != null)
                        {
                            temp.tEntity = null;
                        }
                    }
                }

                for (int i = 0; i < message.Length; i++)
                {
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - message.Length / 2 + i, pcPos.y + 1));
                    if (temp != null)
                    {
                        if (temp.tEntity != null)
                        {
                            temp.tEntity = null;
                        }
                    }
                }

                if (Program.getWorld().getTile(pcPos) != null)
                {
                    if (Program.getWorld().getTile(pcPos).tEntity != null)
                    {
                        Program.getWorld().getTile(pcPos).tEntity = null;
                    }
                }
            }
            else
            {
                for (int i = 0; i < userName.Length; i++)
                {
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - userName.Length / 2 + i, pcPos.y + 1));
                    if(temp != null)
                    {
                        if(temp.tEntity == null)
                        {
                            temp.tEntity = new world.tileEntities.tileEntity(userName.ToCharArray()[i]);
                        }
                    }
                    
                }

                for (int i = 0; i < message.Length; i++)
                {
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - message.Length / 2 + i, pcPos.y + 2));

                    if (temp != null)
                    {
                        if (temp.tEntity == null)
                        {
                            temp.tEntity = new world.tileEntities.tileEntity(message.ToCharArray()[i]);
                        }
                    }
                }

                Tile pcLoc = Program.getWorld().getTile(pcPos);

                if (pcLoc != null)
                {
                    if (pcLoc.tEntity == null)
                    {
                        pcLoc.tEntity = new world.tileEntities.tileEntity('@');
                    }
                }
            }
        }

        public void updateFrameBuffer()
        {
            frameBuffer = Program.getWorld().getLines(screenPos, frameBuffer[0].Length, frameBuffer.Count());
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

    public enum inputMode
    {
        movementMode,
        textMode
    }
}
