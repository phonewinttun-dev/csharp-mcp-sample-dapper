using Microsoft.Data.Sqlite;
using Dapper;

namespace csharp_mcp_sample;

public class NoteRepository {
    private readonly string _connectionString = "Data Source=notes.db";
    public NoteRepository() {
        using var connection = new SqliteConnection(_connectionString);
        connection.Execute("CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, Content TEXT)");
    }
    public async Task<IEnumerable<Note>> GetNotesAsync() {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<Note>("SELECT * FROM Notes");
    }
    public async Task<int> AddNoteAsync(string title, string content) {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.ExecuteAsync("INSERT INTO Notes (Title, Content) VALUES (@Title, @Content)", new { Title = title, Content = content });
    }
}
