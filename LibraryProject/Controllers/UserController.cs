using System;
using System.Net.Http;
using LibraryProject.Views;
using LibraryProject.Services;

namespace LibraryProject.Controllers
{
    public class UserController
    {
        // 사용자 인터페이스 뷰 인스턴스 
        private UserView _view;
        // 외부 도서 API 서비스 연동 인스턴스
        private AladinApiService _apiService;
        // 도서관 도서 대출/반납 관련 비즈니스 로직 서비스 인스턴스
        private LibraryService _libraryService;

        // UserController 생성자
        public UserController()
        {
            // HttpClient를 이용해 API 서비스 생성
            _apiService = new AladinApiService(new HttpClient());
            // 라이브러리 서비스 생성
            _libraryService = new LibraryService();
        }

        // 사용자 뷰를 화면에 표시하는 메서드
        public void ShowUserView()
        {
            // 뷰 인스턴스 초기화
            _view = new UserView();

            // 뷰에서 발생한 SearchRequested 이벤트를 담당 메서드에 연결
            _view.SearchRequested += View_SearchRequested;
            // 뷰에서 발생한 CurationRequested 이벤트를 담당 메서드에 연결
            _view.CurationRequested += View_CurationRequested;
            // 뷰에서 발생한 LoanRequested 이벤트를 담당 메서드에 연결
            _view.LoanRequested += View_LoanRequested;
            // 뷰에서 발생한 ReturnRequested 이벤트를 담당 메서드에 연결
            _view.ReturnRequested += View_ReturnRequested;

            // 뷰 화면을 띄움
            _view.Show();
        }

        // 도서 대출 요청 시 실행되는 이벤트 핸들러
        private void View_LoanRequested(object sender, BookItem book)
        {
            // 오류가 발생할 수 있는 로직을 try-catch 블록으로 감쌈
            try
            {
                // 라이브러리 서비스를 통해 선택된 도서 대출 처리
                _libraryService.LoanBook(book);
                // 뷰에 대출 완료 메시지를 전달하여 표시
                _view.ShowMessage($"'{book.Title}' 도서가 대출되었습니다.");
                // 현재 사용자의 대출 목록을 뷰에 전달헤어 갱신
                _view.DisplayLoans(_libraryService.GetCurrentLoans());
            }
            // 오류 발생 시 예외 객체 포획
            catch (Exception ex)
            {
                // 오류 메시지를 뷰에 표시
                _view.ShowMessage(ex.Message);
            }
        }

        // 도서 반납 요청 시 실행되는 이벤트 핸들러
        private void View_ReturnRequested(object sender, string isbn13)
        {
            // 반납 처리 도중 발생할 수 있는 오류를 잡기 위한 try-catch
            try
            {
                // 입력받은 ISBN으로 도서 반납 처리
                _libraryService.ReturnBook(isbn13);
                // 뷰를 통해 반납 완료 메시지 표시
                _view.ShowMessage("도서가 반납되었습니다.");
                // 대출 목록을 갱신하여 뷰에 반영
                _view.DisplayLoans(_libraryService.GetCurrentLoans());
            }
            // 반납 중 발생한 예외 처리
            catch (Exception ex)
            {
                // 오류 메시지 표시
                _view.ShowMessage(ex.Message);
            }
        }

        // 도서 검색 요청 시 비동기로 실행되는 이벤트 핸들러
        private async void View_SearchRequested(object sender, string query)
        {
            // 검색어가 비어 있거나 공백이면 진행하지 않고 리턴
            if (string.IsNullOrWhiteSpace(query)) return;

            // API 호출 오류를 대비한 try-catch 블록
            try
            {
                // API 서비스를 통해 검색어에 해당하는 도서 목록 비동기 조회
                var response = await _apiService.GetBooksByQuery(query);
                // 응답과 반환된 도서 목록이 null이 아닌지 확인
                if (response != null && response.BookItems != null)
                {
                    // 조회된 도서 목록을 뷰의 화면에 표시하도록 위임
                    _view.DisplayBooks(response.BookItems);
                }
            }
            // 검색 중 예외 발생 시의 처리
            catch (Exception ex)
            {
                // 뷰에 에러 메시지를 표시
                _view.ShowMessage($"도서 검색 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        // 도서 추천(Curation) 요청 시 실행되는 이벤트 핸들러
        private void View_CurationRequested(object sender, EventArgs e)
        {
            // CurationController를 생성하면서 기존 라이브러리 서비스를 넘겨줌
            var curationController = new CurationController(_libraryService);
            // 큐레이션 화면 표시
            curationController.ShowCurationView();
        }
    }
}
