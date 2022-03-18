using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Loader_WPF.Core.Inject
{
    public class DownloadDLL
    {
        static WebClient web = new WebClient();
        public static int progressDownload = 0;
        public static Action action; //Ação é executada quando o progresso de download do injetor é alterado

        public static async Task<bool> DownloadInjector()
        {
            string architecture = Core.HardwareInfo.GetArchitecture();
            string injector_url = Core.RequestAPI.GetFile("injector_" + architecture + ".exe");
            string injector_path = "C:\\Windows\\Temp\\" + "hhhhh.exe";

            CheatManager.CamouflagedInjector(injector_path);

            web.DownloadFileAsync(new Uri(injector_url), injector_path);
            web.DownloadProgressChanged += Web_DownloadProgressChanged_INJECTOR;

            return true;
        }

        private static void Web_DownloadProgressChanged_INJECTOR(object sender, DownloadProgressChangedEventArgs e)
        {
            progressDownload = e.ProgressPercentage;
            action?.Invoke();
        }

        private static string DownloadDll(string dll_name)
        {
            string dll_url = Core.RequestAPI.GetFile(dll_name);
            string dll_path = "C:\\Windows\\Temp\\" + CheatManager.RandCamouflagedFileName(".dll");

            web.DownloadFileAsync(new Uri(dll_url), dll_path);

            return dll_path;
        }

        public static async Task<string> DllName(string dll_name)
        {
            string dll_path = null;

            try
            {
                dll_path = DownloadDll(dll_name);
            }
            catch
            {
                return null;
            }

            return dll_path;
        }
    }
}
