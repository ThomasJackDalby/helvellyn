using Helvellyn.Operations;
using Scallop;
using Sloth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helvellyn
{
    static class Program
    {
        private static Logger logger = Logger.GetLogger(typeof(Logger));
        public static IDataStore DataStore { get { return dataStore; } }

        private static readonly IDataStore dataStore = DataManager.LoadDataStore();
        private static readonly IOperation[] operations = 
        {
            new Import(),
            new Export(),
            new List(),
            new AddTag(),
            new RemoveTag(),
            new ListTags(),
            new Graph(),
        };

        static Program()
        {
            Logger.MinLevel = Level.DEBUG;
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0) displayHelp();
                else processOperation(args);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }

        private static void processOperation(string[] args)
        {
            logger.Trace("Processing operation");
            string command = args[0];

            IOperation operation = operations.SingleOrDefault(o => o.FullCommand == command);
            if (operation == null)
            {
                logger.Error("Unknown operation of [{0}]", command);
                return;
            }

            try
            {
                Dictionary<IArgument, object> arguments = getArguments(operation, args);
                foreach (IArgument argument in arguments.Keys) argument.Set(arguments[argument]);
                operation.Process();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }

        private static Dictionary<IArgument, object> getArguments(IOperation operation, string[] args)
        {
            logger.Trace("Getting arguments");
            Dictionary<IArgument, object> arguments = new Dictionary<IArgument, object>();

            for(int index = 1;index<args.Length;index++)
            {
                string key = args[index];
                IArgument argument = operation.GetArgument(key);

                if (argument.ValueType == typeof(bool)) arguments[argument] = true;
                else
                {
                    string value = args[++index];
                    if (argument.ValueType == typeof(string)) arguments[argument] = value;
                    else if (argument.ValueType == typeof(double)) arguments[argument] = Double.Parse(value);
                    else if (argument.ValueType == typeof(int)) arguments[argument] = Int32.Parse(value);
                    else throw new Exception(String.Format("Argument type of [{0}] not supported", argument.ValueType));
                }
            }

            // any requirements not set, should be set to their default value
            foreach(IArgument argument in operation.Arguments)
            {
                if (!arguments.Keys.Contains(argument)) 
                {
                    if (argument.Required) throw new Exception(String.Format("Value not provided for [{0}] which is required.", argument));
                    arguments[argument] = argument.DefaultValue;
                }
            }

            foreach (IArgument arg in arguments.Keys) logger.Debug("{0} : {1}", arg.Key, arguments[arg]);

            return arguments;
        }



        private static void setSetting(params string[] args)
        {
            string key = args[0];
            string value = args[1];
            Settings.Set(key, value);
        }
        private static void displayHelp()
        {
            logger.Info("{0} v{1}", Identity.Name, Identity.Version);

            foreach(IOperation operation in operations)
            {
                logger.Info("{0} : {1}", operation.FullCommand, operation.Description);
                foreach (IArgument argument in operation.Arguments) logger.Info("    {0} : {1}", argument.Key, argument.Description);
                logger.Info("");
            }
        }
    }
}
