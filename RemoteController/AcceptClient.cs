using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RemoteController
{
    public static class AcceptClient
    {
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
        
        static AcceptClient()
        {
        }

        public static void Connect(string ip)
        {
            int acceptPort = (int)Config.Port.Accept;

            IPAddress ipAddr = IPAddress.Parse(ip);
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, acceptPort);
            RemoteEndPoint = remoteEndPoint;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Socket.Connect(RemoteEndPoint);

            if (Socket.Connected)
            {
                PasswordForm passwordForm = new PasswordForm();
                passwordForm.InputPassword += PasswordForm_InputPassword;
                passwordForm.ShowDialog();
            }
        }

        private static void PasswordForm_InputPassword(object sender, InputPasswordEventArgs e)
        {
            ControllerHost.ImageServerStart();

            string msg = e.Pw;
            byte[] buf = Encoding.UTF8.GetBytes(msg);

            Socket.Send(buf);
        }
    }
}
