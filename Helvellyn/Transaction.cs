using Sloth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class Transaction
    {
        private static Logger logger = Logger.GetLogger(typeof(Logger));

        public string ID { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public string Balance { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Tag { get; set; }

        public const string TABLE_NAME = "transactions";

        public const string COL_DATE = "_date";
        public const string COL_TYPE = "type";
        public const string COL_TAG = "tag";
        public const string COL_DESCRIPTION = "description";
        public const string COL_VALUE = "value";
        public const string COL_BALANCE = "balance";
        public const string COL_ACCOUNT_NAME = "account_name";
        public const string COL_ACCOUNT_NUMBER = "account_number";

        public const string TYPE_TEXT = "TEXT";
        public const string TYPE_DATE = "DATE";
        public const string TYPE_DOUBLE = "DOUBLE";

        public static readonly string[] COLUMNS = { COL_DATE, COL_TAG, COL_TYPE, COL_DESCRIPTION, COL_VALUE, COL_BALANCE, COL_ACCOUNT_NAME, COL_ACCOUNT_NUMBER };
        public static readonly string[] COLUMN_TYPES = { TYPE_DATE, TYPE_TEXT, TYPE_TEXT, TYPE_TEXT, TYPE_DOUBLE, TYPE_TEXT, TYPE_TEXT, TYPE_TEXT };

        public Transaction()
        {
            Tag = "Unset";
        }

        public object[] getValues(string[] columns)
        {
            logger.Debug("Getting raw data...");
            object[] values = new object[columns.Length];
            for (int i = 0; i < values.Length; i++) values[i] = getValue(columns[i]);
            logger.Debug("Got raw data.");
            return values;
        }

        public object getValue(string property)
        {
            logger.Trace("Getting {0}", property);
            switch(property)
            {
                case COL_DATE: return Date;
                case COL_TYPE: return Type;
                case COL_TAG: return Tag;
                case COL_DESCRIPTION: return Description;
                case COL_VALUE: return Value;
                case COL_BALANCE: return Balance;
                case COL_ACCOUNT_NAME: return AccountName;
                case COL_ACCOUNT_NUMBER: return AccountNumber;
                default: throw new Exception("Unknown transaction property " + property);
            }
        }
        public string getSQLValue(string property)
        {
            logger.Trace("Getting {0}", property);
            switch (property)
            {
                case COL_DATE: return "#" + Date.ToString("yyyyMMdd") + "#";
                case COL_TYPE: return Type;
                case COL_DESCRIPTION: return Description;
                case COL_VALUE: return Value.ToString();
                case COL_BALANCE: return Balance;
                case COL_ACCOUNT_NAME: return AccountName;
                case COL_ACCOUNT_NUMBER: return AccountNumber;
                default: throw new Exception("Unknown transaction property " + property);
            }
        }
        public void setValue(string property, string value)
        {
            switch (property)
            {
                case COL_DATE: Date = DateTime.Parse(value); break;
                case COL_TYPE: Type = value.ToString(); break;
                case COL_DESCRIPTION: Description = value; break;
                case COL_VALUE: Value = Double.Parse(value); break;
                case COL_BALANCE: Balance = value; break;
                case COL_ACCOUNT_NAME: AccountName = value; break;
                case COL_ACCOUNT_NUMBER: AccountNumber = value; break;
                default: throw new Exception("Unknown transaction property " + property);
            }
        }

        public static Transaction Parse(string[] data)
        {
            logger.Debug("Parsing transaction.");
            logger.Trace(String.Join(", ", data));

            try
            {
                Transaction transaction = new Transaction();
                transaction.Date = DateTime.Parse(data[0]);
                transaction.Type = data[1].Trim('\'');
                transaction.Description = data[2].Trim('\'');
                transaction.Value = Double.Parse(data[3]);
                transaction.Balance = data[4].Trim('\'');
                transaction.AccountName = data[5].Trim('\'');
                transaction.AccountNumber = data[6].Trim('\'');
                return transaction;
            }
            catch(Exception e)
            {
                logger.Error("Unable to parse transacion");
                logger.Trace(String.Join(", ", data));
                logger.Trace(e.Message);
                return null;
            }
        }


        public string ToShortString()
        {
            return String.Format("[{0:yyyy-MM-dd}] {1,8:#.00} : {2}", Date, Value, Description);
        }
    }
}
