using Scallop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class RemoveTag : IOperation
    {
        public string FullCommand { get { return "remove-tag"; } }
        public string ShortCommand { get { return "rt"; } }
        public string Description { get { return "remove a tag from the database"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private IArgument[] arguments;

        public RemoveTag()
        {
            arguments = new IArgument[]
            {
                
            };
        }

        public void Process()
        {
            //string name = args[1];
            //Program.DataStore.RemoveTag(name);
        }
    }
}
