using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SiteFactory.Orchestrator;

/// <summary>
/// Campos opcionais de personalização visual e de conteúdo do template.
/// </summary>
public class ProjectCustomization
{
    [JsonPropertyName("slogan")]
    public string? Slogan { get; set; }

    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("cidade")]
    public string? Cidade { get; set; }

    [JsonPropertyName("endereco")]
    public string? Endereco { get; set; }

    [JsonPropertyName("telefone")]
    public string? Telefone { get; set; }

    [JsonPropertyName("whatsapp")]
    public string? Whatsapp { get; set; }

    [JsonPropertyName("corPrimaria")]
    public string? CorPrimaria { get; set; }

    [JsonPropertyName("corFundo")]
    public string? CorFundo { get; set; }
}

/// <summary>
/// Especificação de entrada fornecida pelo cliente ou carregada via JSON.
/// </summary>
public class ProjectSpecification
{
    [JsonPropertyName("cliente")]
    public string Cliente { get; set; } = string.Empty;

    [JsonPropertyName("urlAtual")]
    public string UrlAtual { get; set; } = string.Empty;

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [JsonPropertyName("template")]
    public string Template { get; set; } = string.Empty;

    [JsonPropertyName("nivel")]
    public string Nivel { get; set; } = "profissional";

    [JsonPropertyName("objetivo")]
    public string Objetivo { get; set; } = string.Empty;

    [JsonPropertyName("observacoes")]
    public string Observacoes { get; set; } = string.Empty;

    [JsonPropertyName("personalizacao")]
    public ProjectCustomization? Personalizacao { get; set; }

    /// <summary>
    /// Gera um identificador amigável (slug) para pastas e nomes de arquivo.
    /// Exemplo: "Jordan Veículos" -> "jordan-veiculos"
    /// </summary>
    [JsonIgnore]
    public string Slug
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Cliente))
                return "projeto-sem-nome";

            var normalized = Cliente.ToLowerInvariant().Trim();
            // Substituições simples de acentos
            normalized = normalized
                .Replace("á", "a").Replace("à", "a").Replace("ã", "a").Replace("â", "a")
                .Replace("é", "e").Replace("ê", "e")
                .Replace("í", "i")
                .Replace("ó", "o").Replace("õ", "o").Replace("ô", "o")
                .Replace("ú", "u").Replace("ü", "u")
                .Replace("ç", "c");

            // Remove caracteres especiais
            normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");
            // Substitui espaços por hífen
            normalized = Regex.Replace(normalized, @"\s+", "-").Trim('-');

            return string.IsNullOrEmpty(normalized) ? "projeto" : normalized;
        }
    }

    /// <summary>
    /// Valida os dados da especificação.
    /// </summary>
    public (bool IsValid, List<string> Errors) Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Cliente))
            errors.Add("O campo 'cliente' é obrigatório e não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(Tipo))
            errors.Add("O campo 'tipo' é obrigatório (ex: 'automotivo', 'institucional').");

        if (string.IsNullOrWhiteSpace(Objetivo))
            errors.Add("O campo 'objetivo' é obrigatório e deve descrever a meta do projeto.");

        // Validação de segurança contra path traversal na seleção de template
        if (!string.IsNullOrWhiteSpace(Template))
        {
            if (Template.Contains("..") || Template.Contains('/') || Template.Contains('\\'))
            {
                errors.Add("O campo 'template' contém caracteres de caminho ou path traversal inválidos.");
            }
        }

        // Validação opcional de formato de cores hexadecimais
        if (Personalizacao != null)
        {
            var hexRegex = new Regex(@"^#(?:[0-9a-fA-F]{3,4}|[0-9a-fA-F]{6}|[0-9a-fA-F]{8})$");
            if (!string.IsNullOrWhiteSpace(Personalizacao.CorPrimaria) && !hexRegex.IsMatch(Personalizacao.CorPrimaria.Trim()))
            {
                errors.Add($"A cor primária '{Personalizacao.CorPrimaria}' informada na personalização não é uma cor hexadecimal válida (ex: #E5A93C).");
            }
            if (!string.IsNullOrWhiteSpace(Personalizacao.CorFundo) && !hexRegex.IsMatch(Personalizacao.CorFundo.Trim()))
            {
                errors.Add($"A cor de fundo '{Personalizacao.CorFundo}' informada na personalização não é uma cor hexadecimal válida (ex: #0B0F19).");
            }
        }

        return (errors.Count == 0, errors);
    }
}
