using System;
using System.Drawing;

namespace RemoteController
{
    public class ReceiveImageEventArgs : EventArgs
    {
        public Image Image
        {
            get;
            private set;
        }

        public ReceiveImageEventArgs(Image image)
        {
            Image = image;
        }
    }

    public delegate void ReceiveImageEventHandler(object sender, ReceiveImageEventArgs e);
}
