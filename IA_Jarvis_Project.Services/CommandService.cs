namespace IA_Jarvis_Project.Services;

/// <summary>
/// Serviço para execução de tarefas no sistema operacional Windows.
/// 
/// FASE FUTURA – Este serviço permitirá ao Jarvis:
///   - Organizar e mover arquivos
///   - Abrir programas e aplicativos
///   - Analisar conteúdo de pastas
///   - Automatizar tarefas repetitivas
/// 
/// IMPORTANTE: Por segurança, toda ação que altere o sistema passará por
/// uma etapa de confirmação explícita do usuário antes de ser executada.
/// </summary>
public class CommandService
{
    // TODO (Fase 3): Implementar executor de tarefas no Windows
    // Exemplos planejados:
    //   - AbrirPrograma(string nomeProgramaOuCaminho)
    //   - OrganizarArquivos(string pastOrigem, string pastaDestino)
    //   - ListarArquivos(string pasta)
    //   - ExecutarComConfirmacao(string descricao, Func<Task> acao)

    public CommandService()
    {
        // Construtor vazio por enquanto — dependências serão adicionadas na Fase 3
    }

    /// <summary>
    /// Placeholder: verifica se o sistema está apto para executar comandos.
    /// </summary>
    public bool SistemaDisponivel()
    {
        // Na Fase 3, verificará permissões e disponibilidade do sistema
        return false; // Desativado até a Fase 3
    }
}
