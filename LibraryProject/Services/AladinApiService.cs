using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Collections.Generic;

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
            _apiKey = "ttbscbae93272228001";
        }
        /// <summary>
        /// isbn13번호를 기반으로 책 한권의 정보를 가져옵니다.
        /// </summary>
        public async Task<AladinSearchResponse> GetBookInfoAsync(long isbn13)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemLookUp.aspx?ttbkey={_apiKey}&itemIdType=ISBN13&ItemId={isbn13}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AladinSearchResponse>();
        }
        /// <summary>
        /// 검색어(query)를 기반으로 검색결과 목록을 가져옵니다.
        /// </summary>
        public async Task<AladinSearchResponse> GetBooksByQuery(string query)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemSearch.aspx?ttbkey={_apiKey}&Query={query}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AladinSearchResponse>();
        }
        /// <summary>
        /// 분야 코드(categoryCode)를 기반으로 베스트셀러 목록을 가져옵니다.
        /// categoryCode의 기본 값은 0(전체)입니다.
        /// </summary>
        public async Task<AladinSearchResponse> GetBestsellersAsync(int categoryCode = 0)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemList.aspx?ttbkey={_apiKey}&QueryType=Bestseller&MaxResults=10&start=1&SearchTarget=Book&CategoryId={categoryCode}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AladinSearchResponse>();
        }

        /// <summary>
        /// 분야 코드(categoryCode)를 기반으로 신간 도서 목록을 가져옵니다.
        /// categoryCode의 기본 값은 0(전체)입니다.
        /// </summary>
        public async Task<AladinSearchResponse> GetNewBooksAsync(int categoryCode = 0)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemList.aspx?ttbkey={_apiKey}&QueryType=ItemNewAll&MaxResults=10&start=1&SearchTarget=Book&CategoryId={categoryCode}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AladinSearchResponse>();
        }
    }
    /// <summary>
    /// API 응답정보를 담는 DTO 클래스
    /// </summary>
    public class AladinSearchResponse
    {
        [JsonPropertyName("itemPerPage")]
        public int ItemPerPage { get; set; }

        [JsonPropertyName("totalResults")]
        public int TotalResults {  get; set; }

        [JsonPropertyName("startIndex")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("item")]
        public List<BookItem> BookItems { get; set; }
    }
    /// <summary>
    /// 책의 정보를 담는 DTO 클래스
    /// </summary>
    public class BookItem
    {
        [JsonPropertyName("cover")]
        public string CoverLink { get; set; }

        [JsonPropertyName("isbn13")]
        public string Isbn13 { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }
        
        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; }
        
    }
}
