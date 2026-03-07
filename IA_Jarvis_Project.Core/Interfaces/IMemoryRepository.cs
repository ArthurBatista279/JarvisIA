using IA_Jarvis_Project.Core.Entities;

namespace IA_Jarvis_Project.Core.Interfaces;

public interface IMemoryRepository
{
    Task Salvar(MemoryRecord record);
    Task<List<MemoryRecord>> ObterTodosAsync();
}