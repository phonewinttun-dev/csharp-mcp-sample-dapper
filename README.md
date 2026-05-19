# C# MCP Sample

This project demonstrates how to set up and use the **Model Context Protocol (MCP)** in C# .NET 8. It includes both an MCP Server and an MCP Client to demonstrate communication over Stdio transport.

## Project Structure

The solution (`CSharpMcpSample.slnx`) contains two projects:

### 1. CSharpMcpServerSample
An MCP server that exposes tools to manage simple "Notes" stored in a local SQLite database.
*   **Technologies**: `ModelContextProtocol` SDK, `Dapper`, `Microsoft.Data.Sqlite`.
*   **Tools Exposed**:
    *   `add_note`: Adds a new note with a title and content.
    *   `get_notes`: Retrieves all notes from the database.

### 2. CSharpMcpClientSample
A console application acting as an MCP client. It launches the server as a background process, connects to it via Stdio transport, lists the available tools, and calls them.
*   **Technologies**: `ModelContextProtocol` SDK.

## Project Flow

The interaction between the client and server follows this flow:

1.  **Client Starts**: The `CSharpMcpClientSample` application starts.
2.  **Server Spawns**: The client uses `StdioClientTransport` to launch the `CSharpMcpServerSample` as a child process using `dotnet run`.
3.  **Connection**: The client and server establish a JSON-RPC connection over the server's standard input (stdin) and standard output (stdout).
4.  **Discovery**: The client calls `ListToolsAsync()`, sending a `tools/list` request to the server. The server responds with the schemas of `add_note` and `get_notes`.
5.  **Execution**: 
    *   The client calls `add_note` with arguments. The server inserts the note into the SQLite database and returns the result.
    *   The client calls `get_notes`. The server queries the database and returns the list of notes.

## Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher.

## How to Run

### Run the Client (Automated Test)

The easiest way to test the setup is to run the client project. The client is configured to automatically launch the server project and interact with it.

1.  Open a terminal and navigate to the client directory:
    ```powershell
    cd CSharpMcpClientSample
    ```
2.  Run the project:
    ```powershell
    dotnet run
    ```

You should see output showing the connection, the listed tools, and the results of calling `add_note` and `get_notes`.

### Manual Testing of the Server

If you want to test the server directly without the client:

1.  Navigate to the server directory:
    ```powershell
    cd CSharpMcpServerSample
    ```
2.  Run the server:
    ```powershell
    dotnet run
    ```
    *The console will appear to hang because it is waiting for JSON-RPC input on standard input.*
3.  Paste a JSON-RPC request to list tools:
    ```json
    {"jsonrpc": "2.0", "id": 1, "method": "tools/list", "params": {}}
    ```
    *Press Enter.* You should see a JSON response listing the tools.

## Key Learning Points

*   **Attributes**: The SDK uses `[McpServerToolType]` on classes and `[McpServerTool]` on methods to expose them as tools.
*   **Descriptions**: Descriptions using `[Description]` are crucial as they are passed to the AI model to understand the tool.
*   **Data Types**: SQLite `INTEGER` maps to C# `long`. Using `int` for auto-incrementing IDs with Dapper in SQLite can cause invalid cast exceptions.
