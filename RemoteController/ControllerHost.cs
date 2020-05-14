using System;

namespace RemoteController
{
    public static class ControllerHost
    {
        public static event ReceiveImageEventHandler ImageReceived;

        static ControllerHost()
        {
            ImageServer.ImageReceived += ImageServer_ReceiveImage;
        }

        public static void AcceptClientStart(string ip)
        {
            AcceptClient.Connect(ip);
        }

        public static void ImageServerStart()
        {
            ImageServer.Start(Config.LocalIp);
        }

        private static void ImageServer_ReceiveImage(object sender, ReceiveImageEventArgs e)
        {
            ImageReceived?.Invoke(typeof(ControllerHost), e);
        }
    }
}
