using SiteFactory.Orchestrator;

namespace SiteFactory.Agents;

/// <summary>
/// Contrato padrão para todos os agentes do SiteFactory.
/// Permite trocar facilmente entre Agentes Simulados (V1) e Agentes de IA reais (Gemini, Claude, Codex, etc.).
/// </summary>
public interface IAgent
{
    /// <summary>
    /// Identificador do agente (ex: "Planner", "Designer", "Developer").
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Papel ou função especializada (ex: "Planejamento Estratégico", "Design UI/UX").
    /// </summary>
    string Role { get; }

    /// <summary>
    /// Descrição sumária das responsabilidades do agente.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Executa o fluxo de trabalho do agente sobre o contexto do projeto.
    /// </summary>
    Task<AgentExecutionResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default);
}
