using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LibraryProject.Models
{
    public class Database // DB 서버 연결
    {
        public static MySqlConnection Connect()
        {
            string json = File.ReadAllText(@"C:\Users\el071\Documents\GitHub\Library_project\LibraryProject\config.json"); //각 사용자별로 경로 따로 지정할 것
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            string connString = $"Server={config["server"]};Port={config["port"]};Uid={config["uid"]};Database={config["database"]};Pwd={config["pwd"]};SslMode=Required;";
            return new MySqlConnection(connString);
        }
    }
}
