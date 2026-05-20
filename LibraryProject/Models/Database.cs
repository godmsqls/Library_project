using MySqlConnector;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace LibraryProject.Models
{
    public class Database // DB 서버 연결
    {
        public static MySqlConnection Connect()
        {
            string json = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            string connString = $"Server={config["server"]};Port={config["port"]};Uid={config["uid"]};Database={config["database"]};Pwd={config["pwd"]};SslMode=Required;";
            return new MySqlConnection(connString);
        }
    }
}
