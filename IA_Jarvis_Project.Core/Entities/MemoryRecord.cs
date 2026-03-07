namespace IA_Jarvis_Project.Core.Entities;

public class MemoryRecord
{
    public int Id { get; set; }
    public string UserInput { get; set; } = string.Empty;
    public string AiResponse { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
}