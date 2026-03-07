using IA_Jarvis_Project.Core.Entities;
using IA_Jarvis_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IA_Jarvis_Project.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ChatService _chatService;

    public IndexModel(ChatService chatService)
    {
        _chatService = chatService;
    }

    [BindProperty]
    public string MensagemUsuario { get; set; } = string.Empty;

    public List<MemoryRecord> Historico { get; set; } = new();

    public string? UltimaResposta { get; set; }

    public async Task OnGetAsync()
    {
        Historico = await _chatService.ObterHistoricoAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(MensagemUsuario))
        {
            Historico = await _chatService.ObterHistoricoAsync();
            return Page();
        }

        UltimaResposta = await _chatService.EnviarMensagemAsync(MensagemUsuario);
        Historico = await _chatService.ObterHistoricoAsync();

        return Page();
    }
}
