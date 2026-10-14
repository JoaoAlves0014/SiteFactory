using System.Text.Json;
using SiteFactory.Orchestrator;

namespace SiteFactory;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            // Determina caminho do arquivo de especificação
            string specFile = "sample-project.json";
            bool enableGit = true;

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i].Trim();
                if (arg == "--")
                {
                    continue;
                }
                else if ((arg == "--spec" || arg == "-s") && i + 1 < args.Length)
                {
                    specFile = args[i + 1];
                    i++;
                }
                else if (arg == "--no-git")
                {
                    enableGit = false;
                }
                else if (!arg.StartsWith("-"))
                {
                    specFile = arg;
                }
            }

            if (!File.Exists(specFile))
            {
                ConsoleLogger.Error($"Arquivo de especificação não encontrado: {specFile}");
                Console.WriteLine("Uso: dotnet run [-- <caminho-json>] [--spec <caminho-json>] [--no-git]");
                Environment.ExitCode = 1;
                return;
            }

            var jsonContent = await File.ReadAllTextAsync(specFile);
            ProjectSpecification? spec;
            try
            {
                spec = JsonSerializer.Deserialize<ProjectSpecification>(jsonContent);
            }
            catch (JsonException ex)
            {
                ConsoleLogger.Error($"Erro ao processar JSON de entrada: {ex.Message}");
                Environment.ExitCode = 1;
                return;
            }

            if (spec == null)
            {
                ConsoleLogger.Error("Não foi possível carregar a especificação a partir do JSON.");
                Environment.ExitCode = 1;
                return;
            }

            // Validação dos dados do projeto
            var (isValid, validationErrors) = spec.Validate();
            if (!isValid)
            {
                ConsoleLogger.Error("Especificação de projeto inválida:");
                foreach (var err in validationErrors)
                {
                    Console.WriteLine($"  - {err}");
                }
                Environment.ExitCode = 1;
                return;
            }

            // Diretório onde os projetos gerados serão armazenados
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Projects");
            var orchestrator = new SiteOrchestrator(baseDirectory);

            var state = await orchestrator.BuildSiteAsync(spec, enableGit);

            if (state.Status != "COMPLETED")
            {
                ConsoleLogger.Error($"O projeto terminou com status: {state.Status}");
                Environment.ExitCode = 1;
            }
        }
        catch (Exception ex)
        {
            ConsoleLogger.Error($"Erro crítico na execução do SiteFactory: {ex.Message}");
            Console.WriteLine(ex.ToString());
            Environment.ExitCode = 2;
        }
    }
}
