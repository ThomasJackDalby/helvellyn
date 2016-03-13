using Sloth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public static class ImportManager
    {
        private static Logger logger = Logger.GetLogger(typeof(ImportManager));

        public static void Import(params string[] args)
        {
            if (args.Length < 2) throw new Exception("Not enough arguments");

            if (args[0] == "-f") importFile(args[1]);
            else if (args[0] == "-d") importDirectory(args[1]);
        }

        private static void importDirectory(string directory)
        {
            DirectoryInfo info = new DirectoryInfo(directory);
            foreach(FileInfo fileInfo in info.GetFiles())
            {
                if (fileInfo.Extension != ".csv") continue;
                importFile(fileInfo.FullName);
            }
        }

        private static void importFile(string filename)
        {
            logger.Info("Loading transactions from {0}", filename);
           
            CsvFileReader reader = new CsvFileReader(filename, EmptyLineBehavior.Ignore);

            IList<Transaction> transactions = new List<Transaction>();
            List<string> columns = new List<string>();
            bool isFirst = true;
            while(reader.ReadRow(columns))
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                transactions.Add(Transaction.Parse(columns.ToArray()));
            }
            Program.DataStore.RecordTransactions(transactions.ToList());
        }
    }
}
