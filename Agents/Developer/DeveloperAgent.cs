using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using SiteFactory.Orchestrator;

namespace SiteFactory.Agents.Developer;

public class DeveloperAgent : IAgent
{
    public string Name => "Developer";
    public string Role => "Desenvolvimento Frontend e Arquitetura Web";
    public string Description => "Implementa código limpo a partir de templates físicos reutilizáveis, personalizando o conteúdo e o visual a partir dos dados do projeto.";

    public async Task<AgentExecutionResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var spec = context.Specification;
        var generatedFiles = new List<string>();

        // 1. Identifica e localiza o template físico com proteção de segurança
        var templateIdentifier = !string.IsNullOrWhiteSpace(spec.Template) ? spec.Template : spec.Tipo;
        ConsoleLogger.Step($"Localizando template físico para o projeto: '{templateIdentifier}'...");
        await Task.Delay(200, cancellationToken);

        var templateDir = ResolveTemplateDirectory(templateIdentifier);
        ConsoleLogger.Step($"Template encontrado em: {templateDir}");

        // 2. Valida e copia public/favicon.svg (Origem oficial: assets/favicon.svg)
        ConsoleLogger.Step("Carregando favicon vetorial do template (public/favicon.svg)...");
        var faviconSourcePath = Path.Combine(templateDir, "assets", "favicon.svg");
        if (!File.Exists(faviconSourcePath))
        {
            throw new FileNotFoundException($"Arquivo oficial de favicon não encontrado em '{faviconSourcePath}'.");
        }

        var faviconContent = await File.ReadAllTextAsync(faviconSourcePath, cancellationToken);
        var faviconPath = await context.SaveAgentArtifactAsync("public/favicon.svg", faviconContent, Name);
        generatedFiles.Add(faviconPath);

        // 3. Valida, personaliza e salva src/index.html
        ConsoleLogger.Step("Personalizando e gerando estrutura semântica HTML5 (src/index.html)...");
        var htmlSourcePath = Path.Combine(templateDir, "src", "index.html");
        if (!File.Exists(htmlSourcePath))
        {
            throw new FileNotFoundException($"Arquivo obrigatório 'src/index.html' não encontrado no template '{templateIdentifier}' em: {htmlSourcePath}");
        }

        var rawHtml = await File.ReadAllTextAsync(htmlSourcePath, cancellationToken);
        var personalizedHtml = RenderTemplate(rawHtml, spec, "HTML");
        var htmlPath = await context.SaveAgentArtifactAsync("src/index.html", personalizedHtml, Name);
        generatedFiles.Add(htmlPath);

        // 4. Valida, personaliza e salva src/styles/main.css
        ConsoleLogger.Step("Personalizando e gerando estilos CSS (src/styles/main.css)...");
        var cssSourcePath = Path.Combine(templateDir, "src", "styles", "main.css");
        if (!File.Exists(cssSourcePath))
        {
            throw new FileNotFoundException($"Arquivo obrigatório 'src/styles/main.css' não encontrado no template '{templateIdentifier}' em: {cssSourcePath}");
        }

        var rawCss = await File.ReadAllTextAsync(cssSourcePath, cancellationToken);
        var personalizedCss = RenderTemplate(rawCss, spec, "CSS");
        var cssPath = await context.SaveAgentArtifactAsync("src/styles/main.css", personalizedCss, Name);
        generatedFiles.Add(cssPath);

        // 5. Valida, personaliza e salva src/scripts/app.js
        ConsoleLogger.Step("Personalizando e gerando módulos interativos em JS (src/scripts/app.js)...");
        var jsSourcePath = Path.Combine(templateDir, "src", "scripts", "app.js");
        if (!File.Exists(jsSourcePath))
        {
            throw new FileNotFoundException($"Arquivo obrigatório 'src/scripts/app.js' não encontrado no template '{templateIdentifier}' em: {jsSourcePath}");
        }

        var rawJs = await File.ReadAllTextAsync(jsSourcePath, cancellationToken);
        var personalizedJs = RenderTemplate(rawJs, spec, "JS");
        var jsPath = await context.SaveAgentArtifactAsync("src/scripts/app.js", personalizedJs, Name);
        generatedFiles.Add(jsPath);

        // 6. Gera documentação técnica docs/development.md
        ConsoleLogger.Step("Gerando documentação técnica de desenvolvimento (docs/development.md)...");
        var devDocContent = GenerateDevelopmentDoc(spec, templateIdentifier, templateDir);
        var devDocPath = await context.SaveAgentArtifactAsync("docs/development.md", devDocContent, Name);
        generatedFiles.Add(devDocPath);

        stopwatch.Stop();
        context.SharedMemory["DeveloperCompleted"] = true;

        return AgentExecutionResult.CreateSuccess(
            $"Desenvolvimento frontend concluído: site personalizado com base no JSON e template '{templateIdentifier}'.",
            generatedFiles,
            stopwatch.Elapsed
        );
    }

    /// <summary>
    /// Localiza o diretório do template físico com validação estrita contra path traversal.
    /// </summary>
    public static string ResolveTemplateDirectory(string templateName)
    {
        if (string.IsNullOrWhiteSpace(templateName))
        {
            throw new ArgumentException("O identificador do template não pode ser vazio ou nulo.", nameof(templateName));
        }

        var trimmed = templateName.Trim();

        // Proteção contra path traversal
        if (trimmed.Contains("..") || trimmed.Contains('/') || trimmed.Contains('\\') ||
            !Regex.IsMatch(trimmed, @"^[a-zA-Z0-9_-]+$"))
        {
            throw new ArgumentException($"Identificador de template inválido ou tentativa de path traversal detectada: '{templateName}'.", nameof(templateName));
        }

        var normalized = trimmed.ToLowerInvariant() switch
        {
            "automotivo" => "automotive",
            "automotive" => "automotive",
            _ => trimmed.ToLowerInvariant()
        };

        // 1. Busca a partir do diretório de trabalho atual (subindo na árvore)
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (currentDir != null)
        {
            var candidate = Path.Combine(currentDir.FullName, "Templates", normalized);
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            currentDir = currentDir.Parent;
        }

        // 2. Busca a partir do AppDomain / BaseDirectory da aplicação
        var baseDir = new DirectoryInfo(AppContext.BaseDirectory);
        while (baseDir != null)
        {
            var candidate = Path.Combine(baseDir.FullName, "Templates", normalized);
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
            baseDir = baseDir.Parent;
        }

        throw new DirectoryNotFoundException(
            $"Template '{templateName}' (pasta '{normalized}') não foi localizado. " +
            $"Certifique-se de que a pasta 'Templates/{normalized}' existe no repositório.");
    }

    /// <summary>
    /// Substitui placeholders pelos valores configurados e valida ausência de marcadores órfãos.
    /// </summary>
    public static string RenderTemplate(string templateContent, ProjectSpecification spec, string contextType)
    {
        var custom = spec.Personalizacao ?? new ProjectCustomization();

        // Normalização de dados e valores padrão
        var cliente = spec.Cliente?.Trim() ?? string.Empty;
        var slogan = !string.IsNullOrWhiteSpace(custom.Slogan)
            ? custom.Slogan.Trim()
            : "Seminovos selecionados com procedência e transparência";

        var descricao = !string.IsNullOrWhiteSpace(custom.Descricao)
            ? custom.Descricao.Trim()
            : $"{cliente} - Concessionária de veículos seminovos selecionados e premium com garantia de procedência e financiamento facilitado.";

        var cidade = !string.IsNullOrWhiteSpace(custom.Cidade)
            ? custom.Cidade.Trim()
            : "Sua Região";

        var endereco = !string.IsNullOrWhiteSpace(custom.Endereco)
            ? custom.Endereco.Trim()
            : "Consulte nossos canais de atendimento digital";

        var telefone = !string.IsNullOrWhiteSpace(custom.Telefone)
            ? custom.Telefone.Trim()
            : "Atendimento Online";

        // Normalização e validação do WhatsApp
        var rawWhatsapp = custom.Whatsapp ?? string.Empty;
        var whatsappDigits = Regex.Replace(rawWhatsapp, @"\D", "");
        var hasValidWhatsapp = whatsappDigits.Length >= 10 && whatsappDigits.Length <= 14;

        string whatsappNumber = hasValidWhatsapp ? whatsappDigits : string.Empty;
        string whatsappUrl = hasValidWhatsapp
            ? $"https://wa.me/{whatsappDigits}?text={Uri.EscapeDataString($"Olá! Gostaria de mais informações sobre os veículos da {cliente}.")}"
            : "#contato";
        string whatsappTarget = hasValidWhatsapp ? "_blank" : "_self";
        string whatsappRel = hasValidWhatsapp ? "noopener noreferrer" : string.Empty;
        string whatsappLabel = hasValidWhatsapp ? "WhatsApp" : "Fale Conosco";

        // Validação de cores hexadecimais
        var hexRegex = new Regex(@"^#(?:[0-9a-fA-F]{3,4}|[0-9a-fA-F]{6}|[0-9a-fA-F]{8})$");
        var corPrimaria = (!string.IsNullOrWhiteSpace(custom.CorPrimaria) && hexRegex.IsMatch(custom.CorPrimaria.Trim()))
            ? custom.CorPrimaria.Trim()
            : "#E5A93C";

        var corFundo = (!string.IsNullOrWhiteSpace(custom.CorFundo) && hexRegex.IsMatch(custom.CorFundo.Trim()))
            ? custom.CorFundo.Trim()
            : "#0B0F19";

        var anoAtual = DateTime.UtcNow.Year.ToString();

        // Dicionário de substituições conforme o contexto
        var replacements = new Dictionary<string, string>();

        if (contextType == "HTML")
        {
            replacements["{{CLIENTE}}"] = WebUtility.HtmlEncode(cliente);
            replacements["{{SLOGAN}}"] = WebUtility.HtmlEncode(slogan);
            replacements["{{DESCRICAO}}"] = WebUtility.HtmlEncode(descricao);
            replacements["{{CIDADE}}"] = WebUtility.HtmlEncode(cidade);
            replacements["{{ENDERECO}}"] = WebUtility.HtmlEncode(endereco);
            replacements["{{TELEFONE}}"] = WebUtility.HtmlEncode(telefone);
            replacements["{{WHATSAPP_NUMBER}}"] = WebUtility.HtmlEncode(whatsappNumber);
            replacements["{{WHATSAPP_URL}}"] = WebUtility.HtmlEncode(whatsappUrl);
            replacements["{{WHATSAPP_TARGET}}"] = whatsappTarget;
            replacements["{{WHATSAPP_REL}}"] = whatsappRel;
            replacements["{{WHATSAPP_LABEL}}"] = WebUtility.HtmlEncode(whatsappLabel);
            replacements["{{ANO_ATUAL}}"] = anoAtual;
        }
        else if (contextType == "JS")
        {
            // Escape seguro para literais de string em JavaScript
            replacements["{{WHATSAPP_NUMBER}}"] = whatsappNumber.Replace("\\", "\\\\").Replace("\"", "\\\"");
            replacements["{{CLIENTE}}"] = cliente.Replace("\\", "\\\\").Replace("\"", "\\\"");
            replacements["{{ANO_ATUAL}}"] = anoAtual;
        }
        else if (contextType == "CSS")
        {
            replacements["{{COR_PRIMARIA}}"] = corPrimaria;
            replacements["{{COR_FUNDO}}"] = corFundo;
        }

        var result = templateContent;
        foreach (var (placeholder, value) in replacements)
        {
            result = result.Replace(placeholder, value);
        }

        // Verificação de segurança: identifica se restou algum marcador {{...}} não resolvido
        var orphanMatches = Regex.Matches(result, @"\{\{([a-zA-Z0-9_]+)\}\}");
        if (orphanMatches.Count > 0)
        {
            var orphans = string.Join(", ", orphanMatches.Select(m => m.Value).Distinct());
            throw new InvalidOperationException(
                $"Falha de personalização ({contextType}): os seguintes marcadores não foram resolvidos: {orphans}");
        }

        return result;
    }

    private static string GenerateDevelopmentDoc(ProjectSpecification spec, string templateName, string templatePath)
    {
        var custom = spec.Personalizacao;
        return $@"# Documentação Técnica de Desenvolvimento Frontend
**Projeto:** {spec.Cliente}  
**Segmento:** {spec.Tipo}  
**Template Base:** {templateName}  
**Origem do Template:** `{templatePath}`  
**Data da Implementação:** {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss} UTC  

---

## 1. Arquitetura da Solução
A solução foi gerada e personalizada automaticamente através do **SiteFactory V2**, utilizando o template físico e os dados de configuração definidos em JSON.

### Parâmetros de Personalização Aplicados:
- **Slogan:** {custom?.Slogan ?? "(Padrão)"}
- **Cidade:** {custom?.Cidade ?? "(Padrão)"}
- **Telefone:** {custom?.Telefone ?? "(Padrão)"}
- **WhatsApp:** {(string.IsNullOrWhiteSpace(custom?.Whatsapp) ? "(Não configurado)" : custom.Whatsapp)}
- **Cor Primária:** {custom?.CorPrimaria ?? "#E5A93C"}
- **Cor de Fundo:** {custom?.CorFundo ?? "#0B0F19"}

### Estrutura de Arquivos Gerada:
- `src/index.html`: Marcação semântica com metatags personalizadas, hierarquia acessível e IDs exclusivos.
- `src/styles/main.css`: Design System moderno em Vanilla CSS fundamentado em custom properties (`:root`), paleta customizável e responsividade.
- `src/scripts/app.js`: Lógica funcional em Vanilla JS (ES6+) para catálogo de veículos, filtros dinâmicos e simulador de parcelas.
- `public/favicon.svg`: Identidade visual vetorial da marca.

---

## 2. Componentes e Seletores Implementados
- `#navbar`: Cabeçalho sticky com navegação e botão de contato rápido.
- `#home`: Hero banner com apresentação da empresa e métricas de autoridade.
- `#filter-bar`: Abas de categoria e campo de busca interativa.
- `#vehicle-grid`: Grade de renderização dinâmica com cards de veículos.
- `#simulador`: Calculadora interativa de parcelas com cálculo demonstrativo e encaminhamento para WhatsApp.
- `#diferenciais`: Pilares de confiança (perícia cautelar, procedência, garantia).
- `#sobre`: Apresentação institucional da loja.
- `#contato`: Endereço físico, horários e acionamento direto do WhatsApp comercial.
- `#footer`: Rodapé corporativo com direitos autorais e avisos legais.
";
    }
}
