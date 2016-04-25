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
        public static IDataStore DataStore { get; set; }

        static void Main(string[] args)
        {
            Logger.MinLevel = Level.INFO;

            DataStore = DataManager.LoadDataStore();
            try
            {
                if (args.Length  == 0) displayHelp();
                string command = args[0];

                IOperation[] operations = 
                {
                    new Import(),
                    new AddTag(),
                    new ListTags(),
                };

                IOperation operation = null;
                for (int i = 0; i < operations.Length; i++) if (operations[i].Command == command) operation = operations[i];
                if (operation == null) throw new Exception(String.Format("Unknown operation of [{0}]", command));
                operation.Process(args);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                Console.ReadLine();
            }
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

            logger.Info("Help!");

            Console.ReadLine();
        }

        static void list(string[] args)
        {

        }

        

        static void sumMonth(params string[] args)
        {
            logger.Info("Summing month [{0}]", args[0]);
            IList<Transaction> transactions = getMonth(args[0], args[1]);

            double total = (from transaction in transactions select transaction.Value).Sum();
            
            logger.Info("Found {0} transactions", transactions.Count);
            logger.Info("Total value of {0}", total);
        }
        static void sumWeek(params string[] args)
        {
            logger.Info("Summing week [{0}] ", args[0]);
            IList<Transaction> transactions = getWeek(args[0], args[1]);

            double total = (from transaction in transactions select transaction.Value).Sum();

            logger.Info("Found {0} transactions", transactions.Count);
            logger.Info("Total value of {0}", total);
        }

        static void updateTags(IList<Transaction> transactions)
        {
            IList<Tag> tags = DataStore.GetAllTags();
            Dictionary<Tag, Regex> regexs = new Dictionary<Tag, Regex>();
            foreach (Tag tag in tags) regexs[tag] = new Regex(tag.Pattern);

            foreach(Transaction transaction in transactions)
            {
                foreach(Tag tag in tags)
                {
                    Match match = regexs[tag].Match(transaction.Description);
                    if (match.Success) transaction.Tag = tag.Name;
                }
            }
        }
        static void displayTransactions(IList<Transaction> transactions)
        {
            if (transactions.Count == 0) logger.Warn("No transactions.");
            ((List<Transaction>)transactions).Sort((a, b) => a.Date.CompareTo(b.Date));
            foreach (Transaction transaction in transactions) logger.Info(transaction.ToShortString());
        }
    }
}
