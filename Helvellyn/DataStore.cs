using Sloth;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class DataStore : AccessDatabase, IDataStore
    {
        private static Logger logger = Logger.GetLogger(typeof(Logger));
        public const string filepath = "data.mbd";


        public IList<Transaction> GetTransactionsBetween(DateTime start, DateTime end)
        {
            logger.Debug("Getting transactions.");
            string command = "SELECT * FROM " + Transaction.TABLE_NAME +
                " WHERE [" + Transaction.COL_DATE + "] BETWEEN #" + start.ToString("yyyy/MM/dd") + "# AND #" + end.ToString("yyyy/MM/dd") + "#;";
            IList<string[]> result = readData(filepath, command);
            return (from trans in result select Transaction.Parse(trans)).ToList();
        }
        public IList<Transaction> GetAllTransactions()
        {
            logger.Debug("Getting all transactions.");
            string command = "SELECT * FROM " + Transaction.TABLE_NAME;
            IList<string[]> result = readData(filepath, command);
            return (from trans in result select Transaction.Parse(trans)).ToList();
        }
        public void RecordTransactions(IList<Transaction> transactions)
        {
            logger.Debug("Recording transactions.");
            bool isSetup = isDatabaseSetup();
            if (!isSetup) setupDataBase();
            recordTransactions(transactions);
        }


        private static bool isDatabaseSetup()
        {     
            if (!File.Exists(filepath)) return false;
            return true;
        }
        private static void setupDataBase()
        {
            logger.Debug("Setting up data store.");
            createDatabase(filepath);
            string[] columns = new string[Transaction.COLUMNS.Length];
            for (int i = 0; i < columns.Length; i++) columns[i] = String.Format("[{0}] {1}", Transaction.COLUMNS[i], Transaction.COLUMN_TYPES[i]);
            addTable(filepath, "transactions", columns);
        }
        private static void recordTransactions(IList<Transaction> transactions)
        {
            foreach(Transaction transaction in transactions) recordTransaction(transaction);
        }
        private static void recordTransaction(Transaction transaction)
        {
            logger.Debug("Recording transaction.");

            if (!checkUniqueTransaction(transaction)) return;

            object[] rawValues = transaction.getValues(Transaction.COLUMNS);
            string[] values = new string[rawValues.Length];
            for(int i=0;i<rawValues.Length;i++) values[i] = rawValues[i].ToString();

            insert(filepath, Transaction.TABLE_NAME, Transaction.COLUMNS, values);
        }
        private static bool checkUniqueTransaction(Transaction transaction)
        {
            logger.Debug("Checking if transaction is unique.");
            StringBuilder command = new StringBuilder();
            command.Append("SELECT * FROM " + Transaction.TABLE_NAME + " WHERE");

            for(int i=0;i<Transaction.COLUMNS.Length;i++)
            {
                string property = Transaction.COLUMNS[i];
                string type = Transaction.COLUMN_TYPES[i];
                object value = transaction.getValue(property);

                logger.Trace("Checking {0} [{1}] is {2}", property, type, value);

                command.Append(" [" + property + "] = ");
                if (type == Transaction.TYPE_TEXT) command.Append("'" + value.ToString().Replace('\'', '$') + "'");
                else if (type == Transaction.TYPE_DATE) command.Append("#" + ((DateTime)value).ToString("yyyy/MM/dd") + "#");
                else if (type == Transaction.TYPE_DOUBLE) command.Append(value.ToString());

                if (i != Transaction.COLUMNS.Length-1) command.Append(" AND");
            }
            command.Append(";");
            IList<string[]> result = readData(filepath, command.ToString());

            if (result.Count > 0)
            {
                logger.Warn("Transaction already in database.");
                return false;
            }
            else return true;
        }
    }
}
