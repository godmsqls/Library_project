using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        //상품 검색 조회
        //->키워드 기반 검색 목록
        //상품 정보 조회
        //->ISBN 기반 카테고리 정보 조회
        //상품 목록 조회
        //->카테고리 기반 베스트셀러,신간 목록 조회
        
        //번호로 상품 조회
        //ISBN13기반 상품조회
        //보니까 베스트셀러, 신작을 검색할때는 특정 상품의 전체 카테고리가 안보이네
        //그러면 신작 검색 후 그 사람이 해당 책을 조회하면 ISBN13으로 검색해서 값을 받아와야겠구만
        public async Task<string> GetBookInfoAsync(string isbn13)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemLookUp.aspx?ttbkey={_apiKey}&itemIdType=ISBN13&ItemId={isbn13}&output=js&Version=20131101&OptResult=categoryIdList";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetBooksByQuery(string query)
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemSearch.aspx?ttbkey={_apiKey}&Query={query}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 분야 코드(CategoryId)를 기반으로 베스트셀러 목록을 가져옵니다.
        /// </summary>
        public async Task<string> GetBestsellersAsync(string categoryCode = "0")
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemList.aspx?ttbkey={_apiKey}&QueryType=Bestseller&MaxResults=10&start=1&SearchTarget=Book&CategoryId={categoryCode}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 분야 코드(CategoryId)를 기반으로 신간 도서 목록을 가져옵니다.
        /// </summary>
        public async Task<string> GetNewBooksAsync(string categoryCode = "0")
        {
            string url = $"http://www.aladin.co.kr/ttb/api/ItemList.aspx?ttbkey={_apiKey}&QueryType=ItemNewAll&MaxResults=10&start=1&SearchTarget=Book&CategoryId={categoryCode}&output=js&Version=20131101";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
    public class AladinSearchResponse
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("totalResults")]
        public int TotalResults {  get; set; }

        [JsonPropertyName("startIndex")]
        public int TotalPages { get; set; }

        [JsonPropertyName("item")]
        public List<BookItem> BookItems { get; set; }
    }
    public class BookItem
    {
        [JsonPropertyName("isbn13")]
        public string Isbn13 { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }
        
        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("categoryIdList")]
        public List<CategoryInfo>? Categories { get; set; }

    }
    public class CategoryInfo
    {
        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; }
    }
}
