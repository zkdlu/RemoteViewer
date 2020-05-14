using System;

namespace RemoteController
{
    public class AcceptEventArgs : EventArgs
    {
        public string Pw
        {
            get;
            private set;
        }

        public AcceptEventArgs(string pw)
        {
            Pw = pw;
        }
    }

    public delegate void AcceptEventHandler(object sender, AcceptEventArgs e);
}
