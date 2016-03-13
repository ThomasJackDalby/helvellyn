using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public interface IDataStore
    {
        void RecordTransactions(IList<Transaction> transactions);
        IList<Transaction> GetTransactionsBetween(DateTime start, DateTime end);
        IList<Transaction> GetAllTransactions();
    }
}
