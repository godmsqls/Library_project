using System;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryProject.Services;

namespace LibraryProject
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("알라딘 API 서비스 테스트를 시작합니다...");

            using var httpClient = new HttpClient();
            var aladinService = new AladinApiService(httpClient);

            // 예시 카테고리 코드 (351: 컴퓨터)
            string categoryCode = "351"; 

            try
            {
                Console.WriteLine($"\n--- [{categoryCode}] 분류 베스트셀러 목록 조회 ---");
                string bestsellers = await aladinService.GetBestsellersAsync(categoryCode);
                Console.WriteLine(bestsellers);

                Console.WriteLine($"\n--- [{categoryCode}] 분류 신간 도서 목록 조회 ---");
                string newBooks = await aladinService.GetNewBooksAsync(categoryCode);
                Console.WriteLine(newBooks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류가 발생했습니다: {ex.Message}");
                Console.WriteLine("알라딘 API 키를 AladinApiService.cs에 제대로 입력했는지 확인해주세요.");
            }
        }
    }
}
