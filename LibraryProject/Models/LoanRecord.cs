using System;

namespace LibraryProject.Models
{
    public class LoanRecord
    {
        public string Isbn13 { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CategoryName { get; set; }
        public DateTime LoanDate { get; set; }
    }
}
