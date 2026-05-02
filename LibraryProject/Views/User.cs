using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LibraryProject.Services;

namespace LibraryProject.Views
{
    public class UserView : Form
    {
        // 1. 멤버 변수 선언
        private TabControl tabControl1;
        private TabPage tabLoan;
        private TabPage tabReturn;

        private TextBox txtSearch;
        private DataGridView dgvBooks;
        private DataGridView dgvLoans;

        // View -> Controller 이벤트 정의
        public event EventHandler<string> SearchRequested;
        public event EventHandler CurationRequested;
        public event EventHandler<BookItem> LoanRequested;
        public event EventHandler<string> ReturnRequested;

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

            // 도서 검색 텍스트박스와 버튼
            txtSearch = new TextBox { Location = new Point(20, 20), Width = 300 };
            Button btnSearch = new Button { Text = "검색", Location = new Point(330, 20), Size = new Size(80, 25) };
            btnSearch.Click += (s, e) => {
                SearchRequested?.Invoke(this, txtSearch.Text);
            };

            // 검색 결과 DataGridView
            dgvBooks = new DataGridView 
            { 
                Location = new Point(20, 60), 
                Size = new Size(530, 220),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            tabLoan.Controls.Add(txtSearch);
            tabLoan.Controls.Add(btnSearch);
            tabLoan.Controls.Add(dgvBooks);

            // 대출 버튼
            Button btnLoanAction = new Button { Text = "선택 도서 대출", Location = new Point(430, 300), Size = new Size(120, 40) };
            btnLoanAction.Click += (s, e) => {
                if (dgvBooks.SelectedRows.Count > 0)
                {
                    var book = (BookItem)dgvBooks.SelectedRows[0].DataBoundItem;
                    LoanRequested?.Invoke(this, book);
                }
                else if (dgvBooks.SelectedCells.Count > 0)
                {
                    int rowIndex = dgvBooks.SelectedCells[0].RowIndex;
                    var book = (BookItem)dgvBooks.Rows[rowIndex].DataBoundItem;
                    LoanRequested?.Invoke(this, book);
                }
                else
                {
                    MessageBox.Show("대출할 도서를 선택하세요.");
                }
            };
            tabLoan.Controls.Add(btnLoanAction);

            // 추천 도서 버튼
            Button btnGoCuration = new Button { Text = "추천 도서 보기", Location = new Point(20, 300), Size = new Size(120, 40), BackColor = Color.LightGreen };
            btnGoCuration.Click += (s, e) => {
                CurationRequested?.Invoke(this, EventArgs.Empty);
            };

            tabLoan.Controls.Add(btnGoCuration);
            tabControl1.TabPages.Add(tabLoan);
            tabControl1.TabPages.Add(tabReturn);
            this.Controls.Add(tabControl1);

            // 반납 탭 UI 구성
            dgvLoans = new DataGridView 
            { 
                Location = new Point(20, 20), 
                Size = new Size(530, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            tabReturn.Controls.Add(dgvLoans);

            Button btnReturnAction = new Button { Text = "선택 도서 반납", Location = new Point(430, 300), Size = new Size(120, 40) };
            btnReturnAction.Click += (s, e) => {
                if (dgvLoans.SelectedRows.Count > 0)
                {
                    var record = (LibraryProject.Models.LoanRecord)dgvLoans.SelectedRows[0].DataBoundItem;
                    ReturnRequested?.Invoke(this, record.Isbn13);
                }
                else if (dgvLoans.SelectedCells.Count > 0)
                {
                    int rowIndex = dgvLoans.SelectedCells[0].RowIndex;
                    var record = (LibraryProject.Models.LoanRecord)dgvLoans.Rows[rowIndex].DataBoundItem;
                    ReturnRequested?.Invoke(this, record.Isbn13);
                }
                else
                {
                    MessageBox.Show("반납할 도서를 선택하세요.");
                }
            };
            tabReturn.Controls.Add(btnReturnAction);

            tabControl1.SelectedIndexChanged += (s, e) => {
                if (tabControl1.SelectedTab == tabReturn)
                {
                    // Trigger an event to refresh loans if needed, or simply expose a method
                }
            };
        }

        // Controller에서 호출할 메서드: 검색 결과 표시
        public void DisplayBooks(List<BookItem> books)
        {
            dgvBooks.DataSource = null;
            dgvBooks.DataSource = books;
        }

        public void DisplayLoans(List<LibraryProject.Models.LoanRecord> loans)
        {
            dgvLoans.DataSource = null;
            dgvLoans.DataSource = loans;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
