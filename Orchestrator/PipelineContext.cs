namespace SiteFactory.Orchestrator;

/// <summary>
/// Contexto compartilhado fornecido a todos os agentes durante o ciclo de vida do pipeline.
/// </summary>
public class PipelineContext
{
    public ProjectSpecification Specification { get; }
    public ProjectPaths Paths { get; }
    public ProjectState State { get; }
    public GitCheckpointService GitService { get; }
    public Dictionary<string, object> SharedMemory { get; } = new();

    public PipelineContext(ProjectSpecification specification, ProjectPaths paths, ProjectState state, GitCheckpointService gitService)
    {
        Specification = specification;
        Paths = paths;
        State = state;
        GitService = gitService;
    }

    /// <summary>
    /// Salva um artefato produzido por um agente dentro da pasta do projeto.
    /// Registra em log no arquivo do agente em .agents/{agentName}.log
    /// </summary>
    public async Task<string> SaveAgentArtifactAsync(string relativePath, string content, string agentName)
    {
        var fullPath = Path.Combine(Paths.RootPath, relativePath);
        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        await File.WriteAllTextAsync(fullPath, content);

        // Atualiza log do agente específico
        var agentLogPath = Path.Combine(Paths.AgentsPath, $"{agentName.ToLowerInvariant()}.log");
        var logEntry = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Gerou arquivo: {relativePath} ({content.Length} caracteres)\n";
        await File.AppendAllTextAsync(agentLogPath, logEntry);

        ConsoleLogger.ArtifactCreated(relativePath);
        return relativePath;
    }

    /// <summary>
    /// Lê um artefato gerado previamente por outro agente.
    /// </summary>
    public async Task<string?> ReadArtifactAsync(string relativePath)
    {
        var fullPath = Path.Combine(Paths.RootPath, relativePath);
        if (File.Exists(fullPath))
        {
            return await File.ReadAllTextAsync(fullPath);
        }
        return null;
    }
}
