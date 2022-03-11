using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedlix.Controller
{
    public static class IOHandler
    {
        public static string ReadFileToString(string fileName)
        {
            StreamReader r = new StreamReader(fileName);
            return r.ReadToEnd();
        }
        public static void WriteFileFromString(string data)
        {
            using (StreamWriter streamWriter = new StreamWriter("OutputData.json"))
            {
                streamWriter.WriteLine(data);
            }
        }
    }
}
