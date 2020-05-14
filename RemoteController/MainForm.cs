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
            string ip = Config.LocalIp;

            txtId.Text = ip;
            txtPw.Text = Config.Pw;

            Server.Accepted += Server_Accepted;
            Server.Start(ip);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void Server_Accepted(object sender, AcceptEventArgs e)
        {
            string myPw = Config.Pw;
            string pw = e.Pw;

            if (pw.Equals(myPw))
            {
                MessageBox.Show("패스워드가 일치하니 다음 작업을 진행하자");
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string targetIp = txtTargetId.Text;
            Client.Connect(targetIp);
        }

    }
}
