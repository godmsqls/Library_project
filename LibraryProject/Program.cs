using System;
using System.Windows.Forms;
using LibraryProject.Views.Auth;

namespace LibraryProject
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //로그인 화면(Auth) 실행
            Application.Run(new Auth());
        }
    }
}
