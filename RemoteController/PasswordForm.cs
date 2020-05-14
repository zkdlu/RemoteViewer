using System;
using System.Windows.Forms;

namespace RemoteController
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        public event InputPasswordEventHandler InputPassword;
        private void btnOk_Click(object sender, EventArgs e)
        {
            string pw = txtPw.Text;
            InputPasswordEventArgs args = new InputPasswordEventArgs(pw);

            InputPassword?.Invoke(this, args);

            this.Close();
        }
    }
}
