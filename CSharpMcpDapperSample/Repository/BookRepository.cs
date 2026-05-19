using Dapper;
using Microsoft.Data.Sqlite;
using CSharpMcpDapperSample.Models;

namespace CSharpMcpDapperSample.Repository
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=books.db";

            // Initialize the database table if it doesn't exist
            using var connection = new SqliteConnection(_connectionString);
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Books (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Author TEXT NOT NULL,
                    Genre TEXT NOT NULL
                );");
        }

        public async Task<List<Book>> GetAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            var books = await connection.QueryAsync<Book>("SELECT * FROM Books");
            return books.ToList();
        }

        public async Task AddAsync(string title, string author, string genre)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO Books (Title, Author, Genre) VALUES (@Title, @Author, @Genre)",
                new { Title = title, Author = author, Genre = genre }
            );
        }
    }
}
