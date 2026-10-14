using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiteFactory.Orchestrator;

public class StepLog
{
    public string AgentName { get; set; } = string.Empty;
    public string Step { get; set; } = string.Empty;
    public string Status { get; set; } = "Success";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public double DurationSeconds { get; set; }
    public List<string> ProducedFiles { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Representa o estado do projeto ao longo do ciclo de vida do orquestrador.
/// Ciclo de vida: CREATED -> PLANNING -> DESIGNING -> DEVELOPING -> TESTING -> REVIEWING -> COMPLETED (ou FAILED).
/// </summary>
public class ProjectState
{
    public string ProjectName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Status atual do projeto: CREATED, PLANNING, DESIGNING, DEVELOPING, TESTING, REVIEWING, COMPLETED, FAILED
    /// </summary>
    public string Status { get; set; } = "CREATED";

    /// <summary>
    /// Agente que está em execução no momento (ex: "Planner", "Designer") ou null quando ocioso/finalizado.
    /// </summary>
    public string? CurrentAgent { get; set; }

    /// <summary>
    /// Lista dos nomes dos agentes que concluíram com sucesso suas etapas.
    /// </summary>
    public List<string> CompletedSteps { get; set; } = new();

    /// <summary>
    /// Lista dos nomes dos agentes que falharam durante a execução.
    /// </summary>
    public List<string> FailedSteps { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ProjectSpecification Specification { get; set; } = new();
    public List<StepLog> StepHistory { get; set; } = new();
    public List<string> GeneratedArtifacts { get; set; } = new();
    public Dictionary<string, string> Metadata { get; set; } = new();

    public void AddStepLog(string agentName, string step, double durationSeconds, List<string> producedFiles, string notes = "", string status = "Success")
    {
        if (status == "Success")
        {
            if (!CompletedSteps.Contains(agentName))
                CompletedSteps.Add(agentName);
        }
        else
        {
            if (!FailedSteps.Contains(agentName))
                FailedSteps.Add(agentName);
        }

        StepHistory.Add(new StepLog
        {
            AgentName = agentName,
            Step = step,
            Status = status,
            Timestamp = DateTime.UtcNow,
            DurationSeconds = durationSeconds,
            ProducedFiles = producedFiles,
            Notes = notes
        });

        foreach (var file in producedFiles)
        {
            if (!GeneratedArtifacts.Contains(file))
                GeneratedArtifacts.Add(file);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public async Task SaveToFileAsync(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(this, options);
        await File.WriteAllTextAsync(filePath, json);
    }
}
