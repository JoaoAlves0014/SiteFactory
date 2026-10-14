// SiteFactory Portfolio Interativo - João Pedro

const agentsData = {
  planner: {
    badge: 'Etapa 1 de 5 • Planejamento',
    name: 'Planner / Analista Estratégico',
    icon: '🧭',
    desc: 'Disseca as informações de entrada do cliente, diagnostica o mercado concorrente, define personas, requisitos funcionais/técnicos e desenha o sitemap completo.',
    artifacts: [
      { file: 'analysis.md', text: 'Diagnóstico de mercado, público-alvo e oportunidades de conversão.' },
      { file: 'requirements.md', text: 'Requisitos funcionais (catálogo, filtros, simulador) e não-funcionais (SEO, mobile).' },
      { file: 'sitemap.md', text: 'Estrutura de navegação e fluxos em formato Mermaid.' }
    ],
    contract: 'IAgent.ExecuteAsync() -> Planejamento Estratégico',
    commit: 'git commit -m "[SiteFactory] Planner: Planejamento concluído"'
  },
  designer: {
    badge: 'Etapa 2 de 5 • Design UI/UX',
    name: 'Designer UI/UX & Design System',
    icon: '🎨',
    desc: 'Traduz os requisitos em um Design System coeso com paleta refinada (Dark Luxury), tipografia de alta legibilidade, escalas de espaçamento e anatomia de componentes.',
    artifacts: [
      { file: 'design.md', text: 'Design Tokens (:root), cores, tipografia, microinterações e contraste WCAG AA.' },
      { file: 'components.md', text: 'Anatomia anatômica de componentes (Header, Hero, Grid de Produtos, Simulador, Footer).' }
    ],
    contract: 'IAgent.ExecuteAsync() -> Tokens Visuais & Componentes',
    commit: 'git commit -m "[SiteFactory] Designer: Design system gerado"'
  },
  developer: {
    badge: 'Etapa 3 de 5 • Frontend',
    name: 'Developer Frontend & Engenharia Web',
    icon: '💻',
    desc: 'Implementa código limpo, semântico e leve em HTML5, CSS moderno e JavaScript modular sem frameworks pesados, garantindo carregamento instantâneo.',
    artifacts: [
      { file: 'src/index.html', text: 'Página semântica completa com metatags SEO e IDs únicos.' },
      { file: 'src/styles/main.css', text: 'CSS Custom Properties, layouts em Grid/Flexbox e glassmorphism.' },
      { file: 'src/scripts/app.js', text: 'Filtros dinâmicos, busca instantânea e simulador de parcelas.' },
      { file: 'docs/development.md', text: 'Documentação técnica de arquitetura e manutenção.' }
    ],
    contract: 'IAgent.ExecuteAsync() -> Código-Fonte Frontend',
    commit: 'git commit -m "[SiteFactory] Developer: Código frontend gerado"'
  },
  tester: {
    badge: 'Etapa 4 de 5 • QA & Testes',
    name: 'Tester / Qualidade e Validações Técnicas',
    icon: '🧪',
    desc: 'Bateria de validação automatizada verificando integridade de arquivos, semântica SEO (único H1), consistência de tokens no CSS e lógica interativa no JavaScript.',
    artifacts: [
      { file: 'tests/site-checks.json', text: 'Resultado estruturado com taxa de 100% de aprovação (11/11 testes).' },
      { file: 'docs/qa-report.md', text: 'Relatório detalhado com checklist e evidências de conformidade.' }
    ],
    contract: 'IAgent.ExecuteAsync() -> Auditoria Técnica & QA',
    commit: 'git commit -m "[SiteFactory] Tester: 11/11 testes aprovados"'
  },
  reviewer: {
    badge: 'Etapa 5 de 5 • Revisão Final',
    name: 'Reviewer / Auditoria Executiva',
    icon: '📋',
    desc: 'Consolida as entregas dos agentes anteriores, valida conformidade com a meta do cliente, atribui nota de qualidade e orienta os 10-20% de supervisão humana no VS Code.',
    artifacts: [
      { file: 'docs/review.md', text: 'Parecer executivo com nota 9.8/10 e checklist de ajustes na IDE.' },
      { file: 'project.json', text: 'Estado consolidado COMPLETED com histórico de tempos e artefatos.' }
    ],
    contract: 'IAgent.ExecuteAsync() -> Parecer Executivo',
    commit: 'git commit -m "[SiteFactory] Reviewer: Projeto aprovado para supervisão"'
  }
};

function initAgentPipeline() {
  const buttons = document.querySelectorAll('.agent-btn');
  const badgeEl = document.getElementById('agent-badge');
  const nameEl = document.getElementById('agent-name');
  const iconEl = document.getElementById('agent-icon');
  const descEl = document.getElementById('agent-desc');
  const artifactsEl = document.getElementById('agent-artifacts');
  const footerEl = document.querySelector('.agent-meta-footer');

  buttons.forEach(btn => {
    btn.addEventListener('click', () => {
      buttons.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');

      const agentKey = btn.getAttribute('data-agent');
      const data = agentsData[agentKey];
      if (!data) return;

      badgeEl.textContent = data.badge;
      nameEl.textContent = data.name;
      iconEl.textContent = data.icon;
      descEl.textContent = data.desc;

      artifactsEl.innerHTML = data.artifacts.map(a => 
        `<li><code>${a.file}</code> - ${a.text}</li>`
      ).join('');

      if (footerEl) {
        footerEl.innerHTML = `
          <span>Contrato: <code>${data.contract}</code></span>
          <span>Checkpoint: <code>${data.commit}</code></span>
        `;
      }
    });
  });
}

function initCopyEmail() {
  const btn = document.getElementById('btn-copy-email');
  const text = document.getElementById('copy-text');
  const myEmail = 'joaopedro.dev@exemplo.com'; // Pode ser alterado pelo usuário

  if (!btn || !text) return;

  btn.addEventListener('click', () => {
    navigator.clipboard.writeText(myEmail).then(() => {
      const original = text.innerHTML;
      text.innerHTML = '✔ E-mail Copiado!';
      setTimeout(() => {
        text.innerHTML = original;
      }, 2500);
    }).catch(() => {
      prompt('Copie o e-mail abaixo:', myEmail);
    });
  });
}

document.addEventListener('DOMContentLoaded', () => {
  initAgentPipeline();
  initCopyEmail();
});
