using Sloth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class List
    {
        private static Logger logger = Logger.GetLogger(typeof(List));

        public string Command { get { return "list";}}
        
        public void Process(string[] args)
        {
            logger.Debug("list function");
            string tag = args[1];
            string scope = args[2];
            string[] parameters = args.Skip(3).ToArray();

            logger.Debug("Params: {0}", parameters);
            IList<Transaction> transactions;
            if (scope == "-a" || scope == "--all") transactions = getAll();
            else if (scope == "-m" || scope == "--month") transactions = getMonth(parameters[0], parameters[1]);
            else throw new Exception("Not a valid scope.");

            updateTags(transactions);
            displayTransactions(transactions);
        }


        static IList<Transaction> getAll()
        {
            IList<Transaction> transactions = DataStore.GetAllTransactions();
            return transactions;
        }
        static IList<Transaction> getMonth(string monthString, string yearString)
        {
            DateTime month = DateTime.ParseExact(monthString, "MMM", CultureInfo.CurrentCulture);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, month.Month, 1);
            DateTime end = start.AddMonths(1).Subtract(TimeSpan.FromDays(1));
            logger.Debug("Getting transactions between {0} and {1}", start, end);
            IList<Transaction> transactions = DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }
        static IList<Transaction> getWeek(string weekString, string yearString)
        {
            int week = Int32.Parse(weekString);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, 1, 1).AddDays(7 * week);
            DateTime end = start.AddMonths(1);
            IList<Transaction> transactions = DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }
    }
}
