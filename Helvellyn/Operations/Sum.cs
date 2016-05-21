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
    public class Sum : IOperation
    {
        private static Logger logger = Logger.GetLogger(typeof(Sum));

        public string FullCommand { get { return "sum"; } }
        public string ShortCommand { get { return "s"; } }
        public string Description { get { return "add a tag into the database"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private IArgument[] arguments;
        
        public Sum()
        {
            arguments = new IArgument[]
            {
                
            };
        }

        public void Process()
        { }


        static void sumMonth(string[] args)
        {
            logger.Info("Summing month [{0}]", args[0]);

            string monthString = args[3];
            string yearString = args[4];

            DateTime month = DateTime.ParseExact(monthString, "MMM", CultureInfo.CurrentCulture);
            int year = Int32.Parse(yearString);
            DateTime start = new DateTime(year, month.Month, 1);
            DateTime end = start.AddMonths(1).Subtract(TimeSpan.FromDays(1));
            IList<Transaction> transactions = Program.DataStore.GetTransactionsBetween(start, end);

            double total = (from transaction in transactions select transaction.Value).Sum();

            logger.Info("Found {0} transactions", transactions.Count);
            logger.Info("Total value of {0}", total);
        }
        static void sumWeek(string[] args)
        {
            //logger.Info("Summing week [{0}] ", args[0]);
            //IList<Transaction> transactions = getWeek(args[0], args[1]);

            //double total = (from transaction in transactions select transaction.Value).Sum();

            //logger.Info("Found {0} transactions", transactions.Count);
            //logger.Info("Total value of {0}", total);
        }

    }
}
