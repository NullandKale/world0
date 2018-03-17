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
        public bool run;

        public clientMode mode = clientMode.textMode;
        public clientFramebuffer framebuffer;

        private string passcode;

        private inputMode iMode = inputMode.movementMode;
        private string message;

        public clientInfo(string userName, string passcode, int xSize, int ySize)
        {
            this.userName = userName;
            this.passcode = passcode;
            framebuffer = new clientFramebuffer(new vector2(xSize, ySize));
            pcPos = new vector2(10, 10);
            screenPos = new vector2();
            message = "";
            run = true;
        }

        public void destroy()
        {
            displayName(true);
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
            displayName(true);
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
                if(command != '\b')
                {
                    message += command;
                }
                else
                {
                    if(message.Length >= 1)
                    {
                        message = message.Substring(0, message.Length - 1);
                    }
                }
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

            if(command == 'Q')
            {
                run = false;
            }

            move(pos);
        }

        public void move(vector2 movement)
        {
            if(!Program.getWorld().getTile(movement).ground.isCollideable)
            {
                displayName(true);
                pcPos = movement;
                framebuffer.updateScreenPos(pcPos);
                displayName(false);
            }
        }




        public void displayName(bool clear)
        {
            if(clear)
            {
                for (int i = 0; i < userName.Length; i++)
                {
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - userName.Length / 2 + i, pcPos.y - 1));
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
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - message.Length / 2 + i, pcPos.y - 2));
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
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - userName.Length / 2 + i, pcPos.y - 1));
                    if(temp != null)
                    {
                        if(temp.tEntity == null)
                        {
                            temp.tEntity = new world.tileEntities.tileEntity(userName.ToCharArray()[i], false);
                        }
                    }
                    
                }

                for (int i = 0; i < message.Length; i++)
                {
                    Tile temp = Program.getWorld().getTile(new vector2(pcPos.x - message.Length / 2 + i, pcPos.y - 2));

                    if (temp != null)
                    {
                        if (temp.tEntity == null)
                        {
                            temp.tEntity = new world.tileEntities.tileEntity(message.ToCharArray()[i], false);
                        }
                    }
                }

                Tile pcLoc = Program.getWorld().getTile(pcPos);

                if (pcLoc != null)
                {
                    if (pcLoc.tEntity == null)
                    {
                        pcLoc.tEntity = new world.tileEntities.tileEntity('@', false);
                    }
                }
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

    public enum inputMode
    {
        movementMode,
        textMode
    }
}
