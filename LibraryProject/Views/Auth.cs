using LibraryProject.Controllers;
using System;
using System.Windows.Forms;

namespace LibraryProject.Views.Auth
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnLogin.Click += btnLogin_Click;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtId.Text == "admin" && txtPassword.Text == "admin")
            {
                new Librarian().Show(); 
                this.Hide();
            } 
            else
            {
                this.Hide();
                new UserView().Show();
            }
        }
    }
}