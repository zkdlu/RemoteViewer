using System;
using System.Windows.Forms;

namespace RemoteController
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtId.Text = Config.LocalIp;
            txtPw.Text = Config.Pw;

            RemoteHost.AcceptServerStart();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ControllerHost.ImageReceived += ControllerHost_ImageReceived;
        }

        private void ControllerHost_ImageReceived(object sender, ReceiveImageEventArgs e)
        {
            picture.Image = e.Image;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string targetIp = txtTargetId.Text;

            ControllerHost.AcceptClientStart(targetIp);
        }
    }
}
