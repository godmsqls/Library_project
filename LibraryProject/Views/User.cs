using System.Drawing;
using System.Windows.Forms;

namespace LibraryProject.Views
{
    public class UserView : Form
    {
        // 1. 멤버 변수 선언
        private TabControl tabControl1;
        private TabPage tabLoan;
        private TabPage tabReturn;

        public UserView()
        {
            // 2. UI 초기화 함수 호출
            SetupLayout();

            this.Text = "사용자 서비스 - 도서 대출/반납";
            this.Size = new Size(600, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 3. 프로세스 종료 이벤트 (잠김 현상 방지)
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void SetupLayout()
        {
            tabControl1 = new TabControl { Dock = DockStyle.Fill };
            tabLoan = new TabPage("도서 대출");
            tabReturn = new TabPage("도서 반납");

            // 대출 버튼
            Button btnLoanAction = new Button { Text = "선택 도서 대출", Location = new Point(450, 300), Size = new Size(120, 40) };
            tabLoan.Controls.Add(btnLoanAction);

            // 추천 도서 버튼
            Button btnGoCuration = new Button { Text = "추천 도서 보기", Location = new Point(20, 300), Size = new Size(120, 40), BackColor = Color.LightGreen };
            btnGoCuration.Click += (s, e) => {
                // 이미 열려있는지 체크하는 로직을 넣거나, 단순히 새로 띄웁니다.
                Curation curationForm = new Curation();
                curationForm.Show();
            };

            tabLoan.Controls.Add(btnGoCuration);
            tabControl1.TabPages.Add(tabLoan);
            tabControl1.TabPages.Add(tabReturn);
            this.Controls.Add(tabControl1);
        }
    }
}