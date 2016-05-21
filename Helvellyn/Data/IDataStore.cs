using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public interface IDataStore
    {
        IList<Transaction> GetTransactionsMonths(DateTime start, int months);
        IList<Transaction> GetTransactions(DateTime start, TimeSpan duration);
        void RecordTransactions(IList<Transaction> transactions);
        void RecordTag(Tag tag);
        void RemoveTag(string name);
        IList<Transaction> GetTransactionsBetween(DateTime start, DateTime end);
        IList<Transaction> GetAllTransactions();
        IList<Tag> GetAllTags();
    }
}
