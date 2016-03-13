using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scallop
{
    public class Option
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public Action<string[]> Method { get; set; }
    }
}
