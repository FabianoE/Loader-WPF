using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Loader_WPF.Core
{
    class HardwareInfo
    {
        public static string GetArchitecture()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return "x64";
            }

            return "x84";
        }

        public static string GetHwid()
        {
            string id = "";
            var manager = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection managerlist = manager.Get();

            foreach(ManagementObject obj in managerlist)
            {
                id = obj["ProcessorId"].ToString();
                break;
            }

            return id;
        }

        private string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed && !string.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
        }
    }
}
