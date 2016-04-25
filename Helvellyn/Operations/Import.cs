using Helvellyn.Operations;
using Scallop;
using Sloth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class Import : IOperation
    {
        private static Logger logger = Logger.GetLogger(typeof(Import));

        public string Command { get { return "import"; } }

        public void Process(string[] args)
        {
            if (args == null) logger.Warn("Import requires at least one argument.");

            if (args[1] == "-f") importFile(args[2]);
            else if (args[1] == "-d") importDirectory(args[2]);
            else throw new Exception("Unknown flag");
        }

        private static void importDirectory(string directory)
        {
            logger.Info("Loading transactions from {0}", directory);
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
            logger.Info("Found {0} transactions.", transactions.Count);
        }
    }
}
