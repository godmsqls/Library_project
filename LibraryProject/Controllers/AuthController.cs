using LibraryProject.Views;
using LibraryProject.Views.Auth;
using LibraryProject.Models;
using System.Windows.Forms;
using System;

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

            // 회원가입 요청 이벤트 처리
            authView.SignUpRequested += (sender, args) =>
            {
                var signUpView = new SignUp();
                signUpView.SignUpSubmitted += (s, ev) =>
                {
                    var (id, password, name, email, role) = ev;

                    try 
                    {
                        var user = new User(id, password, name, role, email);
                        user.InsertUser();
                        MessageBox.Show("회원가입이 완료되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        signUpView.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"회원가입 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                signUpView.ShowDialog();
            };

            // 뷰에서 로그인 요청 이벤트가 발생했을 때 처리할 람다 이벤트 핸들러 등록
            authView.LoginRequested += (sender, args) => 
            {
                // 전달받은 인자에서 아이디(id)와 비밀번호(password) 추출
                var (id, password) = args;

                // 데이터베이스에서 유저 조회
                var user = User.GetUser(id);

                // 유저가 존재하고 비밀번호가 일치하는지 확인
                if(user != null && user.Password == password)
                {
                    authView.Hide();
                    if(user.Role == "Admin")
                    {
                        // 사서 전용 컨트롤러를 생성하고 사서 뷰를 표시
                        new LibrarianController().ShowLibrarianView();
                    } 
                    else
                    {
                        // 사용자 전용 컨트롤러를 생성하고 사용자 뷰를 표시
                        new UserController().ShowUserView();
                    }
                } 
                else
                {
                    MessageBox.Show("아이디 또는 비밀번호가 올바르지 않습니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // 생성한 authView 폼을 메인 루프에 넣어 애플리케이션 실행
            Application.Run(authView);
        }
    }
}
