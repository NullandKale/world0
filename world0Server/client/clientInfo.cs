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
        public clientMode mode = clientMode.textMode;
        public string userName;
        private string passcode;

        public clientInfo(string userName, string passcode)
        {
            this.userName = userName;
            this.passcode = passcode;
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
