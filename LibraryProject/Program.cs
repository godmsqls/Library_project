using System;
using System.Windows.Forms;
using LibraryProject.Views.Auth;
using LibraryProject.Controllers;
using LibraryProject.Models;

namespace LibraryProject
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var user = new User("testuser9", "1234", "테스트", "Member");
            user.InsertUser();
            MessageBox.Show("삽입 성공");
            //AuthController authController = new AuthController();
            //authController.ShowAuthView();
        }
    }
}
