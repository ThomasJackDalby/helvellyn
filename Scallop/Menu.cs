using Sloth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scallop
{
    public static class Menu
    {
        private static Logger logger = Logger.GetLogger(typeof(Menu));

        public const string KEY_HELP = "?";

        public static void Parse(string[] args, IList<Option> options)
        {
            if (args.Length == 0) displayHelp(options);
            else
            {
                string key = args[0];
                string[] optionArgs = args.Skip(1).ToArray();

                Option option = options.SingleOrDefault(o => o.Key.Equals(key));
                if (option != null) option.Method(optionArgs);
                else if (key.Equals(KEY_HELP)) displayHelp(options);
                else throw new Exception("Unknown option given");
            }
        }

        public static void displayHelp(IList<Option> options)
        {
            logger.Info("Menu options");
            foreach(Option option in options) logger.Info(" {0} : {1}", option.Key, option.Description);
        }
    }

}
