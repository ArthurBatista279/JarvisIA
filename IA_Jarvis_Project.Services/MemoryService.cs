using IA_Jarvis_Project.Core.Entities;
using IA_Jarvis_Project.Core.Interfaces;

namespace IA_Jarvis_Project.Services;

/// <summary>
/// Serviço dedicado ao gerenciamento de memórias do Jarvis.
/// Responsabilidade separada do ChatService para manter o código organizado.
/// </summary>
public class MemoryService
{
    private readonly IMemoryRepository _repository;

    public MemoryService(IMemoryRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Obtém todas as memórias armazenadas, ordenadas da mais recente para a mais antiga.
    /// </summary>
    public async Task<List<MemoryRecord>> ObterTodasMemorias()
    {
        return await _repository.ObterTodosAsync();
    }

    /// <summary>
    /// Salva um registro de memória diretamente.
    /// </summary>
    public async Task SalvarMemoria(string entrada, string resposta)
    {
        var record = new MemoryRecord
        {
            UserInput = entrada,
            AiResponse = resposta,
            Timestamp = DateTime.Now
        };
        await _repository.Salvar(record);
    }
}
