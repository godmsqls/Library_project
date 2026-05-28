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
                var response = await _apiService.GetBestsellersAsync();
                return response.BookItems?.Take(6).ToList() ?? new List<BookItem>();
            }

            var latestLoan = loanHistory.FirstOrDefault();
            var authorKeyword = latestLoan?.Author?.Split(' ').FirstOrDefault() ?? "";

            var searchResponse = await _apiService.GetBooksByQuery(authorKeyword);
            
            var recommendations = searchResponse.BookItems?
                .Where(b => b.Isbn13 != latestLoan.Isbn13) 
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
