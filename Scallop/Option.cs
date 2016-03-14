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
        public List<Argument> Arguments { get; set; }
        public Action<string[]> Method { get; set; }


        public Option(string key, Action<string[]> method)
            : this(key, method, "nothing helpful to add...") 
        { }

        public Option(string key, Action<string[]> method, string description)
            : this(key, method, description, new Argument[0])
        { }

        public Option(string key, Action<string[]> method, string description, params Argument[] arguments)
        {
            this.Key = key;
            this.Method = method;
            this.Description = description;
            Arguments = new List<Argument>();
            foreach (Argument arg in arguments) Arguments.Add(arg);
        }
    }
}
