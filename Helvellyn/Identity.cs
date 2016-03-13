using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helvellyn
{
    public class Identity
    {
        private static readonly Assembly assembly = typeof(Program).Assembly;

        public const string Name = "Helvellyn";

        public static readonly int Major = assembly.GetName().Version.Major;
        public static readonly int Minor = assembly.GetName().Version.Minor;
        public static readonly int Build = assembly.GetName().Version.Build;
        public static readonly int Revision = assembly.GetName().Version.Revision;

        public static readonly string Version = String.Format("{0}.{1}.{2}:{3}", Major, Minor, Build, Revision);
    }
}
