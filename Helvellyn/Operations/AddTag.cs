using Scallop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class AddTag : IOperation
    {
        public string FullCommand { get { return "add-tag"; } }
        public string ShortCommand { get { return "at"; } }
        public string Description { get { return "add a tag into the database"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private IArgument[] arguments;

        public AddTag()
        {
            arguments = new IArgument[]
            {

            };
        }

        public void Process()
        {
            //if (args.Length < 3) throw new Exception("Need 3 arguments.");

            //string name = args[1];
            //string pattern = args[2];
            //Tag tag = new Tag(name, pattern);
            //Program.DataStore.RecordTag(tag);
        }
    }
}
