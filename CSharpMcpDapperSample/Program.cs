using CSharpMcpDapperSample.Data;
using CSharpMcpDapperSample.Models;
using CSharpMcpDapperSample.Repository;
using ModelContextProtocol.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BookRepository>();

// Register McpClient as a DI singleton that connects to the CSharpMcpServerSample project
builder.Services.AddSingleton<McpClient>(sp =>
{
    var transport = new StdioClientTransport(new StdioClientTransportOptions
    {
        Command = "dotnet",
        Arguments = [@"D:\Practice\aspdotnetcore\csharp-mcp-sample\CSharpMcpServerSample\bin\Debug\net8.0\CSharpMcpServerSample.dll"]
    });
    return McpClient.CreateAsync(transport).GetAwaiter().GetResult();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var bookService = scope.ServiceProvider.GetRequiredService<BookRepository>();

    await DbInitializer.Initialize(bookService);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

// Minimal API Routes
app.MapGet("/api/book", async (BookRepository service) =>
{
    var result = await service.GetAsync();
    return Results.Ok(result);
});

// MCP Web API Testing Routes
app.MapGet("/api/mcp/tools", async (McpClient mcpClient) =>
{
    var tools = await mcpClient.ListToolsAsync();
    return Results.Ok(tools);
});

// formatted response body
app.MapGet("/api/mcp/books", async (McpClient mcpClient) =>
{
    var result = await mcpClient.CallToolAsync("get_books", new Dictionary<string, object?>());
    var firstContent = result.Content.FirstOrDefault();
    if (firstContent != null)
    {
        var serialized = System.Text.Json.JsonSerializer.Serialize(firstContent);
        using var doc = System.Text.Json.JsonDocument.Parse(serialized);
        if (doc.RootElement.TryGetProperty("text", out var textProp))
        {
            var rawJson = textProp.GetString();
            if (!string.IsNullOrEmpty(rawJson))
            {
                var books = System.Text.Json.JsonSerializer.Deserialize<List<Book>>(rawJson, new System.Text.Json.JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                return Results.Ok(books);
            }
        }
    }
    return Results.Ok(result.Content);
});

app.MapPost("/api/mcp/books", async (McpClient mcpClient, AddBookRequest request) =>
{
    var result = await mcpClient.CallToolAsync("add_book", new Dictionary<string, object?>()
    {
        ["title"] = request.Title,
        ["author"] = request.Author,
        ["genre"] = request.Genre
    });
    return Results.Ok(result.Content);
});

app.Run();

// Request DTO for adding a book via Swagger
public record AddBookRequest(string Title, string Author, string Genre);

