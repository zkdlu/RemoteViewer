using System;
using System.Net;

namespace RemoteController
{
    public class AcceptEventArgs : EventArgs
    {
        public IPEndPoint RemoteEndPoint
        {
            get;
            private set;
        }

        public string Pw
        {
            get;
            private set;
        }

        public AcceptEventArgs(IPEndPoint remoteEndpPint, string pw)
        {
            RemoteEndPoint = remoteEndpPint;
            Pw = pw;
        }
    }

    public delegate void AcceptEventHandler(object sender, AcceptEventArgs e);
}
