using System;
using System.Windows.Forms;
using LibraryProject.Views.Auth;
using LibraryProject.Controllers;

namespace LibraryProject
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            AuthController authController = new AuthController();
            authController.ShowAuthView();
        }
    }
}
