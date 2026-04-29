using System;
using System.Windows.Forms;
using LibraryProject.Views.Auth;

namespace LibraryProject
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Auth());
        }
    }
}
