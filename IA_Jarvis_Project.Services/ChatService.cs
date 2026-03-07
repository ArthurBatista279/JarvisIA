 using IA_Jarvis_Project.Core.Entities;
using IA_Jarvis_Project.Core.Interfaces;

namespace IA_Jarvis_Project.Services;

/// <summary>
/// Serviço principal de chat do Jarvis.
/// Orquestra a comunicação com a IA e o armazenamento de memória.
/// </summary>
public class ChatService
{
    private readonly IMemoryRepository _repository;
    private readonly IAIService _aiService;

    public ChatService(IMemoryRepository repository, IAIService aiService)
    {
        _repository = repository;
        _aiService = aiService;
    }

    /// <summary>
    /// Envia uma mensagem ao Jarvis, obtém resposta da IA e salva no histórico.
    /// </summary>
    public async Task<string> EnviarMensagemAsync(string mensagemUsuario)
    {
        // 1. Gera resposta via motor de IA
        var resposta = await _aiService.GerarRespostaAsync(mensagemUsuario);

        // 2. Salva a conversa na memória persistente
        var memoria = new MemoryRecord
        {
            UserInput = mensagemUsuario,
            AiResponse = resposta,
            Timestamp = DateTime.Now
        };
        await _repository.Salvar(memoria);

        return resposta;
    }

    /// <summary>
    /// Retorna todo o histórico de conversas com o Jarvis.
    /// </summary>
    public async Task<List<MemoryRecord>> ObterHistoricoAsync()
    {
        return await _repository.ObterTodosAsync();
    }
}