namespace LibraryProject.Models
{
    public class User
    {
        public int UserId { get; private set; }        // DB 자동 부여 PK
        public string UserLoginId { get; set; }         // 로그인 아이디
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
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
    }
}
