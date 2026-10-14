namespace SiteFactory.Agents;

/// <summary>
/// Representa o resultado de execução de um agente no pipeline.
/// </summary>
public class AgentExecutionResult
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> GeneratedFiles { get; set; } = new();
    public Dictionary<string, object> OutputData { get; set; } = new();
    public TimeSpan Duration { get; set; }

    public static AgentExecutionResult CreateSuccess(string message, List<string> files, TimeSpan duration, Dictionary<string, object>? data = null)
    {
        return new AgentExecutionResult
        {
            Success = true,
            Message = message,
            GeneratedFiles = files,
            Duration = duration,
            OutputData = data ?? new Dictionary<string, object>()
        };
    }

    public static AgentExecutionResult CreateFailure(string message, TimeSpan duration)
    {
        return new AgentExecutionResult
        {
            Success = false,
            Message = message,
            Duration = duration
        };
    }
}
