using System;
using System.Net.Http;
using LibraryProject.Views;
using LibraryProject.Services;

namespace LibraryProject.Controllers
{
    public class UserController
    {
        private UserView _view;
        private AladinApiService _apiService;
        private LibraryService _libraryService;

        public UserController()
        {
            _apiService = new AladinApiService(new HttpClient());
            _libraryService = new LibraryService();
        }

        public void ShowUserView()
        {
            _view = new UserView();

            // 뷰 이벤트 구독
            _view.SearchRequested += View_SearchRequested;
            _view.CurationRequested += View_CurationRequested;
            _view.LoanRequested += View_LoanRequested;
            _view.ReturnRequested += View_ReturnRequested;

            _view.Show();
        }

        private void View_LoanRequested(object sender, BookItem book)
        {
            try
            {
                _libraryService.LoanBook(book);
                System.Windows.Forms.MessageBox.Show($"'{book.Title}' 도서가 대출되었습니다.");
                _view.DisplayLoans(_libraryService.GetCurrentLoans());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void View_ReturnRequested(object sender, string isbn13)
        {
            try
            {
                _libraryService.ReturnBook(isbn13);
                System.Windows.Forms.MessageBox.Show("도서가 반납되었습니다.");
                _view.DisplayLoans(_libraryService.GetCurrentLoans());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private async void View_SearchRequested(object sender, string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            try
            {
                var response = await _apiService.GetBooksByQuery(query);
                if (response != null && response.BookItems != null)
                {
                    _view.DisplayBooks(response.BookItems);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"도서 검색 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private void View_CurationRequested(object sender, EventArgs e)
        {
            var curationController = new CurationController(_libraryService);
            curationController.ShowCurationView();
        }
    }
}
