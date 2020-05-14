using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RemoteController
{
    public static class AcceptServer
    {
        private static readonly AsyncCallback asyncAccept;
        private static readonly AsyncCallback asyncReceive;

        public static event AcceptEventHandler Accepted;

        public static Socket Listener
        {
            get;
            private set;
        }

        public static IPEndPoint LocalEndPoint
        {
            get;
            private set;
        }
        
        static AcceptServer()
        {
            asyncAccept = new AsyncCallback(OnAcceptProc);
            asyncReceive = new AsyncCallback(OnReceiveProc);
        }

        private static void OnReceiveProc(IAsyncResult ar)
        {
            var wrapObject = ar.AsyncState as WrapObject;

            Socket handler = wrapObject.Handler;
            IPEndPoint remoteEndPoint = (IPEndPoint)handler.RemoteEndPoint;

            byte[] buf = wrapObject.Buffer;

            if (handler == null)
            {
                throw new Exception("Not Exists Handler object");
            }

            int count = handler.EndReceive(ar);
            if (count > 0)
            {
                string msg = Encoding.UTF8.GetString(buf, 0, count);

                AcceptEventArgs e = new AcceptEventArgs(remoteEndPoint, msg);
                Accepted?.Invoke(typeof(AcceptServer), e);
            }

            wrapObject = new WrapObject
            {
                Handler = handler,
                Buffer = new byte[256]
            };

            if (handler != null)
            {
                handler.BeginReceive(buf, 0, buf.Length, SocketFlags.None, asyncReceive, wrapObject);
            }
        }

        private static void OnAcceptProc(IAsyncResult ar)
        {
            if (Listener == null)
            {
                throw new Exception("Not Exists Server object");
            }

            Socket handler = Listener.EndAccept(ar);

            byte[] buf = new byte[256];

            var wrapObject = new WrapObject
            {
                Handler = handler,
                Buffer = buf
            };

            if (handler != null)
            {
                handler.BeginReceive(buf, 0, buf.Length, SocketFlags.None, asyncReceive, wrapObject);
            }

            Listener.BeginAccept(asyncAccept, null);
        }

        public static void Start(string ip)
        {
            int acceptPort = (int)Config.Port.Accept;

            IPAddress ipAddr = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, acceptPort);

            LocalEndPoint = localEndPoint;
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(LocalEndPoint);
            Listener.Listen(10);

            Listener.BeginAccept(asyncAccept, null);
        }
    }
}
