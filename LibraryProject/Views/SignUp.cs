using System;
using System.Windows.Forms;

namespace LibraryProject.Views.Auth
{
    public partial class SignUp : Form
    {
        public event EventHandler<(string id, string password, string name, string email, string role)> SignUpSubmitted;

        public SignUp()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            cmbRole.SelectedIndex = 0; // Member
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("아이디, 비밀번호, 이름은 필수입니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SignUpSubmitted?.Invoke(this, (txtId.Text, txtPassword.Text, txtName.Text, txtEmail.Text, cmbRole.Text));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
