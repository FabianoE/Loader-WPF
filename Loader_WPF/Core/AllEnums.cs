using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loader_WPF.Core
{
    public class AllEnums
    {
        public enum ResultEnum
        {
            success,
            error,
        }

        public static string ErrorInject(int error)
        {
            switch (error)
            {
                case 1:
                    return "Failed to OpenProcess";
                case 2:
                    return "Failed to allocate memory";
                case 3:
                    return "Failed to WriteProcessMemory";
                case 4:
                    return "Failed to get LoadLybraryA's address";
                case 5:
                    return "Failed to CreateRemoteThread";
                case 6:
                    return "Erro ao tentar baixar a DLLD";
                default:
                    return "Erro desconhecido\nEntre em contato com a staff";
            }

        }

        public enum Forms{
            Login,
            Recharge,
            Injection,
            Error,
        }
    }
}
