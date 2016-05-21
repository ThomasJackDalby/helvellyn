using Scallop;
using Sloth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class List : IOperation
    {
        private static Logger logger = Logger.GetLogger(typeof(List));

        public string FullCommand { get { return "--list"; } }
        public string ShortCommand { get { return "-l"; } }
        public string Description { get { return "add a tag into the database"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private IArgument[] arguments;

        public string Tag { get; set; }
        public string Scope { get; set; }
        public bool IsMonth { get; set; }

        public List()
        {
            arguments = new IArgument[]
            {
                new Argument<string>("-tag", v => Tag = (string)v),
                new Argument<string>("-scope", v => Scope = (string)v),
                new Argument<string>("-month", v => IsMonth = (bool)v),
            };
        }

        public void Process()
        {
            IList<Transaction> transactions;
            if (IsMonth)
            {
                string[] date = Scope.Split('-');
                DateTime month = DateTime.ParseExact(date[0], "MMM", CultureInfo.CurrentCulture);
                int year = Int32.Parse(date[1]);
                DateTime start = new DateTime(year, month.Month, 1);
                DateTime end = start.AddMonths(1).Subtract(TimeSpan.FromDays(1));
                transactions = Program.DataStore.GetTransactionsBetween(start, end);
            }
            else transactions = getAll();

            TagManager.Process(transactions);

            if (Tag == "all")
            {
                Transaction.Display(transactions);
            }
            else
            {
                IList<Transaction> filtered = (from transaction in transactions where transaction.Tag == Tag select transaction).ToList();
                Transaction.Display(filtered);
            }
        }


        static IList<Transaction> getAll()
        {
            IList<Transaction> transactions = Program.DataStore.GetAllTransactions();
            return transactions;
        }
        static IList<Transaction> getMonth(string monthString, string yearString)
        {
            DateTime month = DateTime.ParseExact(monthString, "MMM", CultureInfo.CurrentCulture);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, month.Month, 1);
            DateTime end = start.AddMonths(1).Subtract(TimeSpan.FromDays(1));
            logger.Debug("Getting transactions between {0} and {1}", start, end);
            IList<Transaction> transactions = Program.DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }
        static IList<Transaction> getWeek(string weekString, string yearString)
        {
            int week = Int32.Parse(weekString);
            int year = Int32.Parse(yearString);

            DateTime start = new DateTime(year, 1, 1).AddDays(7 * week);
            DateTime end = start.AddMonths(1);
            IList<Transaction> transactions = Program.DataStore.GetTransactionsBetween(start, end);
            return transactions;
        }

    }
}
