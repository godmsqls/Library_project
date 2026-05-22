using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Windows.Forms;

namespace LibraryProject.Models
{
    // 검색 또는 대출할 때 캐싱할 Book 클래스입니다.
    // 책을 도서관시스템DB에 우선 저장하고 그 이후 대출,반납,검색 등등의 작업을 수행합니다.
    public class Book
    {
        public string Isbn13 { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int? PubYear { get; set; }
        public int? Category { get; set; }
        public string CoverUrl { get; set; }
        public string Summary { get; set; }

        public Book(string isbn13, string title, string author, string publisher, int? pubYear = null, int? category = null, string coverUrl = null, string summary = null)
        {
            Isbn13 = isbn13;
            Title = title;
            Author = author;
            Publisher = publisher;
            PubYear = pubYear;
            Category = category;
            CoverUrl = coverUrl;
            Summary = summary;
        }

        // 책 등록 (캐싱) 
        public void InsertBook()
        {
            // 1. DB에 연결 하여 쿼리문 장성
            using var conn = Database.Connect();
            conn.Open();
            using var cmd = conn.CreateCommand();

            // 2. INSERT IGNORE로 이미 있는 데이터면 쿼리 무시

            cmd.CommandText = @"
                INSERT IGNORE INTO books 
                (Isbn13, Title, Author, Publisher, PubYear, Category, CoverUrl, Summary) 
                VALUES (@isbn13, @title, @author, @publisher, @pubYear, @category, @coverUrl, @summary)";

            // 3. Book 객체를 DB의 books 테이블에 넣습니다.
            cmd.Parameters.AddWithValue("@isbn13", this.Isbn13);
            cmd.Parameters.AddWithValue("@title", this.Title);
            cmd.Parameters.AddWithValue("@author", (object)this.Author ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@publisher", (object)this.Publisher ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pubYear", (object)this.PubYear ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@category", (object)this.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@coverUrl", (object)this.CoverUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@summary", (object)this.Summary ?? DBNull.Value);

            // 쿼리 실행
            cmd.ExecuteNonQuery();
        }

        // isbn으로 DB에서 조회후 Book 객체로 반환합니다.
        public static Book GetBook(string isbn13)
        {
            //1. DB연결
            using var conn = Database.Connect();
            conn.Open();

            //2. Isbn13이 일치하는 tuple 가져옴
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM books WHERE Isbn13 = @isbn13";
            cmd.Parameters.AddWithValue("@isbn13", isbn13);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
                return ReadBook(reader); // 읽은 데이터를 객체로 변환하여 반환

            return null; // DB에 책이 없으면 null 반환
        }

        // 전체 책 조회
        public static List<Book> GetAllBooks()
        {
            
            var books = new List<Book>();
            // 1.DB연결
            using var conn = Database.Connect();
            conn.Open();
            //2. books 테이블의 모든 tuple 조회
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM books";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) // 데이터가 있는 만큼 반복 하여 books에 저장
                books.Add(ReadBook(reader));

            return books;
        }

        // 전체 books 테이블 출력 (개발 및 테스트용)
        public static void PrintAllBooks()
        {
            List<Book> books = GetAllBooks();
            string result = $"=== 등록된 책 목록 ({books.Count}권) ===\n";
            foreach (var book in books)
                result += $"ISBN: {book.Isbn13} | Title: {book.Title} | Author: {book.Author}\n";

            MessageBox.Show(result);
        }

        // SQL에서 읽어온 데이터(MySqlDataReader)를 Book C# 객체로 변환해주는 헬퍼 메서드. private로 선언
        private static Book ReadBook(MySqlDataReader reader)
        {
            return new Book(
                isbn13: reader.GetString("Isbn13"),
                title: reader.GetString("Title"),
                // DB 상에 NULL로 저장되어 있으면 C#의 null로 변환, 아니면 문자열/숫자로 변환합니다.
                author: reader.IsDBNull(reader.GetOrdinal("Author")) ? null : reader.GetString("Author"),
                publisher: reader.IsDBNull(reader.GetOrdinal("Publisher")) ? null : reader.GetString("Publisher"),
                pubYear: reader.IsDBNull(reader.GetOrdinal("PubYear")) ? null : reader.GetInt32("PubYear"),
                category: reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetInt32("Category"),
                coverUrl: reader.IsDBNull(reader.GetOrdinal("CoverUrl")) ? null : reader.GetString("CoverUrl"),
                summary: reader.IsDBNull(reader.GetOrdinal("Summary")) ? null : reader.GetString("Summary")
            );
        }
    }
}
