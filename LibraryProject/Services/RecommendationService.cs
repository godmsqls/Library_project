using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace LibraryProject.Services
{
    /// <summary>
    /// 유저가 읽은 책을 기반으로 관련 categoryId를 제시합니다.
    /// </summary>
    public class RecommendationService
    {
        private Dictionary<int, CategoryNode> map;
        private Dictionary<int, int> categoryRead;

        public RecommendationService()
        {
            CategorySetup categorySetup = new CategorySetup();
            map = categorySetup.InitializeAsync().Result;
            categoryRead = new Dictionary<int, int>();
        }
        /// <summary>
        /// 유저가 읽은 책의 기록을 category 단위로 보관합니다. 기본적으로 한 tuple 씩 저장한다고 가정합니다.( DB -> Recommendation System)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="count"></param>
        public void SaveUserReadRecord(int category, int count = 1)
        {
            if (categoryRead.ContainsKey(category))
            {
                categoryRead[category] += count;
            }
            else
            {
                categoryRead.Add(category, count);
            }
        }
        /// <summary>
        /// 유저가 가장 많이 읽은 categoryId를 정해진 depth기반으로 리턴합니다.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public int GetRecommendedCategory(RecommendationDepth depth)
        {
            Dictionary<int, int> record = categoryRead.ToDictionary();
            List<int> keyList = categoryRead.Keys.ToList();
            if (depth == RecommendationDepth.MINOR)
            {
                return record.Max().Key;
            }
            foreach (int key in keyList)
            {
                CategoryNode node = map[key];
                while (node.parent != null)
                {
                    if (node.parent.parent == null) break;
                    node = node.parent;
                }
                if (node.cid == key) continue;
                if (!record.TryAdd(node.cid, record[key]))
                {
                    record[node.cid] += record[key];
                }
                record.Remove(key);
            }
            if(depth == RecommendationDepth.SUB)
            {
                return record.Max().Key;
            }
            keyList = record.Keys.ToList();
            foreach (int key in keyList)
            {
                CategoryNode node = map[key];
                while (node.parent != null) node = node.parent;
                if(node.cid == key) continue;
                if (!record.TryAdd(node.cid, record[key]))
                {
                    record[node.cid] += record[key];
                }
                record.Remove(key);
            }
            return record.Max().Key;
        }
    }
    public class CategorySetup
    {
        private const string JSON_PATH = "Library_Category.json";
        private const string META_PATH = "Library_Category.meta";
        private const string BLOG_URL = "https://blog.aladin.co.kr/openapi";
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<Dictionary<int, CategoryNode>> InitializeAsync()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
            string html = await _httpClient.GetStringAsync(BLOG_URL);
            string sheetId = GetSheetId(html);
            if (sheetId != null && IsUpdateNeeded(sheetId).Result)
            {
                await SetNewCategoryAsync(sheetId);
            }
            return await LoadJson();
        }
        private string GetSheetId(string html)
        {
            var match = Regex.Match(html, @"https://docs.google.com/spreadsheets/d/([a-zA-Z0-9_-]+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }
        private async Task<bool> IsUpdateNeeded(string sheetId)
        {
            if (!File.Exists(META_PATH)) return true;
            var storedSheetId = await File.ReadAllTextAsync(META_PATH);
            return storedSheetId != sheetId;
        }
        private async Task SetNewCategoryAsync(string sheetId)
        {
            string exportURL = $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=xlsx";
            var bytes = await _httpClient.GetByteArrayAsync(exportURL);
            List<CategoryDTO> categories = ParseXlsx(bytes);
            await SaveJsonAsync(sheetId, categories);

        }
        private async Task SaveJsonAsync(string sheetId, List<CategoryDTO> catogories)
        {
            var json = JsonSerializer.Serialize(catogories);
            await File.WriteAllTextAsync(JSON_PATH, json);
            await File.WriteAllTextAsync(META_PATH, sheetId);
        }
        private List<CategoryDTO> ParseXlsx(byte[] bytes)
        {
            Dictionary<int,CategoryDTO> dtos = new Dictionary<int, CategoryDTO>();
            using var stream = new MemoryStream(bytes);
            using var workbook = new XLWorkbook(stream);

            for(int i = 0; i < workbook.Worksheets.Count; i++)
            {
                Mall mall = (Mall)i;
                string mallName = "";
                switch (mall)
                {
                    case Mall.DOMESTIC:
                        mallName = "국내도서";
                        break;
                    case Mall.EBOOK:
                        mallName = "전자책";
                        break;
                    case Mall.GLOBAL:
                        mallName = "외국도서";
                        break;
                }
                if(!workbook.TryGetWorksheet(mallName, out var sheet)) continue;
                foreach (var row in sheet.RowsUsed().Skip(1))
                {
                    var d1Name = row.Cell(1).GetString().Trim();
                    var d1Cid = int.TryParse(row.Cell(2).GetString().Trim(), out int cid1) ? cid1 : 0;
                    var d2Name = row.Cell(3).GetString().Trim();
                    var d2Cid = int.TryParse(row.Cell(4).GetString().Trim(), out int cid2) ? cid2 : 0;
                    var d3Name = row.Cell(5).GetString().Trim();
                    var d3Cid = int.TryParse(row.Cell(6).GetString().Trim(), out int cid3) ? cid3 : 0;

                    CategoryDTO dto = new CategoryDTO() { Cid = d1Cid, Name = d1Name, Mall = mall, parent = null };
                    //1st depth
                    if(d1Cid != 0 && !dtos.TryAdd(d1Cid, dto))
                    {
                        dtos[d1Cid].parent = null;
                    }
                    //2nd depth
                    dto = new CategoryDTO() { Cid = d2Cid, Name = d2Name, Mall = mall, parent = d1Cid };
                    if (d2Cid != 0 && !dtos.TryAdd(d2Cid, dto) && dtos[d2Cid].parent != null)
                    {
                        dtos[d2Cid].parent = d1Cid;
                    }
                    //3rd depth
                    dto = new CategoryDTO() { Cid = d3Cid, Name = d3Name, Mall = mall, parent = d2Cid };
                    if (d3Cid != 0)
                    {
                        dtos.TryAdd(d3Cid, dto);
                    }
                }
            }
            return dtos.Values.ToList();
        }
        private async Task<Dictionary<int, CategoryNode>> LoadJson()
        {
            string json = await File.ReadAllTextAsync(JSON_PATH);

            List<CategoryDTO> dtos = JsonSerializer.Deserialize<List<CategoryDTO>>(json);
            Dictionary<int, CategoryNode> dict = new Dictionary<int, CategoryNode>();
            foreach(CategoryDTO d in dtos)
            {
                CategoryNode node = new CategoryNode();
                node.cid = d.Cid;
                node.name = d.Name;
                node.mall = d.Mall;
                node.children = new List<CategoryNode>();
                node.parent = null;
                dict.Add(d.Cid, node);
            }
            foreach(CategoryDTO d in dtos)
            {
                if(d.parent != null)
                {
                    CategoryNode parentNode = dict[d.parent.Value];
                    parentNode.children.Add(dict[d.Cid]);
                    dict[d.Cid].parent = parentNode;
                }
            }
            return dict;
        }
    }
        
    public class CategoryNode
    {
        public int cid;
        public string name;
        public Mall mall;
        public CategoryNode parent;
        public List<CategoryNode> children;
    }
    public class CategoryDTO
    {
        [JsonPropertyName("cid")]
        public int Cid { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("parent")]
        public int? parent { get; set; }
        [JsonPropertyName("mall")]
        public Mall Mall { get; set; }

    }
    /// <summary>
    /// DOMESTIC : 국내도서, EBOOK : 전자책, GLOBAL : 외국도서
    /// </summary>
    public enum Mall
    {
        DOMESTIC,
        EBOOK,
        GLOBAL
    }
    /// <summary>
    /// MAIN : 대분류, SUB : 중분류, MINOR : 소분류(끝 노드 기반)
    /// </summary>
    public enum RecommendationDepth
    {
        MAIN,
        SUB,
        MINOR
    }
}
