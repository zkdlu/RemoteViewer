using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace RemoteController
{
    public static class RemoteHost
    {
        public static Rectangle Rect
        {
            get;
            private set;
        }

        static RemoteHost()
        {
            AutomationElement ae = AutomationElement.RootElement;
            System.Windows.Rect rect = ae.Current.BoundingRectangle;

            Rect = new Rectangle((int)rect.Left,
                                 (int)rect.Top,
                                 (int)rect.Width,
                                 (int)rect.Height);

            AcceptServer.Accepted += AcceptServer_Accepted;
        }

        private static void AcceptServer_Accepted(object sender, AcceptEventArgs e)
        {
            string myPw = Config.Pw;
            string pw = e.Pw;

            if (pw.Equals(myPw))
            {
                string remoteIp = e.RemoteEndPoint.Address.ToString();

                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    ImageClientStart(remoteIp);
                });
            }
        }

        private static void ImageClientStart(string remoteIp)
        {
            while (true)
            {
                Image screenImg = CaptureScreen();

                ImageClient.Connect(remoteIp);
                ImageClient.SendImageAsync(screenImg, null);
            }
        }

        private static Image CaptureScreen()
        {
            int width = Rect.Width;
            int height = Rect.Height;

            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                Size size = new Size(width, height);
                gr.CopyFromScreen(new Point(0, 0), new Point(0, 0), size);
            }

            return bitmap;
        }

        public static void AcceptServerStart()
        {
            string ip = Config.LocalIp;

            AcceptServer.Start(ip);
        }
    }
}
