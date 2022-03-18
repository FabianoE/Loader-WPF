using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loader_WPF.Core.Inject
{
    class CheatManager
    {
        public static string RandCamouflagedFileName(string format)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[48];
            var random = new Random();

            for (int str_count = 0; str_count < stringChars.Length; str_count++)
            {
                stringChars[str_count] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars) + format;
        }

        public static void CamouflagedDll(string dll_path)
        {
            File.Move(dll_path, "C:\\Windows\\Temp\\" + RandCamouflagedFileName(".tmp"));
        }

        public static void CamouflagedInjector(string injector_path)
        {
            string injector_camouflaged = "C:\\Windows\\Temp\\" + RandCamouflagedFileName(".tmp");

            if (!File.Exists(injector_path))
                return;
            
            File.Move(injector_path, injector_camouflaged);
            while (File.Exists(injector_camouflaged))
            {
                File.Delete(injector_camouflaged);
            }
        }

        public static string HandleProcessName(string game, string process)
        {
            string architecture = Core.HardwareInfo.GetArchitecture();

            switch (game)
            {
                case "Crossfire BR":
                    if (architecture == "x64")
                        return "crossfire_x64.exe";

                    return "crossfire.exe";
                default:
                    return process;
            }
        }

        public static bool VerifyInjectorIsRunning()
        {
            return false;
        }

        public static Process RunInjector(string ProcessName, string DllPath)
        {
            //Code

            return null;
        }
    }
}
