using System;

namespace RemoteController
{
    public class InputPasswordEventArgs : EventArgs
    {
        public string Pw
        {
            get;
            private set;
        }

        public InputPasswordEventArgs(string pw)
        {
            Pw = pw;
        }
    }

    public delegate void InputPasswordEventHandler(object sender, InputPasswordEventArgs e);
}
