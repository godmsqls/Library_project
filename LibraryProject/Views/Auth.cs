using System;
using System.Windows.Forms;

namespace LibraryProject.Views.Auth
{
    public partial class Auth : Form
    {
        public event EventHandler<(string id, string password)> LoginRequested;
        public event EventHandler SignUpRequested;

        public Auth()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginRequested?.Invoke(this, (txtId.Text, txtPassword.Text));
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            SignUpRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}