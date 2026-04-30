using System.Drawing;
using System.Windows.Forms;

namespace LibraryProject.Views
{
    public class Librarian : Form
    {
        // 1. UI 컨트롤을 멤버 변수로 선언하여 관리
        private Label lblHeader;
        private DataGridView dgvOverdue;
        private Panel pnlBottom;
        private Button btnNotify;

        public Librarian()
        {
            // 2. 레이아웃 설정 메서드 호출
            SetupLayout();

            this.Text = "사서 관리 모드 - 연체자 관리";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 3. 폼이 닫힐 때 프로세스를 완전히 종료 (파일 잠김 방지)
            this.FormClosed += (s, e) => Application.Exit();
        }

        private void SetupLayout()
        {
            // 헤더 라벨 설정
            lblHeader = new Label
            {
                Text = "연체자 관리 대시보드",
                Font = new Font("Noto Sans KR", 14),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 연체자 목록 그리드뷰 설정
            dgvOverdue = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White
            };
            // 데이터 바인딩 예시: dgvOverdue.DataSource = overdueList;

            // 하단 패널 및 버튼 설정
            pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 60 };
            btnNotify = new Button
            {
                Text = "연체 알림 발송",
                Location = new Point(650, 15),
                Size = new Size(120, 30)
            };

            pnlBottom.Controls.Add(btnNotify);

            // 컨트롤 추가 (중복 방지를 위해 Clear 후 추가하거나 순서대로 추가)
            this.Controls.Clear();
            this.Controls.Add(dgvOverdue);
            this.Controls.Add(lblHeader);
            this.Controls.Add(pnlBottom);
        }
    }
}