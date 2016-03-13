using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sloth
{
    public class Logger
    {
        private Type type;

        public static Level MinLevel = Level.INFO;

        private Logger(Type _type)
        {
            type = _type;
        }

        public void Error(string message, params object[] inputs)
        {
            write(type, ConsoleColor.Red, Level.ERROR, message, inputs);
        }
        public void Warn(string message, params object[] inputs)
        {
            write(type, ConsoleColor.Yellow, Level.WARN, message, inputs);
        }
        public void Info(string message, params object[] inputs)
        {
            write(type, ConsoleColor.Gray, Level.INFO, message, inputs);
        }
        public void Debug(string message, params object[] inputs)
        {
            write(type, ConsoleColor.Blue, Level.DEBUG, message, inputs);
        }
        public void Trace(string message, params object[] inputs)
        {
            write(type, ConsoleColor.Cyan, Level.TRACE, message, inputs);
        }

        public static Logger GetLogger(Type type)
        {
            return new Logger(type);
        }

        private static void write(Type type, ConsoleColor colour, Level level, string text, params object[] inputs)
        {
            if (level < MinLevel) return;
            string content = String.Format(text, inputs);
            Console.ForegroundColor = colour;
            Console.WriteLine("{0,-5}: {1}", level, content);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
