using IA_Jarvis_Project.Core.Entities;
using IA_Jarvis_Project.Core.Interfaces;
using IA_Jarvis_Project.Infrastructure.Database;

namespace IA_Jarvis_Project.Infrastructure.Repository;

public class MemoryRepository : IMemoryRepository
{
    private readonly JarvisDbContext _context;

    public MemoryRepository(JarvisDbContext context)
    {
        _context = context;
    }

    public async Task Salvar(MemoryRecord record)
    {
        _context.Memories.Add(record);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MemoryRecord>> ObterTodosAsync()
    {
        return await Task.FromResult(
            _context.Memories
                .OrderByDescending(m => m.Timestamp)
                .ToList()
        );
    }
}