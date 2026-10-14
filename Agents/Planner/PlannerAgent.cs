using System.Diagnostics;
using SiteFactory.Orchestrator;

namespace SiteFactory.Agents.Planner;

public class PlannerAgent : IAgent
{
    public string Name => "Planner";
    public string Role => "Planejamento e Análise Estratégica";
    public string Description => "Analisa especificações do cliente, concorrência e gera diagnóstico, requisitos e arquitetura de informação.";

    public async Task<AgentExecutionResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var spec = context.Specification;
        var generatedFiles = new List<string>();

        ConsoleLogger.Step($"Analisando dados do cliente: {spec.Cliente} ({spec.Tipo} / {spec.Nivel})...");
        await Task.Delay(300, cancellationToken); // Simulação de processamento/análise

        // 1. Gera docs/analysis.md
        ConsoleLogger.Step("Gerando diagnóstico e análise competitiva (docs/analysis.md)...");
        var analysisContent = $@"# Análise Estratégica e Diagnóstico Digital
**Cliente:** {spec.Cliente}  
**Segmento:** {spec.Tipo}  
**URL de Referência / Atual:** {spec.UrlAtual}  
**Nível de Projeto:** {spec.Nivel}  
**Data da Análise:** {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss} UTC  

---

## 1. Contexto e Diagnóstico Atual
O cliente **{spec.Cliente}** atua no setor automotivo e busca:
> ""{spec.Objetivo}""

### Principais Oportunidades Identificadas:
1. **Modernização Visual:** Substituir o visual datado por uma identidade visual escura e sofisticada (Slate/Dark Luxury), muito adotada pelas concessionárias premium contemporâneas.
2. **Foco em Conversão (Leads):** Reduzir atritos na jornada de compra. A inclusão de um simulador de financiamento com pré-aprovação rápida e botões diretos para WhatsApp aumenta a taxa de conversão em até 40%.
3. **Experiência Mobile:** Mais de 70% das buscas de veículos seminovos e novos acontecem em smartphones; o site necessita de layout 100% responsivo com toque intuitivo.
4. **Filtros Rápidos e Navegação:** Permitir que o visitante filtre estoque por categoria (SUVs, Sedans, Pick-ups, Hatchbacks) sem recarregar a página.

---

## 2. Público-Alvo e Persona
- **Perfil Principal:** Compradores de veículos que valorizam procedência, agilidade no atendimento e facilidade na simulação de parcelas.
- **Dores Principais:** Falta de transparência nas fotos, lentidão de resposta em canais tradicionais e cálculos complexos de financiamento.
- **Proposta de Valor:** Confiança, estoque verificado e atendimento imediato via WhatsApp.

---

## 3. Diretrizes de Posicionamento da Marca
- **Tom de Voz:** Confiável, moderno, dinâmico e focado em soluções.
- **Aparência Recomendada:** Estilo Dark Mode elegante com acentos em âmbar dourado/azul cobalto, transmitindo solidez e sofisticação mecânica.
";
        var analysisFile = await context.SaveAgentArtifactAsync("docs/analysis.md", analysisContent, Name);
        generatedFiles.Add(analysisFile);

        // 2. Gera docs/requirements.md
        ConsoleLogger.Step("Definindo especificação de requisitos funcionais e técnicos (docs/requirements.md)...");
        var requirementsContent = $@"# Especificação de Requisitos do Projeto
**Projeto:** {spec.Cliente}  
**Template Selecionado:** {spec.Template}  
**Objetivo Primário:** {spec.Objetivo}  

---

## 1. Requisitos Funcionais (RF)
- **RF-01 [Header/Navegação]:** Barra de navegação fixa com logo, links de rolagem suave para seções e botão de contato rápido.
- **RF-02 [Hero Showcase]:** Banner de destaque de alta conversão com chamada de impacto, estatísticas rápidas e botão principal de ação.
- **RF-03 [Filtro Rápido de Estoque]:** Sistema de busca e filtragem interativa por categorias (Todos, SUVs, Sedans, Pick-ups, Elétricos/Híbridos).
- **RF-04 [Grid de Veículos]:** Cards de veículos com ano, quilometragem, transmissão, valor à vista e parcela estimada com botão de cotação direta no WhatsApp.
- **RF-05 [Simulador de Financiamento]:** Calculadora interativa em tempo real com seleção de valor de entrada e número de parcelas (12x, 24x, 36x, 48x, 60x).
- **RF-06 [Diferenciais de Confiança]:** Cards destacando garantia de procedência, laudo cautelar aprovado e avaliação justa na troca.
- **RF-07 [Depoimentos de Clientes]:** Prova social com avaliações de compradores satisfeitos.
- **RF-08 [Localização e Contato]:** Endereço, horário de funcionamento e formulário/botão de direcionamento.

---

## 2. Requisitos Não-Funcionais (RNF)
- **RNF-01 [Responsividade]:** Suporte total a resoluções Desktop (1280px+), Tablet (768px-1024px) e Mobile (360px-480px).
- **RNF-02 [Performance]:** Código Vanilla (HTML5/CSS3/ES6+ puro) sem dependências pesadas externas para carregamento instantâneo.
- **RNF-03 [Acessibilidade]:** Contraste em conformidade com WCAG AA, marcações semânticas e labels de formulário claros.
- **RNF-04 [SEO]:** Heading tags estruturadas (um único H1), Open Graph tags para compartilhamento em redes sociais e metadados descritivos.
";
        var requirementsFile = await context.SaveAgentArtifactAsync("docs/requirements.md", requirementsContent, Name);
        generatedFiles.Add(requirementsFile);

        // 3. Gera docs/sitemap.md
        ConsoleLogger.Step("Desenhando arquitetura de informação e sitemap (docs/sitemap.md)...");
        var sitemapContent = $@"# Arquitetura de Informação e Sitemap
**Estrutura de Páginas e Seções:**

```mermaid
graph TD
    A[Página Principal: index.html] --> B[Header & Navbar com Contato]
    A --> C[Hero Showcase com Chamada de Ação]
    A --> D[Filtro Rápido de Categorias]
    A --> E[Grid de Estoque em Destaque]
    A --> F[Simulador Interativo de Financiamento]
    A --> G[Pilares de Confiança e Garantia]
    A --> H[Avaliações de Clientes]
    A --> I[Seção de Contato e Localização]
    A --> J[Rodapé Institucional]
```

### Detalhamento das Seções:
1. `#home` - Hero Principal com headline de modernização e CTAs principais.
2. `#estoque` - Vitrine dinâmica com filtro instantâneo por categoria.
3. `#simulador` - Calculadora interativa de parcelas.
4. `#diferenciais` - 4 selos de procedência e laudo cautelar.
5. `#depoimentos` - Prova social com feedbacks de clientes reais.
6. `#contato` - Mapa, endereço, horário e botão direto para WhatsApp.
";
        var sitemapFile = await context.SaveAgentArtifactAsync("docs/sitemap.md", sitemapContent, Name);
        generatedFiles.Add(sitemapFile);

        stopwatch.Stop();
        context.SharedMemory["PlannerCompleted"] = true;
        context.SharedMemory["AnalysisSummary"] = "Estrutura e requisitos de concessionária moderna definidos.";

        return AgentExecutionResult.CreateSuccess(
            "Planejamento concluído: diagnóstico, requisitos e sitemap gerados.",
            generatedFiles,
            stopwatch.Elapsed
        );
    }
}
