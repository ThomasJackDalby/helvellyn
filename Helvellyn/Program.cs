using Scallop;
using Sloth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    static class Program
    {
        private static Logger logger = Logger.GetLogger(typeof(Logger));
        public static IDataStore DataStore { get; set; }

        static void Main(string[] args)
        {
            Logger.MinLevel = Level.TRACE;

            DataStore = DataManager.LoadDataStore();
            try
            {
                processMenu(args);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                Console.ReadLine();
            }
        }

        private static void processMenu(string[] args)
        {
            List<Option> options = new List<Option>();
            options.Add(new Option("import", a => ImportManager.Import(a), "read in and process csv files containing transaction information"));
            options.Add(new Option("export", a => ExportManager.Process(a)));
            options.Add(new Option("report", a => ReportManager.Process(a)));
            options.Add(new Option("list", a => list(a)));
            options.Add(new Option("graph", a => GraphManager.Process(a)));
            Menu.Parse(args, options);
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

        private static void import(params string[] args)
        {
            
        }
        private static void list(params string[] args)
        {
            if (args.Length == 0) throw new ArgumentException("Need more arguments");
            string scope = args[0];

            if (scope == "all") listAll(args.Skip(1).ToArray());
            else if (scope == "month") listMonth(args.Skip(1).ToArray());
            else throw new Exception("Not a list option.");
        }

        private static void sum(params string[] args)
        {



        }

        private static IList<Transaction> getAll()
        {
            IList<Transaction> transactions = DataStore.GetAllTransactions();
            return transactions;
        }
        private static IList<Transaction> getMonth(string monthString, string yearString)
        {
            DateTime month = DateTime.ParseExact(monthString, "MMM", CultureInfo.CurrentCulture);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, month.Month, 1);
            DateTime end = start.AddMonths(1);
            IList<Transaction> transactions = DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }
        private static IList<Transaction> getWeek(string weekString, string yearString)
        {
            int week = Int32.Parse(weekString);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, 1, 1).AddDays(7 * week);
            DateTime end = start.AddMonths(1);
            IList<Transaction> transactions = DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }



        private static void listMonth(params string[] args)
        {
            IList<Transaction> transactions = getMonth(args[0], args[1]);
            listTransactions(transactions);
        }
        private static void listAll(params string[] args)
        {
            IList<Transaction> transactions = getAll();
            listTransactions(transactions);
        }

        private static void sumMonth(params string[] args)
        {
            logger.Info("Summing month [{0}]", args[0]);
            IList<Transaction> transactions = getMonth(args[0], args[1]);

            double total = (from transaction in transactions select transaction.Value).Sum();
            
            logger.Info("Found {0} transactions", transactions.Count);
            logger.Info("Total value of {0}", total);
        }
        private static void sumWeek(params string[] args)
        {
            logger.Info("Summing week [{0}] ", args[0]);
            IList<Transaction> transactions = getWeek(args[0], args[1]);

            double total = (from transaction in transactions select transaction.Value).Sum();

            logger.Info("Found {0} transactions", transactions.Count);
            logger.Info("Total value of {0}", total);
        }






        private static void listTransactions(IList<Transaction> transactions)
        {
            if (transactions.Count == 0) logger.Warn("No transactions.");
            foreach (Transaction transaction in transactions) logger.Info(transaction.ToShortString());
        }
    }
}
