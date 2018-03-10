using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.client
{
    public static class clientManager
    {
        public const string csvPath = "clientData/userList.csv";

        private static Dictionary<string, clientInfo> clients;

        public static clientInfo userBuilder(StreamReader sr, StreamWriter sw)
        {
            loadClients();

            sw.WriteLine("LOG-IN...");
            sw.WriteLine("_______________________________________________________");
            sw.WriteLine("REMEMBER!!!!!!! ALL PASSCODES ARE NOT PASSWORDS.");
            sw.WriteLine("ALL PASSCODES ARE NOT STORED SECURELY");
            sw.WriteLine("DO NOT USE ANY PASSWORD YOU USE AS YOUR PASSCODE!!!!");
            sw.WriteLine("_______________________________________________________");
            sw.WriteLine("Enter username: ");
            sw.WriteLine("<end>");

            string userName = getString(sr, sw);

            clientInfo cInfo;
            if(clients.TryGetValue(userName, out cInfo))
            {
                sw.WriteLine("Enter passcode: ");
                sw.WriteLine("<end>");

                string passcode = getString(sr, sw);
                if(cInfo.checkPasscode(passcode))
                {
                    sw.WriteLine("ACESSS GRANTED.");
                }
                else
                {
                    sw.WriteLine("INVALID PASSWORD DISCONNECTING");
                    sw.WriteLine("<quit>");
                    sw.WriteLine("<end>");
                }

                return cInfo;
            }
            else
            {
                sw.WriteLine("Username not found.");
                sw.WriteLine("Enter passcode for new user " + userName + ":");
                sw.WriteLine("<end>");

                string passcode = getString(sr, sw);
                cInfo = new clientInfo(userName, passcode);
                addClient(cInfo);
                saveCSV();
                return cInfo;
            }
        }

        public static void saveCSV()
        {
            utils.csvWriter csv = new utils.csvWriter(csvPath);
            List<KeyValuePair<string, clientInfo>> clientList = clients.ToList();

            foreach(KeyValuePair<string, clientInfo> cKVP in clientList)
            {
                csv.add(cKVP.Value.ToString());
            }

            csv.write();
        }

        private static void addClient(clientInfo cInfo)
        {
            if(!clients.ContainsKey(cInfo.userName))
            {
                clients.Add(cInfo.userName, cInfo);
            }
        }

        private static clientInfo diskBuilder(string line)
        {
            string[] splits = line.Split(',');

            if (splits.Length == 2)
            {
                string userName = splits[0];
                string passcode = splits[1];
                return new clientInfo(userName, passcode);
            }
            else
            {
                return null;
            }
        }

        private static string getString(StreamReader sr, StreamWriter sw)
        {
            string message = "";
            while (true)
            {
                message = sr.ReadLine();
                if (message == null)
                {
                    sw.WriteLine("ERRRRR");
                    sw.WriteLine("<end>");
                }
                else
                {
                    break;
                }
            }
            return message;
        }

        private static void loadClients()
        {
            if (clients == null)
            {
                clients = new Dictionary<string, clientInfo>();
                using (utils.csvReader userList = new utils.csvReader(csvPath))
                {
                    string line = userList.readLine();
                    while (line != null)
                    {
                        clientInfo temp = diskBuilder(line);
                        if (temp != null)
                        {
                            addClient(temp);
                        }
                        line = userList.readLine();
                    }
                }
            }
        }
    }
}
