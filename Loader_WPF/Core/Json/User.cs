using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loader_WPF.Core.Json
{
    class User
    {
        public int recharge { get; set; }
        public string result{get; set;}
        public string message{get; set;}
        public IList<CheatsJson> cheats { get; set; }

        //Save Login/Password
        public string username{ get; set; }
        public string password { get; set; }
    }
}
