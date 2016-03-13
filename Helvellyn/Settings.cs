using Sloth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public static class Settings
    {
        public const string filename = "config.ini";

        public static Level MinLevel { get; set; }

        static Settings()
        {
            if (!File.Exists(filename)) File.Create(filename);

            string[] lines = File.ReadAllLines(filename);
            for(int i=0;i<lines.Length;i++)
            {
                string[] setting = lines[i].Trim().Split('=');
                string key = setting[0];
                string value = setting[1];

                if (key == "logger_level") MinLevel = (Level)Enum.Parse(typeof(Level), value);
            }
        }

        public static void Set(string key, string value)
        { }
    }
}
