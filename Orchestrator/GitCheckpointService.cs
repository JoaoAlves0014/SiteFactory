using System.Diagnostics;

namespace SiteFactory.Orchestrator;

/// <summary>
/// Serviço de controle de versão Git para registrar checkpoints e históricos das etapas dos agentes.
/// </summary>
public class GitCheckpointService
{
    private readonly bool _enabled;
    private readonly string _workingDirectory;

    public GitCheckpointService(string workingDirectory, bool enabled = true)
    {
        _workingDirectory = workingDirectory;
        _enabled = enabled;
    }

    /// <summary>
    /// Inicializa repositório Git no diretório do projeto, caso ainda não exista.
    /// </summary>
    public async Task<bool> InitializeRepoAsync()
    {
        if (!_enabled) return false;

        try
        {
            var gitDir = Path.Combine(_workingDirectory, ".git");
            if (Directory.Exists(gitDir))
                return true;

            var (code, _) = await RunGitCommandAsync("init");
            if (code == 0)
            {
                // Configura usuário local para o repositório do projeto
                await RunGitCommandAsync("config user.name \"SiteFactory Agent\"");
                await RunGitCommandAsync("config user.email \"agent@sitefactory.local\"");
                return true;
            }
        }
        catch
        {
            // Git pode não estar configurado ou sem permissão
        }

        return false;
    }

    /// <summary>
    /// Cria um checkpoint (commit) com a mensagem da etapa executada.
    /// </summary>
    public async Task<bool> CreateCheckpointAsync(string agentName, string message)
    {
        if (!_enabled) return false;

        try
        {
            var (addCode, _) = await RunGitCommandAsync("add .");
            if (addCode != 0) return false;

            var commitMsg = $"[SiteFactory] {agentName}: {message}";
            var (commitCode, _) = await RunGitCommandAsync($"commit -m \"{commitMsg}\"");
            return commitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private async Task<(int ExitCode, string Output)> RunGitCommandAsync(string arguments)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                WorkingDirectory = _workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();

            var stdoutTask = process.StandardOutput.ReadToEndAsync();
            var stderrTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
            var stdout = await stdoutTask;
            var stderr = await stderrTask;

            return (process.ExitCode, stdout + stderr);
        }
        catch (Exception ex)
        {
            return (-1, ex.Message);
        }
    }
}
