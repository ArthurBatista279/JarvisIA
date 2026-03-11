using System.Net.Http.Json;
using System.Text.Json;
using IA_Jarvis_Project.Core.Entities;
using IA_Jarvis_Project.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IA_Jarvis_Project.Infrastructure.AI;

public class GeminiService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string ModelName = "gemini-2.0-flash";

    // Numero maximo de mensagens do historico enviadas como contexto
    private const int MaxHistoricoContexto = 10;

    // Tentativas de retry quando a API retorna 429
    private const int MaxTentativas = 3;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException(
            "Chave de API do Gemini nao encontrada em appsettings.json. " +
            "Adicione: \"Gemini\": { \"ApiKey\": \"SUA_CHAVE_AQUI\" }");
    }

    public async Task<string> GerarRespostaAsync(string mensagem, IEnumerable<MemoryRecord>? historico = null)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{ModelName}:generateContent?key={_apiKey}";

        // Monta o historico de conversa para enviar como contexto
        var contents = new List<object>();

        if (historico != null)
        {
            // Pega os ultimos N registros para nao exceder tokens
            var registrosRecentes = historico
                .OrderBy(h => h.Timestamp)
                .TakeLast(MaxHistoricoContexto)
                .ToList();

            foreach (var registro in registrosRecentes)
            {
                // Mensagem do usuario
                contents.Add(new
                {
                    role = "user",
                    parts = new[] { new { text = registro.UserInput } }
                });

                // Resposta do Jarvis (model)
                contents.Add(new
                {
                    role = "model",
                    parts = new[] { new { text = registro.AiResponse } }
                });
            }
        }

        // Adiciona a mensagem atual do usuario
        contents.Add(new
        {
            role = "user",
            parts = new[] { new { text = mensagem } }
        });

        var requestBody = new
        {
            system_instruction = new
            {
                parts = new[]
                {
                    new { text = "Voce e Jarvis, um assistente pessoal inteligente, eficiente e educado. " +
                                 "Responda sempre em portugues do Brasil de forma clara e concisa." }
                }
            },
            contents = contents.ToArray()
        };

        // Retry com backoff exponencial para lidar com 429 TooManyRequests
        for (int tentativa = 1; tentativa <= MaxTentativas; tentativa++)
        {
            var response = await _httpClient.PostAsJsonAsync(url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var json = JsonDocument.Parse(jsonString);

                var texto = json.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return texto ?? "[Resposta vazia da IA]";
            }

            // Se for 429 e ainda ha tentativas restantes, aguarda e tenta novamente
            if ((int)response.StatusCode == 429 && tentativa < MaxTentativas)
            {
                // Backoff exponencial: 4s, 8s, 16s
                var espera = TimeSpan.FromSeconds(Math.Pow(2, tentativa + 1));
                await Task.Delay(espera);
                continue;
            }

            // Outros erros ou esgotou tentativas
            var mensagemErro = (int)response.StatusCode == 429
                ? "[Jarvis: Limite de requisicoes atingido. Aguarde alguns segundos e tente novamente.]"
                : $"[Erro na API do Gemini: {response.StatusCode} - Verifique sua chave de API]";

            return mensagemErro;
        }

        return "[Jarvis: Nao foi possivel obter resposta apos varias tentativas. Tente novamente em breve.]";
    }
}
