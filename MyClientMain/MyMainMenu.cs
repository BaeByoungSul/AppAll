using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyClientMain
{
    public class MyMainMenu
    {
        public int MenuID { get; set; }
        public int ParentID { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public string ShowInfo { get; set; }
    }
    public  class MyLoadedAssembly
    {
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
        public Assembly DevAssembly { get; set; }
    }
}
