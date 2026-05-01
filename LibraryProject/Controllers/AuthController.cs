using LibraryProject.Views;
using LibraryProject.Views.Auth;
using System.Windows.Forms;

namespace LibraryProject.Controllers
{
    public class AuthController
    {
        public void ShowAuthView()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var authView = new Auth();
            authView.LoginRequested += (sender, args) => 
            {
                var (id, password) = args;
                if(id == "admin" && password == "admin")
                {
                    authView.Hide();
                    new LibrarianController().ShowLibrarianView();
                } 
                else
                {
                    authView.Hide();
                    new UserController().ShowUserView();
                }
            };

            Application.Run(authView);
        }
    }
}
