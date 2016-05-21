
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class TagManager
    {
        public static void Process(IList<Transaction> transactions)
        {
            IList<Tag> tags = Program.DataStore.GetAllTags();
            Dictionary<Tag, Regex> regexs = new Dictionary<Tag, Regex>();
            foreach (Tag tag in tags) regexs[tag] = new Regex(tag.Pattern);

            foreach (Transaction transaction in transactions)
            {
                foreach (Tag tag in tags)
                {
                    Match match = regexs[tag].Match(transaction.Description);
                    if (match.Success) transaction.Tag = tag.Name;
                }
            }
        }
    }
}
