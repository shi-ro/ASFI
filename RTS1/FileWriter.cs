using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTS1
{
    public class FileWriter
    {
        private string _path = "";
        private string _r = Path.GetPathRoot(Environment.SystemDirectory);
        public FileWriter(string path, string name, string ext)
        {
            _path = $@"{_r}{path}\{name}.{ext}";
            if (!File.Exists($"{_r}{path}"))
            {
                Directory.CreateDirectory($"{_r}{path}");
                Write("");
            }
        }
        public string Read()
        {
            return File.ReadAllText(_path);
        }

        public void Write(string value)
        {
            File.WriteAllText(_path, value);
        }
    }
}
