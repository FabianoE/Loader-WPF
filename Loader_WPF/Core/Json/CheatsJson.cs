using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loader_WPF.Core.Json
{

    class CheatsJson
    {
        public int id { get; set; }
        public string Game{ get; set; }
        public string Version { get; set; }
        public string process_name { get; set; }
        public string Desenvolvedor{  get;set; }
        public string path_dll { get; set; }
        public string Status { get; set; }
        public string developer { set { Desenvolvedor = value; } }
        public string status{ set  { Status = value == "1" ? "Online" : "Offline"; } } 
    }
}
