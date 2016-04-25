using Sloth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class ListTags : IOperation
    {
        private Logger logger = Logger.GetLogger(typeof(ListTags));
        public string Command { get { return "list-tags"; } }

        public void Process(string[] args)
        {
            IList<Tag> tags = Program.DataStore.GetAllTags();

            for (int i = 0; i < tags.Count; i++)
            {
                logger.Info("[{0}] [{1}]", tags[i].Name, tags[i].Pattern);
            }
        }
    }
}
