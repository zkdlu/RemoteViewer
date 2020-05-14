using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RemoteController
{
    public static class ImageServer
    {
        private static readonly AsyncCallback asyncAccept;

        public static event ReceiveImageEventHandler ImageReceived;

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

        static ImageServer()
        {
            asyncAccept = new AsyncCallback(OnAcceptProc);
        }

        private static void OnAcceptProc(IAsyncResult ar)
        {
            if (Listener == null)
            {
                throw new Exception("Not Exists Server object");
            }

            try
            {
                Socket handler = Listener.EndAccept(ar);

                OnReceive(handler);

                Listener.BeginAccept(asyncAccept, null);
            }
            catch
            {
                throw new Exception("Receive Image Error");
            }
        }

        private static void OnReceive(Socket handler)
        {
            byte[] lenBuf = new byte[4];
            handler.Receive(lenBuf);

            int len = BitConverter.ToInt32(lenBuf, 0);
            byte[] buf = new byte[len];

            int trans = 0;
            while (trans < len)
            {
                trans += handler.Receive(buf, trans, len - trans, SocketFlags.None);
            }

            Image image = ConvertImage(buf);
            ReceiveImageEventArgs args = new ReceiveImageEventArgs(image);
            ImageReceived?.Invoke(typeof(ImageServer), args);
        }

        private static Image ConvertImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, (int)data.Length);
                Bitmap bitmap = new Bitmap(ms);
                return bitmap;
            }
        }

        public static void Start(string ip)
        {
            int imagePort = (int)Config.Port.Image;

            try
            {
                IPAddress ipAddr = IPAddress.Parse(ip);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, imagePort);

                LocalEndPoint = localEndPoint;
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(LocalEndPoint);
                Listener.Listen(10);

                Listener.BeginAccept(asyncAccept, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
