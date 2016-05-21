using Scallop;
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
        public string FullCommand { get { return "--list-tags"; } }
        public string ShortCommand { get { return "-lt"; } }
        public string Description { get { return "list all tags in the database"; } }
        public IArgument[] Arguments { get { return arguments; } }

        public IArgument[] arguments;

        public ListTags()
        {
            arguments = new IArgument[]
            {
            //    new Argument<string>("-source", v => Source = (string)v),
            //    new Argument<bool>("-directory", v => IsDirectory = (bool)v),
            //    new Argument<bool>("-tags", v => IsTags = (bool)v),
            };
        }

        public void Process()
        {
            IList<Tag> tags = Program.DataStore.GetAllTags();

            for (int i = 0; i < tags.Count; i++)
            {
                logger.Info("[{0}] [{1}]", tags[i].Name, tags[i].Pattern);
            }
        }
    }
}
