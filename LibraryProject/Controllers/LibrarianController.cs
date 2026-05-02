using LibraryProject.Views;

namespace LibraryProject.Controllers
{
    // 사서(Librarian)의 동작을 제어하는 컨트롤러 클래스
    public class LibrarianController
    {
        // 사서 뷰를 화면에 표시하는 메서드
        public void ShowLibrarianView()
        {
            // 새로운 Librarian 뷰 인스턴스 생성
            var view = new Librarian();
            // 화면에 Librarian 뷰 출력
            view.Show();
        }
    }
}
