using System.Diagnostics;
using SiteFactory.Orchestrator;

namespace SiteFactory.Agents.Designer;

public class DesignerAgent : IAgent
{
    public string Name => "Designer";
    public string Role => "Design UI/UX e Design System";
    public string Description => "Define o design system, paleta de cores, tipografia, escala visual e arquitetura de componentes.";

    public async Task<AgentExecutionResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var spec = context.Specification;
        var generatedFiles = new List<string>();

        ConsoleLogger.Step("Consultando requisitos e sitemap do Planner...");
        var requirements = await context.ReadArtifactAsync("docs/requirements.md");
        await Task.Delay(300, cancellationToken);

        // 1. Gera docs/design.md
        ConsoleLogger.Step("Elaborando Design System e tokens visuais (docs/design.md)...");
        var designContent = $@"# Design System & Identidade Visual
**Projeto:** {spec.Cliente}  
**Tema:** Dark Luxury Automotive  
**Versão:** 1.0.0  

---

## 1. Paleta de Cores (Tokens CSS)
Utilizamos uma paleta escura sofisticada inspirada em cockpits esportivos modernos:

| Token | Valor Hex | Descrição de Uso |
| :--- | :--- | :--- |
| `--bg-base` | `#0B0F19` | Fundo principal da página (Deep Midnight) |
| `--bg-surface` | `#111827` | Fundo de cartões, seções e modais |
| `--bg-surface-elevated`| `#1E293B` | Superfície elevada em hover |
| `--primary` | `#F59E0B` | Âmbar Dourado para CTAs principais e badges |
| `--primary-hover` | `#D97706` | Estado de foco/hover de botões primários |
| `--accent-blue` | `#2563EB` | Azul elétrico para destaques tecnológicos |
| `--text-primary` | `#F8FAFC` | Texto de alto contraste e títulos principais |
| `--text-secondary` | `#94A3B8` | Subtítulos, especificações técnicas |
| `--border-subtle` | `rgba(255, 255, 255, 0.08)` | Bordas finas com efeito glassmorphism |
| `--success` | `#10B981` | Selo de laudo cautelar aprovado e garantia |

---

## 2. Tipografia
- **Família Tipográfica Primária:** `'Plus Jakarta Sans', system-ui, -apple-system, sans-serif`
- **Escala de Tamanhos:**
  - `Hero H1`: `clamp(2.25rem, 5vw, 3.5rem)` com `font-weight: 800`
  - `Section H2`: `clamp(1.75rem, 3.5vw, 2.5rem)` com `font-weight: 700`
  - `Card Title H3`: `1.25rem` com `font-weight: 600`
  - `Body / Parágrafos`: `1rem` com `line-height: 1.6`
  - `Badges & Metadados`: `0.85rem` com `font-weight: 500`

---

## 3. Filosofia de Microinterações
- **Glassmorphism:** Efeito de desfoque de fundo (`backdrop-filter: blur(12px)`) no cabeçalho fixo e nos cards flutuantes.
- **Hover Responsivo:** Elevação suave em cards de veículos (`transform: translateY(-4px)`) com transição de 0.25s cúbica.
- **Badges de Destaque:** Efeito sutil de gradiente nos botões e nos selos de procedência.
";
        var designFile = await context.SaveAgentArtifactAsync("docs/design.md", designContent, Name);
        generatedFiles.Add(designFile);

        // 2. Gera docs/components.md
        ConsoleLogger.Step("Mapeando anatomia de componentes UI (docs/components.md)...");
        var componentsContent = $@"# Especificação Anatômica de Componentes
**Cliente:** {spec.Cliente}  

---

## 1. Header / Navbar (`#navbar`)
- **Composição:** Logo da marca, menu de links suaves (`Estoque`, `Simulador`, `Diferenciais`, `Contato`), botão de CTA direto para o WhatsApp.
- **Comportamento:** Fixo no topo (`position: sticky`), background translúcido escuro com blur ao rolar.

---

## 2. Hero Showcase (`#hero`)
- **Composição:** Tag badge de novidade, Título H1 imponente, parágrafo de impacto, 2 botões de CTA (Ver Estoque / Simular Financiamento), e métricas de autoridade (Ex: +10 anos de tradição, +1.200 carros entregues, 100% periciados).

---

## 3. Filter Bar (`#filter-bar`)
- **Composição:** Abas/botões de filtro rápido com contagem dinâmica (Todos, SUV, Sedan, Hatch, Pick-up) e campo de busca rápida por modelo.

---

## 4. Vehicle Card Grid (`#vehicle-grid`)
- **Composição por Card:**
  - Imagem do veículo com tag de badge (Ex: 'Único Dono', 'IPVA 2026 Pago').
  - Nome do modelo e versão.
  - Grade de metadados: Ano, Quilometragem, Câmbio.
  - Preço à vista em destaque e valor simulado de entrada + parcela.
  - Botão de ação direta: 'Tenho Interesse' (abre WhatsApp com modelo pré-preenchido).

---

## 5. Simulador Interativo (`#simulador`)
- **Composição:** Seleção do veículo ou valor total, slider/input de entrada, seletor de parcelas (12x a 60x), cálculo em tempo real do valor aproximado da parcela e botão de envio de proposta para análise cadastral.

---

## 6. Footer Corporativo (`#footer`)
- **Composição:** Informações legais da empresa, CNPJ simulado, endereço físico, horário de atendimento, links rápidos e créditos do SiteFactory.
";
        var componentsFile = await context.SaveAgentArtifactAsync("docs/components.md", componentsContent, Name);
        generatedFiles.Add(componentsFile);

        stopwatch.Stop();
        context.SharedMemory["DesignerCompleted"] = true;

        return AgentExecutionResult.CreateSuccess(
            "Design system e anatomia de componentes gerados com sucesso.",
            generatedFiles,
            stopwatch.Elapsed
        );
    }
}
