using Scallop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn.Operations
{
    public class Export : IOperation
    {
        public string FullCommand { get { return "export"; } }
        public string ShortCommand { get { return "e"; } }
        public string Description { get { return "export tags to a csv file"; } }
        public IArgument[] Arguments { get { return arguments; } }

        private IArgument[] arguments;

        public Export()
        {
            arguments = new IArgument[]
            {
                
            };
        }

        public void Process()
        {
            IList<Tag> alltags = Program.DataStore.GetAllTags();

            List<string> data = new List<string>();
            foreach (Tag tag in alltags) data.Add(String.Format("{0}, {1}", tag.Name, tag.Pattern));
            File.WriteAllLines("tags.csv", data);
        }
    }
}
