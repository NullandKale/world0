using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.utils
{
    public class csvReader : IDisposable
    {
        private StreamReader sr;
        public csvReader(string path)
        {
            try
            {
                sr = new StreamReader(Program.pathStart + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("This file could not be read: " + path);
                Console.WriteLine(e.Message);
            }
        }

        public string[] readSplitLine()
        {
            string line = sr.ReadLine();

            if(line == null)
            {
                sr.Close();
                return null;
            }
            else
            {
                string[] splits = line.Split(',');
                if(splits != null)
                {
                    return splits;
                }
                else
                {
                    sr.Dispose();
                    sr.Close();
                    return null;
                }
            }
        }

        public string readLine()
        {
            return sr.ReadLine();
        }

        public void Dispose()
        {
            sr.Dispose();
        }
    }
}
