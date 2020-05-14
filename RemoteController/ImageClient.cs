using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RemoteController
{
    public static class ImageClient
    {
        public delegate bool SendImageDele(Image img);
        
        public static Socket Socket
        {
            get;
            private set;
        }

        public static IPEndPoint RemoteEndPoint
        { 
            get; 
            private set; 
        }

        public static void Connect(string ip)
        {
            try
            {
                int acceptPort = (int)Config.Port.Image;

                IPAddress ipAddr = IPAddress.Parse(ip);
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, acceptPort);
                RemoteEndPoint = remoteEndPoint;

                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Socket.Connect(RemoteEndPoint);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool SendImage(Image img)
        {
            if (Socket == null  && !Socket.Connected)
            {
                return false;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Jpeg);

                byte[] buf = ms.GetBuffer();
                int len = buf.Length;

                byte[] lenBuf = BitConverter.GetBytes(len);
                Socket.Send(lenBuf);

                int trans = 0;
                while (trans < len)
                {
                    trans += Socket.Send(buf, trans, len - trans, SocketFlags.None);
                }

                Socket.Close();
                Socket = null;
                return true;
            }
        }

        public static void SendImageAsync(Image img, AsyncCallback callback)
        {
            SendImageDele dele = new SendImageDele(SendImage);
            dele.BeginInvoke(img, callback, null);
        }
    }
}
