using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyClientLib
{
    public class DbReturn
    {
        public string ReturnCD { get; set; }
        public string ReturnMsg { get; set; }
        public DataSet  ReturnDs { get; set; }
    }
}
