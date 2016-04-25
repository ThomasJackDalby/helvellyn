using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public static class DataManager
    {
        public static IDataStore LoadDataStore()
        {
            return new DataStore();
        }     
    }
}
