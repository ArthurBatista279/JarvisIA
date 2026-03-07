using System.Net.Http.Json;
using System.Text.Json;
using IA_Jarvis_Project.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IA_Jarvis_Project.Infrastructure.AI;

public class GeminiService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string ModelName = "gemini-2.0-flash";

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException(
            "Chave de API do Gemini não encontrada em appsettings.json. " +
            "Adicione: \"Gemini\": { \"ApiKey\": \"SUA_CHAVE_AQUI\" }");
    }

    public async Task<string> GerarRespostaAsync(string mensagem)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{ModelName}:generateContent?key={_apiKey}";

        var requestBody = new
        {
            system_instruction = new
            {
                parts = new[]
                {
                    new { text = "Você é Jarvis, um assistente pessoal inteligente, eficiente e educado. " +
                                 "Responda sempre em português do Brasil de forma clara e concisa." }
                }
            },
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = mensagem }
                    }
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(url, requestBody);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return $"[Erro na API do Gemini: {response.StatusCode} – Verifique sua chave de API]";
        }

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
}
