namespace LibraryProject.Models
{
    public class User
    {
        private string userId;
        private string name;
        private string role; // User ¶Ç´Â Librarian

        private string UserId
        {
            get => userId;
            set => userId = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }
        public string Role
        {
            get => role;
            set => role = value;
        }
        public User(string id, string name, string role)
        {
            this.UserId = id;
            this.Name = name;
            this.Role = role;
        }
    }
}
