# 🏭 SiteFactory - Orquestrador de Sites Multi-Agentes (V1)

**SiteFactory** é uma fábrica/orquestrador de sites construída em **C# (.NET 8)** que coordena uma esteira de agentes especializados para automatizar de **80% a 90%** do trabalho repetitivo e previsível de criação web, mantendo o desenvolvedor no controle (**Human-in-the-Loop**) através do **VS Code / IDE**.

---

## 🎯 Filosofia do Projeto
- **Automação Inteligente:** A IA cuida do trabalho pesado (análise de mercado, arquitetura de informação, design system, código boilerplate semântico, simuladores, testes de QA e relatórios).
- **Supervisão Humana:** O desenvolvedor supervisiona as alterações no VS Code, faz ajustes criativos finos (fotos reais, telefones oficiais, cores de marca) e toma a decisão final de publicação.
- **V1 com Agentes Simulados:** Esta versão prova a robustez e o ciclo de vida completo do **Orquestrador**, preparando a arquitetura para plugar futuramente APIs como **Gemini** (análise/pesquisa), **Claude** (design/revisão) e **Codex CLI** (desenvolvimento).
- **Controle com Git:** Checkpoint automático a cada agente concluído com commits organizados no projeto do cliente.

---

## 🔄 Fluxo de Execução do Pipeline

```
ENTRADA DO CLIENTE (JSON)
        ↓
[1] PLANNER / ANALISTA        → Gera docs/analysis.md, docs/requirements.md, docs/sitemap.md
        ↓
[2] DESIGNER UI/UX           → Gera docs/design.md, docs/components.md
        ↓
[3] DEVELOPER FRONTEND       → Codifica src/index.html, src/styles/main.css, src/scripts/app.js, public/favicon.svg
        ↓
[4] TESTER / QA              → Valida integridade e gera tests/site-checks.json, docs/qa-report.md
        ↓
[5] REVIEWER / AUDITOR       → Avalia objetivo, nota executiva e gera docs/review.md + project.json
        ↓
PROJETO FINAL PRONTO PARA SUPERVISÃO NO VS CODE
```

---

## 📁 Estrutura de Diretórios

```
SiteFactory/
├── Agents/                      # Contratos e implementação dos agentes especializados
│   ├── IAgent.cs                # Interface base para agentes simulados ou conectados a LLMs
│   ├── AgentExecutionResult.cs  # DTO de resultado de execução
│   ├── Planner/                 # Agente de Análise e Requisitos
│   ├── Designer/                # Agente de UI/UX e Design System
│   ├── Developer/               # Agente de Código Frontend (HTML, CSS, JS)
│   ├── Tester/                  # Agente de Testes e Validações Técnicas
│   └── Reviewer/                # Agente de Auditoria e Parecer Executivo
├── Orchestrator/                # Núcleo da fábrica de software
│   ├── SiteOrchestrator.cs      # Coordenador sequencial do pipeline
│   ├── PipelineContext.cs       # Contexto e memória compartilhada entre agentes
│   ├── ProjectSpecification.cs  # Modelo de entrada do cliente
│   ├── ProjectState.cs          # Máquina de estado e histórico de passos
│   ├── ProjectPaths.cs          # Gerenciamento de diretórios de projetos
│   ├── GitCheckpointService.cs  # Checkpoints automáticos via Git
│   └── ConsoleLogger.cs         # Interface visual de terminal
├── Projects/                    # Onde os projetos de cada cliente são criados
│   └── jordan-veiculos/         # Exemplo gerado automaticamente
│       ├── .agents/             # Logs individuais de cada agente
│       ├── .git/                # Repositório local com commits de checkpoint
│       ├── docs/                # Artefatos estratégicos (analysis, design, qa, review...)
│       ├── public/              # Ativos estáticos e favicon
│       ├── src/                 # Código fonte da aplicação (index.html, CSS, JS)
│       ├── tests/               # Resultados estruturados de testes
│       └── project.json         # Estado, especificações e métricas consolidadas
├── Prompts/                     # Templates de engenharia de prompt para futuras LLMs
├── Standards/                   # Diretrizes técnicas (código, design, QA)
├── Templates/                   # Templates base por segmento
├── sample-project.json          # Exemplo de entrada do cliente
├── SiteFactory.csproj           # Configuração do projeto .NET 8
└── README.md                    # Documentação do projeto
```

---

## 🚀 Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git (opcional, para checkpoints automáticos)

### 1. Executar com a especificação padrão (`sample-project.json`):
```bash
dotnet run
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
  "observacoes": "Design premium, responsivo, integração com WhatsApp para cotação e simulador de financiamento interativo."
}
```

---

## 🔎 Supervisão no VS Code

Após a execução, abra a pasta `Projects/jordan-veiculos/` no seu VS Code:

1. **Documentação em `docs/`:**
   - [`analysis.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/analysis.md): Diagnóstico e persona.
   - [`requirements.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/requirements.md): Requisitos funcionais e não-funcionais.
   - [`sitemap.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/sitemap.md): Arquitetura das seções.
   - [`design.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/design.md): Design tokens e paleta Dark Luxury.
   - [`components.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/components.md): Anatomia de cada componente.
   - [`qa-report.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/qa-report.md): Checklist e status dos testes (10/10 aprovados).
   - [`review.md`](file:///c:/Users/joaop/OneDrive/Desktop/SiteFactory/Projects/jordan-veiculos/docs/review.md): Parecer executivo, nota e itens recomendados para supervisão humana.

2. **Visualizar no Navegador:**
   - Abra diretamente no navegador o arquivo:
     `Projects/jordan-veiculos/src/index.html`
   - O site possui catálogo interativo com filtros por categoria, busca em tempo real, simulador dinâmico de parcelas e integração com WhatsApp.

3. **Histórico Git:**
   - Veja os commits de cada agente:
     ```bash
     git -C Projects/jordan-veiculos log --oneline
     ```

---

## 🔮 Próximos Passos (Evolução Futura)
- Conexão da interface `IAgent` com **Gemini API** para agentes de pesquisa/análise.
- Conexão com **Claude API** para refino criativo de design e revisão crítica.
- Integração de comandos CLI (Codex) para geração incremental de componentes.
- Suporte a rollback para checkpoints anteriores via comando do orquestrador.
