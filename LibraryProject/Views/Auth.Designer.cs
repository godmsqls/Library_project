namespace LibraryProject.Views.Auth
{
    partial class Auth
    {
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            txtId = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            btnLogin = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Noto Sans KR", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
            label1.Location = new System.Drawing.Point(89, 21);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(151, 29);
            label1.TabIndex = 0;
            label1.Text = "도서 관리 시스템";
            // 
            // txtId
            // 
            txtId.Font = new System.Drawing.Font("Noto Sans KR", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            txtId.Location = new System.Drawing.Point(151, 82);
            txtId.Name = "txtId";
            txtId.PlaceholderText = "아이디";
            txtId.Size = new System.Drawing.Size(125, 29);
            txtId.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Font = new System.Drawing.Font("Noto Sans KR", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            txtPassword.Location = new System.Drawing.Point(151, 122);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderText = "비밀번호";
            txtPassword.Size = new System.Drawing.Size(125, 29);
            txtPassword.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Noto Sans KR", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            label2.Location = new System.Drawing.Point(45, 82);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 25);
            label2.TabIndex = 3;
            label2.Text = "아이디";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Noto Sans KR", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
            label3.Location = new System.Drawing.Point(45, 122);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 25);
            label3.TabIndex = 4;
            label3.Text = "비밀번호";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = System.Drawing.SystemColors.ActiveCaption;
            btnLogin.Font = new System.Drawing.Font("Noto Sans KR Medium", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
            btnLogin.Location = new System.Drawing.Point(223, 165);
            btnLogin.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(90, 34);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "로그인";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // Auth
            // 
            ClientSize = new System.Drawing.Size(332, 203);
            Controls.Add(btnLogin);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtPassword);
            Controls.Add(txtId);
            Controls.Add(label1);
            Name = "Auth";
            Text = "로그인 화면";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogin;
    }
}