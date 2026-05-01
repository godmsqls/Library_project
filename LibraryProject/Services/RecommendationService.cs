using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.Services
{
    public class RecommendationService
    {
        private readonly AladinApiService _apiService;

        public RecommendationService(AladinApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<BookItem>> GetRecommendationsAsync(List<LoanRecord> loanHistory)
        {
            if (loanHistory == null || loanHistory.Count == 0)
            {
                // 대출 이력이 없으면 베스트셀러를 추천
                var response = await _apiService.GetBestsellersAsync();
                return response.BookItems?.Take(6).ToList() ?? new List<BookItem>();
            }

            // 대출 이력의 작가, 제목의 키워드로 검색 등
            // 간단하게 최근 대출된 도서의 작가명 첫 단어로 검색
            var latestLoan = loanHistory.Last();
            var authorKeyword = latestLoan.Author.Split(' ').FirstOrDefault() ?? "";

            var searchResponse = await _apiService.GetBooksByQuery(authorKeyword);
            
            var recommendations = searchResponse.BookItems?
                .Where(b => b.Isbn13 != latestLoan.Isbn13) // 제외
                .Take(6)
                .ToList() ?? new List<BookItem>();

            if (recommendations.Count == 0)
            {
                var bestsellers = await _apiService.GetBestsellersAsync();
                recommendations = bestsellers.BookItems?.Take(6).ToList() ?? new List<BookItem>();
            }

            return recommendations;
        }
    }
}
