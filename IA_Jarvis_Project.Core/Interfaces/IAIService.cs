namespace IA_Jarvis_Project.Core.Interfaces;

public interface IAIService
{
    /// <summary>
    /// Envia uma mensagem para o motor de IA e retorna a resposta gerada.
    /// </summary>
    Task<string> GerarRespostaAsync(string mensagem);
}
