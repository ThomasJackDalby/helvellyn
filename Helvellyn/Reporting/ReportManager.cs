using Sloth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Helvellyn
{
    public static class ReportManager
    {
        private static Logger logger = Logger.GetLogger(typeof(ReportManager));

        public static void Process(string[] args)
        {
            average();
        }

        private static void sum()
        {
            TimeSpan span = TimeSpan.FromDays(7);

            // Get the start and finish date time
            IList<Transaction> transactions = Program.DataStore.GetAllTransactions();

            DateTime start = (from t in transactions select t.Date).Min();
            DateTime end = (from t in transactions select t.Date).Max();

            // sum up the intervals
            double currentSum = 0;
            TimeSpan currentSpan = TimeSpan.Zero;

            DateTime currentStart = start;
            for (DateTime day = start; day < end; day = day.AddDays(1))
            {
                currentSum += (from t in transactions where t.Date.Equals(day) select t.Value).Sum();
                currentSpan = currentSpan.Add(TimeSpan.FromDays(1));
                if (currentSpan >= span)
                {
                    logger.Info("{0} - {1} : {2}", currentStart, day, currentSum);
                    currentSum = 0;
                    currentStart = day;
                    currentSpan = TimeSpan.Zero;
                }
            }
        }
        private static void average()
        {
            TimeSpan averageSpan = TimeSpan.FromDays(1);
            TimeSpan reportingSpan = TimeSpan.FromDays(1);

            // Get the start and finish date time
            IList<Transaction> transactions = Program.DataStore.GetAllTransactions();

            DateTime start = (from t in transactions select t.Date).Min();
            DateTime end = (from t in transactions select t.Date).Max();

            // sum up the intervals
            List<double> sums = new List<double>();
            double currentSum = 0;
            TimeSpan currentSpan = TimeSpan.Zero;
            TimeSpan currentReportSpan = TimeSpan.Zero;

            DateTime currentStart = start;
            for (DateTime day = start; day < end; day = day.AddDays(1))
            {
                currentSum += (from t in transactions where t.Date.Equals(day) select t.Value).Sum();
                currentSpan = currentSpan.Add(TimeSpan.FromDays(1));
                currentReportSpan = currentReportSpan.Add(TimeSpan.FromDays(1));

                if (currentSpan >= averageSpan)
                {
                    sums.Add(currentSum);
                    currentSum = 0;
                    currentSpan = TimeSpan.Zero;
                }

                if (currentReportSpan >= reportingSpan)
                {
                    double avg = sums.Average();
                    string format = "{0:dd/MMM/yyyy} | {1:dd/MMM/yyyy}  £{2,8:N2}";

                    if (avg >= 0) logger.Info(format, currentStart, day, sums.Average());
                    else logger.Info(format, ConsoleColor.Red, currentStart, day, sums.Average());
                    currentReportSpan = TimeSpan.Zero;
                    currentStart = day;
                    sums.Clear();
                }
            }

            
        }

        private static void processRegex()
        {
            IList<Tag> tags = Program.DataStore.GetAllTags();
            IList<Transaction> transactions = Program.DataStore.GetAllTransactions();



        }

        private static IList<Transaction> getMatches(IList<Transaction> transactions, string pattern)
        {
            Regex regex = new Regex(pattern);

            IList<Transaction> matches = new List<Transaction>();
            foreach(Transaction transaction in transactions)
            {
                Match result = regex.Match(transaction.Description);

                if (result.Success) matches.Add(transaction);
            }

            return matches;
        }
    }
}
