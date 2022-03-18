using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Loader_WPF.Core.Cheat
{
    class CheatList
    {
        public static List<Json.CheatsJson> returnList(IList<Json.CheatsJson> list)
        {
            return (List<Json.CheatsJson>)list;
        }
    }

    class InjectionCheat
    {
        public Dictionary<int, Json.CheatsJson> listInjection = new Dictionary<int, Json.CheatsJson>();

        public int currentCheat = 0;

        public void AddItem(Json.CheatsJson item)
        {
            if (item == null)
                return;
            listInjection.Add(item.id, item);
        }

        public void SetCurrentCheat(int id)
        {
            if (listInjection.ContainsKey(id))
                currentCheat = id;
        }

        public Json.CheatsJson returnCurrentCheatData()
        {
            return listInjection.ContainsKey(currentCheat) && listInjection[currentCheat].Status == "Online" ?  listInjection[currentCheat] : null;
        }
    }
}
