using ModelContextProtocol.Client;
using System;

namespace CsharpMcpClientSample;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Configure the transport to launch your C# MCP server
        var transport = new StdioClientTransport(new StdioClientTransportOptions
        {
            Command = "dotnet",
            Arguments = ["run", "--project", @"D:\Practice\aspdotnetcore\csharp-mcp-sample\CSharpMcpServerSample\CSharpMcpServerSample.csproj"],
            StandardErrorLines = lines => 
            {
                Console.WriteLine($"[Server Error] {lines}");
            }
        });

        Console.WriteLine("Connecting to MCP Server...");
        
        // 2. Create and connect the client
        var mcpClient = await McpClient.CreateAsync(transport);
        Console.WriteLine("Connected!");

        // 3. List available tools
        Console.WriteLine("\n--- Available Tools ---");
        var tools = await mcpClient.ListToolsAsync();
        foreach (var tool in tools)
        {
            Console.WriteLine($"Tool: {tool.Name} - {tool.Description}");
        }

        // 4. Call a tool to add a note
        Console.WriteLine("\n--- Calling add_note ---");
        try 
        {
            var addResult = await mcpClient.CallToolAsync("add_note", new Dictionary<string, object?>()
            {
                ["title"] = "Note from C# Client",
                ["content"] = "This note was created programmatically by a C# client!"
            });

            Console.WriteLine("Add Note Result:");
            Console.WriteLine(addResult.Content.FirstOrDefault()?.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling add_note: {ex.Message}");
        }

        // 5. Call a tool to get all notes
        Console.WriteLine("\n--- Calling get_notes ---");
        try
        {
            var getResult = await mcpClient.CallToolAsync("get_notes", new Dictionary<string, object?>());

            Console.WriteLine("Get Notes Result:");
            Console.WriteLine(getResult.Content.FirstOrDefault()?.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling get_notes: {ex.Message}");
        }

        // Call a tool to add a book
        Console.WriteLine("\n--- Calling add_book ---");
        try 
        {
            var addBookResult = await mcpClient.CallToolAsync("add_book", new Dictionary<string, object?>()
            {
                ["title"] = "The Lord of the Rings",
                ["author"] = "J.R.R. Tolkien",
                ["genre"] = "Fantasy"
            });

            Console.WriteLine("Add Book Result:");
            Console.WriteLine(addBookResult.Content.FirstOrDefault()?.ToString() ?? "Success!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling add_book: {ex.Message}");
        }

        // Call a tool to get all books
        Console.WriteLine("\n--- Calling get_books ---");
        try
        {
            var getBooksResult = await mcpClient.CallToolAsync("get_books", new Dictionary<string, object?>());

            Console.WriteLine("Get Books Result:");
            Console.WriteLine(getBooksResult.Content.FirstOrDefault()?.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling get_books: {ex.Message}");
        }
    }
}
