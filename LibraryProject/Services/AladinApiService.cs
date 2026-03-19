using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LibraryProject.Services
{
    public class AladinApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AladinApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // TODO: 실제 환경에서는 설정 파일(appsettings.json)이나 환경 변수에서 API 키를 주입받아야 합니다.
            _apiKey = "ttbel07101608001";
        }

        /// <summary>
        /// 분야 코드(CategoryId)를 기반으로 베스트셀러 목록을 가져옵니다.
        /// </summary>
        public async Task<string> GetBestsellersAsync(string categoryCode)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemList.aspx?ttbkey={_apiKey}&QueryType=Bestseller&MaxResults=10&start=1&SearchTarget=Book&CategoryId={categoryCode}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 분야 코드(CategoryId)를 기반으로 신간 도서 목록을 가져옵니다.
        /// </summary>
        public async Task<string> GetNewBooksAsync(string categoryCode)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemList.aspx?ttbkey={_apiKey}&QueryType=ItemNewAll&MaxResults=10&start=1&SearchTarget=Book&CategoryId={categoryCode}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
