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

        public string FullCommand { get { return "--import"; } }
        public string ShortCommand { get { return "-i"; } }
        public string Description { get { return "add a tag into the database"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private string Source { get; set; }
        private bool IsDirectory { get; set; }
        private bool IsTags { get; set; }

        private IArgument[] arguments;

        public Import()
        {
            arguments = new IArgument[]
            {
                new Argument<string>("-source", v => Source = (string)v),
                new Argument<bool>("-directory", v => IsDirectory = (bool)v),
                new Argument<bool>("-tags", v => IsTags = (bool)v),
            };
        }

        public void Process()
        {
            if (IsTags) importTags(Source);
            else if (IsDirectory) importDirectory(Source);
            else importFile(Source);
        }

        private static void importTags(string filename)
        {
            string[] data = File.ReadAllLines(filename);
            foreach (string line in data)
            {
                string[] tag = line.Split(',');
                Program.DataStore.RecordTag(new Tag(tag[0], tag[1]));
            }
        }
        private static void importDirectory(string directory)
        {
            logger.Info("Loading transactions from directory [{0}]", directory);
            DirectoryInfo info = new DirectoryInfo(directory);
            foreach(FileInfo fileInfo in info.GetFiles())
            {
                if (fileInfo.Extension != ".csv") continue;
                importFile(fileInfo.FullName);
            }
        }
        private static void importFile(string filename)
        {
            logger.Info("Loading transactions from file [{0}]", filename);
           
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
