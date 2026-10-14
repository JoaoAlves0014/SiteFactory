# 🏭 SiteFactory - Orquestrador de Sites Multi-Agentes (V2)

**SiteFactory** é uma fábrica/orquestrador de sites construída em **C# (.NET 8)** que coordena uma esteira de agentes especializados para automatizar de **80% a 90%** do trabalho repetitivo e previsível de criação web, mantendo o desenvolvedor no controle (**Human-in-the-Loop**) através do **VS Code / IDE**.

Na **V2**, o SiteFactory evoluiu para utilizar **templates físicos modulares e reutilizáveis** (HTML5 semântico, CSS3 e JavaScript puro), desacoplados do código compilado e parametrizáveis via arquivo JSON de especificação.

---

## 🎯 Filosofia do Projeto
- **Automação Inteligente:** A esteira de agentes cuida da análise de requisitos, design system, personalização de templates, geração de código, testes automatizados de QA e relatórios de auditoria.
- **Templates Físicos Reutilizáveis (V2):** O código frontend reside em arquivos físicos desacoplados dentro de `Templates/`, facilitando manutenção visual e expansão para novos segmentos de mercado.
- **Personalização via JSON:** Conteúdo, dados de contato, cores do Design System e metatags são injetados automaticamente a partir da especificação do cliente.
- **Supervisão Humana:** O desenvolvedor supervisiona o código no VS Code, substitui os dados demonstrativos por fotos e telefones reais e toma a decisão final de publicação.
- **Controle com Git:** Checkpoint automático a cada agente concluído com repositório local e commits organizados no projeto do cliente.

---

## 🔄 Fluxo de Execução do Pipeline

```
ENTRADA DO CLIENTE (JSON)
        ↓
[1] PLANNER / ANALISTA        → Gera docs/analysis.md, docs/requirements.md, docs/sitemap.md
        ↓
[2] DESIGNER UI/UX           → Gera docs/design.md, docs/components.md
        ↓
[3] DEVELOPER FRONTEND       → Personaliza e gera src/index.html, src/styles/main.css, src/scripts/app.js, public/favicon.svg
        ↓
[4] TESTER / QA              → Valida integridade e gera tests/site-checks.json, docs/qa-report.md (11/11 testes)
        ↓
[5] REVIEWER / AUDITOR       → Avalia conformidade, nota executiva e gera docs/review.md + project.json
        ↓
PROJETO FINAL PRONTO PARA SUPERVISÃO NO VS CODE
```

---

## 📁 Estrutura de Diretórios

```
SiteFactory/
├── Agents/                      # Contratos e implementação dos agentes especializados
│   ├── IAgent.cs                # Interface base para agentes
│   ├── AgentExecutionResult.cs  # DTO de resultado de execução
│   ├── Planner/                 # Agente de Análise e Requisitos
│   ├── Designer/                # Agente de UI/UX e Design System
│   ├── Developer/               # Agente de Integração e Personalização de Templates
│   ├── Tester/                  # Agente de Testes e Validações Técnicas (QA)
│   └── Reviewer/                # Agente de Auditoria e Parecer Executivo
├── Orchestrator/                # Núcleo do orquestrador
│   ├── SiteOrchestrator.cs      # Coordenador sequencial do pipeline
│   ├── PipelineContext.cs       # Contexto e memória compartilhada entre agentes
│   ├── ProjectSpecification.cs  # Modelo de especificação e personalização do cliente
│   ├── ProjectState.cs          # Máquina de estado e histórico de passos
│   ├── ProjectPaths.cs          # Gerenciamento de diretórios de projetos
│   ├── GitCheckpointService.cs  # Checkpoints automáticos via Git
│   └── ConsoleLogger.cs         # Interface visual de terminal
├── Templates/                   # Catálogo de templates físicos reutilizáveis
│   └── automotive/              # Template automotivo moderno (V2)
│       ├── base-template.json   # Metadados e design tokens padrão
│       ├── assets/              # Favicon e ativos vetoriais
│       │   └── favicon.svg
│       └── src/                 # Código fonte do template
│           ├── index.html       # Estrutura semântica com placeholders {{...}}
│           ├── styles/main.css  # Design System em Vanilla CSS (:focus-visible, prefers-reduced-motion)
│           └── scripts/app.js   # Catálogo interativo, filtros e simulador financeiro
├── Projects/                    # Onde os projetos gerados são armazenados
│   └── jordan-veiculos/         # Exemplo oficial gerado e personalizado
├── sample-project.json          # Exemplo de entrada do cliente com personalização
├── SiteFactory.csproj           # Configuração do projeto .NET 8
└── README.md                    # Documentação do projeto
```

---

## 🚀 Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git (opcional, para checkpoints automáticos locais)

### 1. Executar com a especificação padrão (`sample-project.json`):
```bash
dotnet run -- sample-project.json
```

### 2. Executar com especificação personalizada:
```bash
dotnet run --spec meu-cliente.json
```

### 3. Executar sem checkpoints do Git:
```bash
dotnet run --spec meu-cliente.json --no-git
```

---

## 📝 Exemplo de Entrada do Cliente (`sample-project.json`)

```json
{
  "cliente": "Jordan Veículos",
  "urlAtual": "https://www.jordanveiculos.com",
  "tipo": "automotivo",
  "template": "automotivo",
  "nivel": "profissional",
  "objetivo": "Modernizar o site atual",
  "observacoes": "Design premium, responsivo, integração com WhatsApp e simulador.",
  "personalizacao": {
    "slogan": "Seminovos selecionados com procedência e transparência",
    "descricao": "Concessionária multimarcas especializada em veículos seminovos periciados, garantia de procedência e atendimento consultivo.",
    "cidade": "Curitiba - PR",
    "endereco": "Atendimento presencial sob agendamento e consultoria digital",
    "telefone": "(41) 3333-0000",
    "whatsapp": "5541999999999",
    "corPrimaria": "#E5A93C",
    "corFundo": "#0B0F19"
  }
}
```

*Todos os campos dentro de `personalizacao` são opcionais e possuem fallbacks seguros baseados no segmento.*

---

## 🔎 Supervisão no VS Code e Visualização Local

Após a execução do pipeline, acesse a pasta `Projects/jordan-veiculos/`:

1. **Visualizar no Navegador:**
   - Abra diretamente no navegador o arquivo:
     `Projects/jordan-veiculos/src/index.html`
   - O site opera de forma autônoma sem necessidade de servidor backend (CSS puro, JS vanilla, ícones vetoriais embutidos).

2. **Funcionalidades do Template Automotivo (V2):**
   - Catálogo interativo com filtros por categoria (*SUVs, Sedans, Hatches, Pick-ups*) e busca em tempo real.
   - Simulador demonstrativo de financiamento com cálculo de parcelas (Tabela Price) e envio dinâmico para WhatsApp.
   - Design responsivo para Desktop (1440px), Tablet (768px) e Mobile (390px).
   - Acessibilidade aprimorada com `:focus-visible` e `@media (prefers-reduced-motion: reduce)`.

3. **Documentação Técnica em `docs/`:**
   - [`analysis.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/analysis.md): Diagnóstico estratégico.
   - [`requirements.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/requirements.md): Requisitos funcionais e técnicos.
   - [`sitemap.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/sitemap.md): Arquitetura das seções.
   - [`design.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/design.md): Tokens e paleta Dark Luxury.
   - [`development.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/development.md): Documentação técnica e origem do template.
   - [`qa-report.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/qa-report.md): Relatório de QA (11/11 testes aprovados).
   - [`review.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/review.md): Parecer executivo da auditoria final.

---

## ⚠️ Limitações Conhecidas (V2)

- **Dados Comerciais Demonstrativos:** Os telefones, endereços, fotos vetoriais e veículos do catálogo de exemplo são fictícios e ilustrativos. Devem ser substituídos pelos dados comerciais reais do cliente antes da publicação definitiva.
- **Inspeção Visual:** A validação técnica, semântica e de QA é 100% automatizada pelo `TesterAgent`. A inspeção visual final de layout deve ser revisada pelo desenvolvedor no navegador.

---

## 🔮 Próximos Passos (Evolução Futura / V3)
- Conexão de agentes com APIs de LLMs (**Gemini**, **Claude**, **Codex**).
- Expansão do catálogo de templates para outros segmentos (médico, advocacia, imobiliário, restaurantes).
- Integração de CMS leve / headless para edição de estoque sem alterar código.
