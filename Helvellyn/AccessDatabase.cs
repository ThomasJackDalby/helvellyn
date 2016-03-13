using Sloth;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class AccessDatabase
    {
        private static Logger logger = Logger.GetLogger(typeof(AccessDatabase));

        protected static void createDatabase(string filepath)
        {
            ADOX.CatalogClass cat = new ADOX.CatalogClass();
            string command = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";" + "Jet OLEDB:Engine Type=5";
            logger.Trace(command);
            cat.Create(command);
            cat = null;
        }
        protected static void executeNonQuery(string filepath, string query)
        {
            OleDbConnection connection = getConnection(filepath);
            executeNonQuery(filepath, query, connection);
        }
        protected static void executeNonQuery(string filepath, string query, OleDbConnection connection)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = query;
            logger.Trace(command.CommandText);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }
        protected static void addTable(string filepath, string table, string[] columns)
        {
            string command = String.Format("CREATE TABLE {0}({1});", table, String.Join(", ", columns));   
            executeNonQuery(filepath, command);
        } 
        protected static void insert(string filepath, string table, string[] columns, string[] values)
        {
            logger.Debug("Inserting record into database.");
            for (int i = 0; i < values.Length; i++) values[i] = values[i].Replace('\'', ' ');
            string command = String.Format("INSERT INTO {0}([{1}]) VALUES('{2}');",
                table, String.Join("], [", columns), String.Join("', '", values));
            executeNonQuery(filepath, command);
        }
        protected static IList<string[]> readData(string filepath, string query)
        {
            OleDbConnection connection = getConnection(filepath);
            
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = query;
            logger.Trace(command.CommandText);

            List<string[]> queryResult = new List<string[]>();

            connection.Open();
            OleDbDataReader reader = command.ExecuteReader();                    
            while (reader.Read()) 
            {
                string[] row = new string[reader.FieldCount];
                for(int i=0;i<row.Length;i++) row[i] = reader[i].ToString();
                queryResult.Add(row);
            }
            connection.Close();
            reader.Dispose();
            return queryResult;
        }
        protected static OleDbConnection getConnection(string filepath)
        {
            return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filepath);
        }
    }
}
