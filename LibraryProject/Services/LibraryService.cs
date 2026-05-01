using System;
using System.Collections.Generic;
using System.Linq;
using LibraryProject.Models;
using LibraryProject.Services;

namespace LibraryProject.Services
{
    public class LibraryService
    {
        private static readonly List<LoanRecord> _loanHistory = new List<LoanRecord>();
        private static readonly List<LoanRecord> _currentLoans = new List<LoanRecord>();

        public void LoanBook(BookItem book)
        {
            if (_currentLoans.Any(l => l.Isbn13 == book.Isbn13))
            {
                throw new Exception("이미 대출중인 도서입니다.");
            }

            var record = new LoanRecord
            {
                Isbn13 = book.Isbn13,
                Title = book.Title,
                Author = book.Author,
                CategoryName = book.CategoryName,
                LoanDate = DateTime.Now
            };

            _currentLoans.Add(record);
            _loanHistory.Add(record);
        }

        public void ReturnBook(string isbn13)
        {
            var record = _currentLoans.FirstOrDefault(l => l.Isbn13 == isbn13);
            if (record != null)
            {
                _currentLoans.Remove(record);
            }
        }

        public List<LoanRecord> GetCurrentLoans()
        {
            return _currentLoans.ToList();
        }

        public List<LoanRecord> GetLoanHistory()
        {
            return _loanHistory.ToList();
        }
    }
}
