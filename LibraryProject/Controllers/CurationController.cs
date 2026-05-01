using System;
using System.Net.Http;
using LibraryProject.Views;
using LibraryProject.Services;

namespace LibraryProject.Controllers
{
    public class CurationController
    {
        private Curation _view;
        private RecommendationService _recommendationService;
        private LibraryService _libraryService;

        public CurationController(LibraryService libraryService)
        {
            _libraryService = libraryService;
            _recommendationService = new RecommendationService(new AladinApiService(new HttpClient()));
        }

        public async void ShowCurationView()
        {
            _view = new Curation();
            _view.Show();

            try
            {
                var history = _libraryService.GetLoanHistory();
                _view.DisplayStatistics(history);
                
                var recommendations = await _recommendationService.GetRecommendationsAsync(history);
                _view.DisplayRecommendations(recommendations);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"추천 도서를 가져오는 중 오류가 발생했습니다: {ex.Message}");
            }
        }
    }
}
