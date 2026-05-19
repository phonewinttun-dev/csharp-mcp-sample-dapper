using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol;
using csharp_mcp_sample;
using CSharpMcpDapperSample.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<NoteRepository>();
builder.Services.AddSingleton<NoteTools>();
builder.Services.AddSingleton<BookRepository>();
builder.Services.AddSingleton<BookTools>();

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();
await app.RunAsync();
