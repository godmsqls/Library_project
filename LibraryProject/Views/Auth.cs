using System;
using System.Windows.Forms;

namespace LibraryProject.Views.Auth
{
    public partial class Auth : Form
    {
        public event EventHandler<(string id, string password)> LoginRequested;

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
    }
}