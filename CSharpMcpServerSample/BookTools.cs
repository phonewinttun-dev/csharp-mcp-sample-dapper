using System.ComponentModel;
using ModelContextProtocol.Server;
using CSharpMcpDapperSample.Services;
using CSharpMcpDapperSample.Models;

namespace csharp_mcp_sample;

[McpServerToolType]
public class BookTools {
    private readonly BookRepository _bookRepository;
    
    public BookTools(BookRepository service)
    {
        _bookRepository = service;
    }
    
    [McpServerTool]
    [Description("Get all books from the database")]
    public async Task<IEnumerable<Book>> GetBooks()
    {
        return await _bookRepository.GetAsync();
    }
    
    [McpServerTool]
    [Description("Add a new book to the database")]
    public async Task AddBook
    (
        [Description("The title of the book")] string title,
        [Description("The author of the book")] string author,
        [Description("The genre of the book")] string genre
    )
    {
        await _bookRepository.AddAsync(title, author, genre);
    }
}
