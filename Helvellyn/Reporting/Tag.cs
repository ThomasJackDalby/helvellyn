using Scallop;
using Sloth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class Tag
    {
        private static Logger logger = Logger.GetLogger(typeof(Logger));

        public string Name { get { return name; } }
        public string Pattern { get { return pattern; } }

        public readonly string pattern;
        public readonly string name;

        public const string TABLE_NAME = "TAGS";
        public const string COL_NAME = "name";
        public const string COL_PATTERN = "pattern";

        public const string TYPE_TEXT = "TEXT";
        public static readonly string[] COLUMNS = { COL_NAME, COL_PATTERN };
        public static readonly string[] COLUMN_TYPES = { TYPE_TEXT, TYPE_TEXT };

        public Tag(string name, string pattern)
        {
            this.name = name;
            this.pattern = pattern;
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
            switch (property)
            {
                case COL_NAME: return Name;
                case COL_PATTERN: return Pattern;
                default: throw new Exception("Unknown transaction property " + property);
            }
        }

        public static Tag Parse(string[] data)
        {
            logger.Debug("Parsing transaction.");
            logger.Trace(String.Join(", ", data));

            try
            {
                return new Tag(data[0].Trim('\''), data[1].Trim('\''));
            }
            catch (Exception e)
            {
                logger.Error("Unable to parse transacion");
                logger.Trace(String.Join(", ", data));
                logger.Trace(e.Message);
                return null;
            }
        }   
    }
}
