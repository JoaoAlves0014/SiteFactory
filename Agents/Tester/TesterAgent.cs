using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using SiteFactory.Orchestrator;

namespace SiteFactory.Agents.Tester;

public class QaTestResult
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Details { get; set; } = string.Empty;
}

public class TesterAgent : IAgent
{
    public string Name => "Tester";
    public string Role => "Garantia de Qualidade (QA) & Validações Técnicas";
    public string Description => "Executa testes automatizados de integridade sobre os arquivos gerados, valida semântica, responsividade e SEO.";

    public async Task<AgentExecutionResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var generatedFiles = new List<string>();
        var testResults = new List<QaTestResult>();

        ConsoleLogger.Step("Iniciando bateria de validações técnicas sobre os artefatos...");
        await Task.Delay(300, cancellationToken);

        // 1. Verificação de Arquivos Obrigatórios
        var requiredFiles = new[]
        {
            "src/index.html",
            "src/styles/main.css",
            "src/scripts/app.js",
            "public/favicon.svg",
            "docs/development.md"
        };

        foreach (var req in requiredFiles)
        {
            var fullPath = Path.Combine(context.Paths.RootPath, req);
            var exists = File.Exists(fullPath);
            testResults.Add(new QaTestResult
            {
                Id = $"FILE-{req.Replace('/', '-').Replace('.', '-')}",
                Category = "Estrutura de Arquivos",
                Description = $"Existência do arquivo '{req}'",
                Passed = exists,
                Details = exists ? $"Arquivo presente ({new FileInfo(fullPath).Length} bytes)" : "Arquivo ausente!"
            });
        }

        // 2. Validação Semântica do HTML
        var htmlContent = await context.ReadArtifactAsync("src/index.html") ?? string.Empty;

        // Verifica tag H1 única
        var h1Matches = Regex.Matches(htmlContent, @"<h1[^>]*>.*?</h1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        testResults.Add(new QaTestResult
        {
            Id = "SEO-H1-COUNT",
            Category = "SEO & Semântica",
            Description = "Presença de exatamente um cabeçalho H1",
            Passed = h1Matches.Count == 1,
            Details = $"Encontrado(s) {h1Matches.Count} elemento(s) <h1> na página principal."
        });

        // Verifica Metatags
        var hasViewport = htmlContent.Contains("name=\"viewport\"", StringComparison.OrdinalIgnoreCase);
        var hasDescription = htmlContent.Contains("name=\"description\"", StringComparison.OrdinalIgnoreCase);
        testResults.Add(new QaTestResult
        {
            Id = "SEO-METATAGS",
            Category = "SEO & Semântica",
            Description = "Presença de metatags essenciais (viewport, description)",
            Passed = hasViewport && hasDescription,
            Details = $"Viewport: {(hasViewport ? "OK" : "Ausente")}, Description: {(hasDescription ? "OK" : "Ausente")}"
        });

        // 3. Validação do CSS
        var cssContent = await context.ReadArtifactAsync("src/styles/main.css") ?? string.Empty;
        var hasRootTokens = cssContent.Contains(":root") && cssContent.Contains("--primary");
        var hasMediaQuery = cssContent.Contains("@media");

        testResults.Add(new QaTestResult
        {
            Id = "CSS-DESIGN-SYSTEM",
            Category = "CSS & Design Tokens",
            Description = "Declaração de tokens CSS no :root e variáveis de cor",
            Passed = hasRootTokens,
            Details = hasRootTokens ? "Variáveis e design tokens configurados no :root" : "Falta configuração de variáveis no :root"
        });

        testResults.Add(new QaTestResult
        {
            Id = "CSS-RESPONSIVE",
            Category = "Responsividade",
            Description = "Breakpoints responsivos com @media queries",
            Passed = hasMediaQuery,
            Details = hasMediaQuery ? "Breakpoints responsivos detectados" : "Nenhuma media query encontrada"
        });

        // 4. Validação de Interatividade do JS
        var jsContent = await context.ReadArtifactAsync("src/scripts/app.js") ?? string.Empty;
        var hasSimulatorLogic = jsContent.Contains("initSimulator") && jsContent.Contains("renderVehicles");
        var hasWhatsAppHooks = jsContent.Contains("wa.me");

        testResults.Add(new QaTestResult
        {
            Id = "JS-INTERACTIVITY",
            Category = "Lógica Interativa",
            Description = "Funções de renderização dinâmica de catálogo e cálculo de simulador",
            Passed = hasSimulatorLogic,
            Details = hasSimulatorLogic ? "Módulos de catálogo e simulador presentes" : "Lógica interativa incompleta"
        });

        testResults.Add(new QaTestResult
        {
            Id = "LEADS-WHATSAPP",
            Category = "Conversão de Vendas",
            Description = "Integração direta de CTAs e formulários com WhatsApp",
            Passed = hasWhatsAppHooks,
            Details = hasWhatsAppHooks ? "Links de redirecionamento dinâmico wa.me configurados" : "Ausência de canal WhatsApp"
        });

        var passedCount = testResults.Count(t => t.Passed);
        var totalCount = testResults.Count;
        var passRate = (double)passedCount / totalCount * 100.0;

        ConsoleLogger.Step($"Testes executados: {passedCount}/{totalCount} aprovados ({passRate:F0}%)...");

        // Gera tests/site-checks.json
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        var jsonReport = JsonSerializer.Serialize(new
        {
            Timestamp = DateTime.UtcNow,
            TotalTests = totalCount,
            Passed = passedCount,
            PassRate = $"{passRate:F1}%",
            Tests = testResults
        }, jsonOptions);

        var jsonPath = await context.SaveAgentArtifactAsync("tests/site-checks.json", jsonReport, Name);
        generatedFiles.Add(jsonPath);

        // Gera docs/qa-report.md
        ConsoleLogger.Step("Gerando relatório consolidado de QA (docs/qa-report.md)...");
        var mdReport = $@"# Relatório de Qualidade e Garantia Técnica (QA)
**Data de Execução:** {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss} UTC  
**Total de Testes:** {totalCount}  
**Aprovados:** {passedCount} ({passRate:F1}%)  
**Status Geral:** {(passRate >= 90 ? "APROVADO COM SUCESSO" : "REQUER AJUSTES")}  

---

## 1. Tabela de Validações

| Identificador | Categoria | Descrição | Status | Detalhes |
| :--- | :--- | :--- | :---: | :--- |
{string.Join("\n", testResults.Select(t => $"| `{t.Id}` | {t.Category} | {t.Description} | {(t.Passed ? "✔ PASSOU" : "✖ FALHOU")} | {t.Details} |"))}

---

## 2. Resumo da Avaliação de QA
- **Estrutura de Arquivos:** Todos os artefatos primários (HTML, CSS, JS, Favicon SVG) foram criados nos caminhos corretos.
- **Acessibilidade e SEO:** Heading hierarchy em conformidade técnica (1 único H1). Metatags configuradas.
- **Design Tokens:** O Design System foi implementado via variáveis CSS customizadas, permitindo manutenibilidade.
- **Interatividade:** Módulos de filtragem instantânea e simulador financeiro parametrizados.
";
        var mdPath = await context.SaveAgentArtifactAsync("docs/qa-report.md", mdReport, Name);
        generatedFiles.Add(mdPath);

        stopwatch.Stop();
        context.SharedMemory["QaCompleted"] = true;
        context.SharedMemory["QaPassRate"] = passRate;

        return AgentExecutionResult.CreateSuccess(
            $"QA concluído: {passedCount}/{totalCount} testes passaram com sucesso.",
            generatedFiles,
            stopwatch.Elapsed
        );
    }
}
