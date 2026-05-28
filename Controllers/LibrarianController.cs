using LibraryProject.Views;

namespace LibraryProject.Controllers
{
    // 사서(Librarian)의 동작을 제어하는 컨트롤러 클래스
    public class LibrarianController
    {
        private Views.Librarian _view;

        // 사서 뷰를 화면에 표시하는 메서드
        public void ShowLibrarianView()
        {
            // 새로운 Librarian 뷰 인스턴스 생성
            _view = new Librarian();

            // 데이터 로드
            var overdueLoans = Models.LoanRecord.GetOverdueLoans();
            _view.DisplayOverdueLoans(overdueLoans);

            _view.NotifyRequested += (s, e) =>
            {
                var loans = Models.LoanRecord.GetOverdueLoans();
                if (loans.Count == 0)
                {
                    _view.ShowMessage("연체 중인 사용자가 없습니다.");
                    return;
                }

                var emails = new System.Collections.Generic.HashSet<string>();
                foreach (var loan in loans)
                {
                    var user = Models.User.GetUserById(loan.UserId);
                    if (user != null && !string.IsNullOrWhiteSpace(user.Email))
                    {
                        emails.Add(user.Email);
                    }
                }

                if (emails.Count > 0)
                {
                    string emailsToNotify = string.Join("\n", emails);
                    _view.ShowMessage($"다음 이메일로 연체 알림을 발송합니다:\n{emailsToNotify}");
                }
                else
                {
                    _view.ShowMessage("연체 중인 사용자 중 등록된 이메일이 없습니다.");
                }
            };

            // 화면에 Librarian 뷰 출력
            _view.Show();
        }
    }
}
