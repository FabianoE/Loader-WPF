using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Loader_WPF.Core.Auth
{
    class Login
    {
        public static AllEnums.ResultEnum SendLogin(string username, string password, out Json.User user)
        {
            user = new Json.User();

            AllEnums.ResultEnum returnResponse = AllEnums.ResultEnum.error;

            string requestData = string.Format("username={0}&password={1}&hwid={2}", username, password, HardwareInfo.GetHwid());

            var response = RequestAPI.GetRequest("https://NOTHING/api/loader.php", requestData);

            Json.User json = JsonConvert.DeserializeObject<Json.User>(response);

            user = json;

            switch (json.result)
            {
                case "success": //Success
                    Core.Inject.DownloadDLL.DownloadInjector();
                    returnResponse = AllEnums.ResultEnum.success;
                    break;

                case "error": //Failed
                    returnResponse = AllEnums.ResultEnum.error;
                    break;
            }

            return returnResponse;
        }
    }
}
