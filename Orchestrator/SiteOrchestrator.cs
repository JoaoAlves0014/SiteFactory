using System.Diagnostics;
using SiteFactory.Agents;
using SiteFactory.Agents.Designer;
using SiteFactory.Agents.Developer;
using SiteFactory.Agents.Planner;
using SiteFactory.Agents.Reviewer;
using SiteFactory.Agents.Tester;

namespace SiteFactory.Orchestrator;

/// <summary>
/// Orquestrador principal da fábrica de sites.
/// Coordena o pipeline sequencial de agentes especializados.
/// Fluxo de estados: CREATED -> PLANNING -> DESIGNING -> DEVELOPING -> TESTING -> REVIEWING -> COMPLETED (ou FAILED).
/// </summary>
public class SiteOrchestrator
{
    private readonly string _projectsBaseDirectory;
    private readonly List<IAgent> _pipeline = new();

    public SiteOrchestrator(string projectsBaseDirectory)
    {
        _projectsBaseDirectory = projectsBaseDirectory;

        // Registra o pipeline padrão planejado
        _pipeline.Add(new PlannerAgent());
        _pipeline.Add(new DesignerAgent());
        _pipeline.Add(new DeveloperAgent());
        _pipeline.Add(new TesterAgent());
        _pipeline.Add(new ReviewerAgent());
    }

    /// <summary>
    /// Executa o pipeline completo para a especificação de projeto fornecida.
    /// </summary>
    public async Task<ProjectState> BuildSiteAsync(ProjectSpecification spec, bool enableGit = true, CancellationToken cancellationToken = default)
    {
        var totalStopwatch = Stopwatch.StartNew();

        ConsoleLogger.Banner();
        ConsoleLogger.ProjectHeader(spec.Cliente);

        // 1. Configuração de Diretórios e Estrutura
        var paths = new ProjectPaths(_projectsBaseDirectory, spec.Slug);
        ConsoleLogger.Step($"Criando estrutura de pastas em: {paths.RootPath}");
        paths.EnsureDirectoriesExist();

        // 2. Inicialização do Estado (Status inicial: CREATED)
        var state = new ProjectState
        {
            ProjectName = spec.Cliente,
            Slug = spec.Slug,
            Status = "CREATED",
            CurrentAgent = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Specification = spec
        };

        // Salva estado inicial em project.json
        await state.SaveToFileAsync(paths.ProjectJsonPath);

        // 3. Inicialização do Git Checkpoint Service (opcional, não-bloqueante)
        var gitService = new GitCheckpointService(paths.RootPath, enableGit);
        if (enableGit)
        {
            var gitOk = await gitService.InitializeRepoAsync();
            if (gitOk)
            {
                ConsoleLogger.Step("Repositório Git local inicializado para checkpoints.");
                await gitService.CreateCheckpointAsync("Orchestrator", "Projeto inicializado com especificações do cliente");
            }
        }

        // 4. Criação do Contexto Compartilhado
        var context = new PipelineContext(spec, paths, state, gitService);

        // 5. Execução Sequencial dos Agentes com Transições de Estado
        for (int i = 0; i < _pipeline.Count; i++)
        {
            var agent = _pipeline[i];
            var stepNumber = i + 1;

            // Transição de estado para o agente correspondente
            state.Status = GetAgentStepStatus(agent.Name);
            state.CurrentAgent = agent.Name.ToLowerInvariant();
            await state.SaveToFileAsync(paths.ProjectJsonPath);

            ConsoleLogger.AgentStart(stepNumber, _pipeline.Count, agent.Name);

            try
            {
                var result = await agent.ExecuteAsync(context, cancellationToken);

                if (!result.Success)
                {
                    ConsoleLogger.Error($"Falha na etapa {agent.Name}: {result.Message}");
                    state.Status = "FAILED";
                    state.CurrentAgent = agent.Name.ToLowerInvariant();
                    state.AddStepLog(agent.Name, agent.Role, result.Duration.TotalSeconds, result.GeneratedFiles, result.Message, "Failed");
                    await state.SaveToFileAsync(paths.ProjectJsonPath);
                    return state;
                }

                // Registra log do passo e sucesso
                state.AddStepLog(agent.Name, agent.Role, result.Duration.TotalSeconds, result.GeneratedFiles, result.Message);
                await state.SaveToFileAsync(paths.ProjectJsonPath);

                ConsoleLogger.AgentFinished(agent.Name);

                // Checkpoint Git (não-bloqueante)
                if (enableGit)
                {
                    await gitService.CreateCheckpointAsync(agent.Name, result.Message);
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Exceção não tratada no agente {agent.Name}: {ex.Message}");
                state.Status = "FAILED";
                state.CurrentAgent = agent.Name.ToLowerInvariant();
                state.AddStepLog(agent.Name, agent.Role, 0, new List<string>(), ex.Message, "Exception");
                await state.SaveToFileAsync(paths.ProjectJsonPath);
                throw;
            }
        }

        totalStopwatch.Stop();
        state.Status = "COMPLETED";
        state.CurrentAgent = null;
        state.Metadata["TotalPipelineDurationSeconds"] = totalStopwatch.Elapsed.TotalSeconds.ToString("F2");
        await state.SaveToFileAsync(paths.ProjectJsonPath);

        // 6. Resumo e Finalização
        ConsoleLogger.Success("Projeto concluído.");
        PrintPipelineSummary(context, totalStopwatch.Elapsed);

        return state;
    }

    private static string GetAgentStepStatus(string agentName) => agentName switch
    {
        "Planner" => "PLANNING",
        "Designer" => "DESIGNING",
        "Developer" => "DEVELOPING",
        "Tester" => "TESTING",
        "Reviewer" => "REVIEWING",
        _ => agentName.ToUpperInvariant()
    };

    private static void PrintPipelineSummary(PipelineContext context, TimeSpan totalElapsed)
    {
        ConsoleLogger.Header("RESUMO DA EXECUÇÃO");
        ConsoleLogger.Info($"Tempo total: {totalElapsed.TotalSeconds:F2} segundos.");
        ConsoleLogger.Info($"Diretório do projeto: {context.Paths.RootPath}");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  Artefatos produzidos para supervisão no VS Code:");
        Console.ResetColor();

        foreach (var artifact in context.State.GeneratedArtifacts)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("    - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(artifact);
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  Página principal para validação no navegador:");
        Console.ForegroundColor = ConsoleColor.Green;
        var indexPath = Path.Combine(context.Paths.SrcPath, "index.html");
        Console.WriteLine($"  file:///{indexPath.Replace('\\', '/')}");
        Console.ResetColor();
        Console.WriteLine();
    }
}
