using System.Net.Sockets;

namespace RemoteController
{
    class WrapObject
    {
        public Socket Handler
        {
            get;
            set;
        }

        public byte[] Buffer
        {
            get;
            set;
        }
    }
}
