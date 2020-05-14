using System.Net;
using System.Net.Sockets;

namespace RemoteController
{
    public static class Config
    {
        private static readonly bool Debug = true;
        public enum Port
        {
            Accept = 10200, Image = 10202
        }

        static Config()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    LocalIp = ip.ToString();
                }
            }
        }

        private static string localIp;
        public static string LocalIp
        {
            get
            {
                if (Debug)
                {
                    return IPAddress.Loopback.ToString();
                }

                return localIp;
            }
            private set
            {
                localIp = value;
            }
        }

        public static string Pw
        {
            get
            {
                return "TEST";
            }
        }
    }
}
