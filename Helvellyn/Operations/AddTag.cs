using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class AddTag : IOperation
    {
        public string Command { get { return "addtag"; } }

        public void Process(string[] args)
        {
            if (args.Length < 3) throw new Exception("Need 3 arguments.");

            string name = args[1];
            string pattern = args[2];
            Tag tag = new Tag { Name = name, Pattern = pattern };
            Program.DataStore.RecordTag(tag);
        }
    }
}
