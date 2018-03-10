using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world0Server.utils
{
    public class csvWriter
    {
        public string fullPath;

        private List<String> lines;

        public csvWriter(string path)
        {
            fullPath = Program.pathStart + path;
            lines = new List<string>();
        }

        public void add(string s)
        {
            lines.Add(s);
        }

        public bool write()
        {
            Console.WriteLine("WARNGING SLOW -> saving CSV: " + fullPath);

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath))
                {
                    foreach (string s in lines)
                    {
                        sw.WriteLine(s);
                    }
                    sw.Flush();
                    sw.Dispose();
                    sw.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("This file could not be saved: " + fullPath);
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
