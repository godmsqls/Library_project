using LibraryProject.Views;

namespace LibraryProject.Controllers
{
    public class LibrarianController
    {
        public void ShowLibrarianView()
        {
            var view = new Librarian();
            view.Show();
        }
    }
}
