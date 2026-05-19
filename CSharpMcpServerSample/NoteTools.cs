using System.ComponentModel;
using ModelContextProtocol.Server;

namespace csharp_mcp_sample;

[McpServerToolType]
public class NoteTools {
    private readonly NoteRepository _repository;
    public NoteTools(NoteRepository repository) {
        _repository = repository;
    }
    
    [McpServerTool]
    [Description("Get all notes")]
    public async Task<IEnumerable<Note>> GetNotes() {
        return await _repository.GetNotesAsync();
    }
    
    [McpServerTool]
    [Description("Add a new note")]
    public async Task<int> AddNote(
        [Description("The title of the note")] string title, 
        [Description("The content of the note")] string content) {
        return await _repository.AddNoteAsync(title, content);
    }
}
