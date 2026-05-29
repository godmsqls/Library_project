namespace LibraryProject.Views.Auth
{
    partial class SignUp
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblId = new System.Windows.Forms.Label();
            lblPassword = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            lblEmail = new System.Windows.Forms.Label();
            lblRole = new System.Windows.Forms.Label();
            txtId = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            txtName = new System.Windows.Forms.TextBox();
            txtEmail = new System.Windows.Forms.TextBox();
            cmbRole = new System.Windows.Forms.ComboBox();
            btnSubmit = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Noto Sans KR", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
            lblTitle.Location = new System.Drawing.Point(120, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(95, 35);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "회원가입";
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new System.Drawing.Point(40, 80);
            lblId.Name = "lblId";
            lblId.Size = new System.Drawing.Size(52, 20);
            lblId.TabIndex = 1;
            lblId.Text = "아이디";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(40, 120);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(67, 20);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "비밀번호";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(40, 160);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(37, 20);
            lblName.TabIndex = 3;
            lblName.Text = "이름";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new System.Drawing.Point(40, 200);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new System.Drawing.Size(52, 20);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "이메일";
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Location = new System.Drawing.Point(40, 240);
            lblRole.Name = "lblRole";
            lblRole.Size = new System.Drawing.Size(37, 20);
            lblRole.TabIndex = 5;
            lblRole.Text = "권한";
            // 
            // txtId
            // 
            txtId.Location = new System.Drawing.Point(120, 77);
            txtId.Name = "txtId";
            txtId.Size = new System.Drawing.Size(160, 27);
            txtId.TabIndex = 6;
            // 
            // txtPassword
            // 
            txtPassword.Location = new System.Drawing.Point(120, 117);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new System.Drawing.Size(160, 27);
            txtPassword.TabIndex = 7;
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(120, 157);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(160, 27);
            txtName.TabIndex = 8;
            // 
            // txtEmail
            // 
            txtEmail.Location = new System.Drawing.Point(120, 197);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new System.Drawing.Size(160, 27);
            txtEmail.TabIndex = 9;
            // 
            // cmbRole
            // 
            cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "Member", "Admin" });
            cmbRole.Location = new System.Drawing.Point(120, 237);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new System.Drawing.Size(160, 28);
            cmbRole.TabIndex = 10;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btnSubmit.Location = new System.Drawing.Point(70, 290);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new System.Drawing.Size(90, 40);
            btnSubmit.TabIndex = 11;
            btnSubmit.Text = "가입하기";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(180, 290);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(90, 40);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "취소";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SignUp
            // 
            ClientSize = new System.Drawing.Size(340, 360);
            Controls.Add(btnCancel);
            Controls.Add(btnSubmit);
            Controls.Add(cmbRole);
            Controls.Add(txtEmail);
            Controls.Add(txtName);
            Controls.Add(txtPassword);
            Controls.Add(txtId);
            Controls.Add(lblRole);
            Controls.Add(lblEmail);
            Controls.Add(lblName);
            Controls.Add(lblPassword);
            Controls.Add(lblId);
            Controls.Add(lblTitle);
            Name = "SignUp";
            Text = "회원가입";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
    }
}