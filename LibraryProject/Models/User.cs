using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Windows.Forms;

namespace LibraryProject.Models
{
    public class User
    {
        public int UserId { get; private set; }        // DB 자동 부여 PK
        public string UserLoginId { get; set; }         // 로그인 아이디
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }            //'Member' 또는 'Admin' 만 가능
        public string Email { get; set; }
        public int? PreferCategory { get; set; }

        public User(string userLoginId, string password, string name, string role, string email = null, int? preferCategory = null)
        {
            UserLoginId = userLoginId;
            Password = password;
            Name = name;
            Role = role;
            Email = email;
            PreferCategory = preferCategory;
        }

        public void SetUserId(int id)
        {
            UserId = id;
        }

        // 유저 등록
        public void InsertUser()
        {
            var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO users (userLoginId, password, name, email, role, preferCategory) VALUES (@userLoginId, @password, @name, @email, @role, @preferCategory)";
            cmd.Parameters.AddWithValue("@userLoginId", this.UserLoginId);
            cmd.Parameters.AddWithValue("@password", this.Password);
            cmd.Parameters.AddWithValue("@name", this.Name);
            cmd.Parameters.AddWithValue("@email", this.Email);
            cmd.Parameters.AddWithValue("@role", this.Role);
            cmd.Parameters.AddWithValue("@preferCategory", this.PreferCategory);
            cmd.ExecuteNonQuery();
        }

        //Login_id 로 유저 조회(반환)
        public static User GetUser(string userLoginId)
        {
            var conn = Database.Connect(); 
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE userLoginId = @userLoginId";
            cmd.Parameters.AddWithValue("@userLoginId", userLoginId);

            using var reader = cmd.ExecuteReader(); 
            if (reader.Read())
            {
                var user = new User( 
                    userLoginId: reader.GetString("userLoginId"),
                    password: reader.GetString("password"),
                    name: reader.GetString("name"),
                    role: reader.GetString("role"),
                    email: reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    preferCategory: reader.IsDBNull(reader.GetOrdinal("preferCategory")) ? null : reader.GetInt32("preferCategory")
                );
                user.SetUserId(reader.GetInt32("UserId"));
                return user;
            }
            return null;
        }

        public static User GetUserById(int userId)
        {
            var conn = Database.Connect(); 
            conn.Open();
            using var cmd = conn.CreateCommand(); 
            cmd.CommandText = "SELECT * FROM users WHERE UserId = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var user = new User( 
                    userLoginId: reader.GetString("userLoginId"),
                    password: reader.GetString("password"),
                    name: reader.GetString("name"),
                    role: reader.GetString("role"),
                    email: reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    preferCategory: reader.IsDBNull(reader.GetOrdinal("preferCategory")) ? null : reader.GetInt32("preferCategory")
                );
                user.SetUserId(reader.GetInt32("UserId"));
                return user;
            }
            return null;
        }

        // 모든 유저를 리스트로 받아서 반환
        public static List<User> GetAllUsers()
        {
            var users = new List<User>(); //user들의 리스트
            var conn = Database.Connect(); //쿼리 명령 생성
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users";

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) //쿼리 결과를 한 줄 씩 읽으며
            {
                users.Add(new User( //결과에 해당하는 User 객체 생성 후 반환 -> list에 추가
                    userLoginId: reader.GetString("userLoginId"),
                    password: reader.GetString("password"),
                    name: reader.GetString("name"),
                    role: reader.GetString("role"),
                    email: reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    preferCategory: reader.IsDBNull(reader.GetOrdinal("preferCategory")) ? null : reader.GetInt32("preferCategory")
                ));
            }
            return users;
        }


        //유저 전체 목록 출력(테스트 용)
        public static void PrintAllUsers()
        {
            List<User> users = GetAllUsers();
            string result = $"=== 등록된 유저 목록 ({users.Count}명) ===\n";
            foreach (var user in users)
                result += $"ID: {user.UserLoginId} | 이름: {user.Name} | 권한: {user.Role}\n";

            MessageBox.Show(result);
        }

        //유저 전체 삭제 (테스트 용)
        public static void DeleteAllUsers()
        {
            var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM users";
            cmd.ExecuteNonQuery();
        }
    }
}
