using IA_Jarvis_Project.Core.Entities;

namespace IA_Jarvis_Project.Core.Interfaces;

public interface IAIService
{
    /// <summary>
    /// Envia uma mensagem para o motor de IA com histórico de conversa e retorna a resposta gerada.
    /// </summary>
    Task<string> GerarRespostaAsync(string mensagem, IEnumerable<MemoryRecord>? historico = null);
}
