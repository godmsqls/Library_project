using LibraryProject.Views;
using LibraryProject.Views.Auth;
using System.Windows.Forms;

namespace LibraryProject.Controllers
{
    // 사용자 인증 관련 처리를 담당하는 컨트롤러 클래스
    public class AuthController
    {
        // 인증 화면(로그인 화면)을 띄우는 메서드
        public void ShowAuthView()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 로그인 화면 뷰 객체 생성
            var authView = new Auth();
            // 뷰에서 로그인 요청 이벤트가 발생했을 때 처리할 람다 이벤트 핸들러 등록
            authView.LoginRequested += (sender, args) => 
            {
                // 전달받은 인자에서 아이디(id)와 비밀번호(password) 추출
                var (id, password) = args;
                // 아이디가 "admin"이고 비밀번호가 "admin"인지 확인
                if(id == "admin" && password == "admin")
                {
                    authView.Hide();
                    // 사서 전용 컨트롤러를 생성하고 사서 뷰를 표시
                    new LibrarianController().ShowLibrarianView();
                } 
                // 관리자가 아닌 일반 사용자일 경우
                else
                {
                    authView.Hide();
                    // 사용자 전용 컨트롤러를 생성하고 사용자 뷰를 표시
                    new UserController().ShowUserView();
                }
            };

            // 생성한 authView 폼을 메인 루프에 넣어 애플리케이션 실행
            Application.Run(authView);
        }
    }
}
