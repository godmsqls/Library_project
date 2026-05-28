using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Windows.Forms;

namespace LibraryProject.Models
{
    // 대출 현황 및 이력에 대한 클래스입니다. 
    public class LoanRecord
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public string Isbn13 { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }


        // 'JOIN ON isbn13' 을 통해 책 테이블(books)에서 가져올 추가 정보들
        public string Title { get; set; }
        public string Author { get; set; }
        public string CategoryName { get; set; }

        
        public LoanRecord() { }

        // DB에서 읽어온 데이터(JOIN 포함)를 매개변수로 받아서 객체를 생성.
        public LoanRecord(int loanId, int userId, string isbn13, string title, string author, string categoryName, DateTime loanDate, DateTime dueDate, DateTime? returnDate = null)
        {
            LoanId = loanId;
            UserId = userId;
            Isbn13 = isbn13;
            Title = title;
            Author = author;
            CategoryName = categoryName;
            LoanDate = loanDate;
            DueDate = dueDate;
            ReturnDate = returnDate;
        }

        // 대출 등록
        public static void InsertLoan(int userId, Book book, DateTime dueDate)
        {
            book.InsertBook(); // 우선 books를 캐싱해서 저장. 이미 존재한다면 무시.

            using var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();

            // loans테이블에 대출내역 저장
            cmd.CommandText = "INSERT INTO loans (UserId, Isbn13, LoanDate, DueDate) VALUES (@userId, @isbn13, @loanDate, @dueDate)";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@isbn13", book.Isbn13);
            cmd.Parameters.AddWithValue("@loanDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@dueDate", dueDate);

            cmd.ExecuteNonQuery();
        }

        // 반납 처리 -> RetrunDate를 null에서 현재 시간으로 변경
        public static void ReturnBook(int loanId)
        {
            using var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE loans SET ReturnDate = @returnDate WHERE LoanId = @loanId";
            cmd.Parameters.AddWithValue("@returnDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@loanId", loanId);

            cmd.ExecuteNonQuery();
        }

        // 특정 유저의 대출 이력 조회
        public static List<LoanRecord> GetLoansByUser(int userId)
        {
           // Title, Author, Category값이 있는 LoanRecord 객체로 반환.
            var loans = new List<LoanRecord>();
            using var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();

            // Join을 통해 Title, Author, Category값을 조회 
            cmd.CommandText = @"
                SELECT l.LoanId, l.UserId, l.LoanDate, l.DueDate, l.ReturnDate,
                       b.Isbn13, b.Title, b.Author, b.Category
                FROM loans l
                JOIN books b ON l.Isbn13 = b.Isbn13
                WHERE l.UserId = @userId
                ORDER BY l.LoanDate DESC";

            cmd.Parameters.AddWithValue("@userId", userId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                loans.Add(ReadLoan(reader));

            return loans;
        }

        // 전체 대출 이력 조회
        public static List<LoanRecord> GetAllLoans()
        {

            // Title, Author, Category값이 있는 LoanRecord 객체로 반환.
            var loans = new List<LoanRecord>();
            using var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();

            // Join을 통해 Title, Author, Category값을 조회
            cmd.CommandText = @"
                SELECT l.LoanId, l.UserId, l.LoanDate, l.DueDate, l.ReturnDate,
                       b.Isbn13, b.Title, b.Author, b.Category
                FROM loans l
                JOIN books b ON l.Isbn13 = b.Isbn13
                ORDER BY l.LoanDate DESC";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                loans.Add(ReadLoan(reader));

            return loans;
        }

        // 연체 중인 대출만 조회
        public static List<LoanRecord> GetOverdueLoans()
        {
            // Title, Author, Category값이 있는 LoanRecord 객체로 반환.
            var loans = new List<LoanRecord>();
            using var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();

            // Join을 통해 Title, Author, Category값을 조회
            cmd.CommandText = @"
                SELECT l.LoanId, l.UserId, l.LoanDate, l.DueDate, l.ReturnDate,
                       b.Isbn13, b.Title, b.Author, b.Category
                FROM loans l
                JOIN books b ON l.Isbn13 = b.Isbn13
                WHERE l.ReturnDate IS NULL AND l.DueDate < @now
                ORDER BY l.DueDate ASC";

            cmd.Parameters.AddWithValue("@now", DateTime.Now);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                loans.Add(ReadLoan(reader));

            return loans;
        }

        // 전체 loans 테이블을 LoanRecord 객체로 만들어서 출력. (테스트 용)
        public static void PrintAllLoans()
        {
            List<LoanRecord> loans = GetAllLoans();
            string result = $"=== 대출 목록 ({loans.Count}건) ===\n";
            foreach (var loan in loans)
            {              
                result += $"LoanId: {loan.LoanId} | Title: {loan.Title} | ReturnDate: {loan.ReturnDate?.ToString("yyyy-MM-dd") ?? "NULL"}\n";
            }

            MessageBox.Show(result);
        }

        // SQL에서 읽어온 데이터(MySqlDataReader)를 LoanRecord C# 객체로 변환해주는 메서드. private
        private static LoanRecord ReadLoan(MySqlDataReader reader)
        {
            return new LoanRecord(
                loanId: reader.GetInt32("LoanId"),
                userId: reader.GetInt32("UserId"),
                isbn13: reader.GetString("Isbn13"),
                title: reader.GetString("Title"),
                author: reader.IsDBNull(reader.GetOrdinal("Author")) ? null : reader.GetString("Author"),
                categoryName: reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetInt32("Category").ToString(),
                loanDate: reader.GetDateTime("LoanDate"),
                dueDate: reader.GetDateTime("DueDate"),
                returnDate: reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? null : (DateTime?)reader.GetDateTime("ReturnDate")
            );
        }
    }
} 
