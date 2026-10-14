using System.Diagnostics;
using SiteFactory.Orchestrator;

namespace SiteFactory.Agents.Developer;

public class DeveloperAgent : IAgent
{
    public string Name => "Developer";
    public string Role => "Desenvolvimento Frontend e Arquitetura Web";
    public string Description => "Implementa código limpo em HTML5 semântico, CSS moderno e JavaScript modular baseado nas especificações do Designer.";

    public async Task<AgentExecutionResult> ExecuteAsync(PipelineContext context, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var spec = context.Specification;
        var generatedFiles = new List<string>();

        ConsoleLogger.Step("Consultando Design System e especificações de componentes...");
        await Task.Delay(350, cancellationToken);

        // 1. Gera public/favicon.svg
        ConsoleLogger.Step("Gerando favicon vetorial da marca (public/favicon.svg)...");
        var faviconSvg = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100"" fill=""none"">
  <rect width=""100"" height=""100"" rx=""20"" fill=""#0B0F19"" />
  <circle cx=""50"" cy=""50"" r=""36"" stroke=""#F59E0B"" stroke-width=""5"" stroke-dasharray=""6 4"" />
  <path d=""M28 58 L42 42 L58 42 L72 58 Z"" fill=""#F59E0B"" />
  <circle cx=""38"" cy=""62"" r=""6"" fill=""#F8FAFC"" />
  <circle cx=""62"" cy=""62"" r=""6"" fill=""#F8FAFC"" />
  <path d=""M36 44 L44 34 L56 34 L64 44 Z"" fill=""#2563EB"" opacity=""0.85"" />
</svg>";
        var faviconPath = await context.SaveAgentArtifactAsync("public/favicon.svg", faviconSvg, Name);
        generatedFiles.Add(faviconPath);

        // 2. Gera src/index.html
        ConsoleLogger.Step("Codificando estrutura semântica HTML5 (src/index.html)...");
        var htmlContent = GenerateHtml(spec);
        var htmlPath = await context.SaveAgentArtifactAsync("src/index.html", htmlContent, Name);
        generatedFiles.Add(htmlPath);

        // 3. Gera src/styles/main.css
        ConsoleLogger.Step("Codificando estilos e design tokens em CSS (src/styles/main.css)...");
        var cssContent = GenerateCss();
        var cssPath = await context.SaveAgentArtifactAsync("src/styles/main.css", cssContent, Name);
        generatedFiles.Add(cssPath);

        // 4. Gera src/scripts/app.js
        ConsoleLogger.Step("Codificando interatividades, simulador e filtros em JS (src/scripts/app.js)...");
        var jsContent = GenerateJs(spec);
        var jsPath = await context.SaveAgentArtifactAsync("src/scripts/app.js", jsContent, Name);
        generatedFiles.Add(jsPath);

        // 5. Gera docs/development.md
        ConsoleLogger.Step("Gerando documentação técnica de desenvolvimento (docs/development.md)...");
        var devDocContent = GenerateDevelopmentDoc(spec);
        var devDocPath = await context.SaveAgentArtifactAsync("docs/development.md", devDocContent, Name);
        generatedFiles.Add(devDocPath);

        stopwatch.Stop();
        context.SharedMemory["DeveloperCompleted"] = true;

        return AgentExecutionResult.CreateSuccess(
            "Desenvolvimento frontend concluído: index.html, main.css, app.js, development.md e favicon.svg gerados.",
            generatedFiles,
            stopwatch.Elapsed
        );
    }

    private static string GenerateDevelopmentDoc(ProjectSpecification spec)
    {
        return $@"# Documentação Técnica de Desenvolvimento Frontend
**Projeto:** {spec.Cliente}  
**Segmento:** {spec.Tipo}  
**Data da Implementação:** {DateTime.UtcNow:dd/MM/yyyy HH:mm:ss} UTC  

---

## 1. Arquitetura da Solução
A solução foi desenvolvida adotando a filosofia de **zero dependências externas pesadas**, garantindo performance máxima, facilidade de manutenção e tempo de carregamento inferior a 1 segundo.

### Estrutura de Arquivos Implementada:
- `src/index.html`: Marcação semântica com metatags Open Graph, SEO, acessibilidade e IDs exclusivos em todos os elementos interativos.
- `src/styles/main.css`: Sistema de design em CSS puro baseado em custom properties (`:root`), flexbox, CSS Grid e microinterações com glassmorphism.
- `src/scripts/app.js`: Lógica funcional em ES6+ para catálogo com filtro dinâmico de categorias (SUVs, Sedans, Pick-ups, Hatchbacks), busca em tempo real e simulador de financiamento com taxas de mercado e encaminhamento dinâmico para WhatsApp.
- `public/favicon.svg`: Identidade visual vetorial personalizada.

---

## 2. Componentes e Seletores Implementados
- `#navbar`: Cabeçalho sticky com efeito backdrop blur e botão de contato rápido.
- `#home`: Hero banner com chamada de impacto, estatísticas de autoridade e CTAs.
- `#filter-bar`: Abas de categoria e campo de busca interativa.
- `#vehicle-grid`: Grade de renderização dinâmica com cards de veículos.
- `#simulador`: Calculadora interativa de parcelas com seleção de veículo, entrada e prazo (24x a 60x).
- `#diferenciais`: Selos de garantia, laudo cautelar e procedência.
- `#depoimentos`: Prova social com avaliações verificadas de clientes.
- `#contato`: Endereço físico, horários e acionamento direto do WhatsApp comercial.

---

## 3. Instruções de Manutenção e Customização no VS Code
1. Para alterar veículos do catálogo, modifique a constante `vehiclesData` em `src/scripts/app.js`.
2. Para alterar o telefone de WhatsApp de destino, substitua o número `5511999999999` em `src/index.html` e `src/scripts/app.js`.
3. Para ajustar cores do tema, modifique as variáveis no bloco `:root` em `src/styles/main.css`.
";
    }

    private static string GenerateHtml(ProjectSpecification spec)
    {
        return $@"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <meta name=""description"" content=""{spec.Cliente} - Concessionária de veículos seminovos selecionados e premium com garantia de procedência e financiamento facilitado."">
  <meta name=""theme-color"" content=""#0B0F19"">
  <link rel=""icon"" type=""image/svg+xml"" href=""../public/favicon.svg"">
  
  <title>{spec.Cliente} | Veículos Selecionados & Seminovos Premium</title>
  
  <!-- Tipografia do Google Fonts -->
  <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
  <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
  <link href=""https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap"" rel=""stylesheet"">
  
  <link rel=""stylesheet"" href=""styles/main.css"">
</head>
<body>

  <!-- Header / Barra de Navegação -->
  <header class=""navbar-header"" id=""navbar"">
    <div class=""container nav-container"">
      <a href=""#"" class=""brand-logo"" id=""brand-logo"">
        <span class=""brand-icon"">⚡</span>
        <span class=""brand-name"">{spec.Cliente}</span>
      </a>

      <nav class=""nav-menu"" id=""nav-menu"">
        <a href=""#estoque"" class=""nav-link"">Estoque</a>
        <a href=""#simulador"" class=""nav-link"">Simulador</a>
        <a href=""#diferenciais"" class=""nav-link"">Diferenciais</a>
        <a href=""#depoimentos"" class=""nav-link"">Depoimentos</a>
        <a href=""#contato"" class=""nav-link"">Contato</a>
      </nav>

      <div class=""nav-actions"">
        <a href=""https://wa.me/5511999999999?text=Olá!+Gostaria+de+saber+mais+sobre+o+estoque+da+{Uri.EscapeDataString(spec.Cliente)}"" target=""_blank"" rel=""noopener noreferrer"" class=""btn btn-primary btn-sm"" id=""nav-cta-whatsapp"">
          <span>Falar no WhatsApp</span>
        </a>
      </div>
    </div>
  </header>

  <!-- Hero Section -->
  <section class=""hero-section"" id=""home"">
    <div class=""container hero-grid"">
      <div class=""hero-content"">
        <div class=""badge-pill"">
          <span class=""badge-dot""></span>
          <span>Novo Padrão em Veículos Premium</span>
        </div>

        <h1 class=""hero-title"">
          Encontre o seu próximo carro com <span class=""text-gradient"">procedência e elegância</span>.
        </h1>

        <p class=""hero-subtitle"">
          Veículos periciados com laudo cautelar 100% aprovado, garantia estendida e condições exclusivas de financiamento com taxas competitivas.
        </p>

        <div class=""hero-actions"">
          <a href=""#estoque"" class=""btn btn-primary btn-lg"" id=""hero-btn-estoque"">Explorar Estoque</a>
          <a href=""#simulador"" class=""btn btn-secondary btn-lg"" id=""hero-btn-simulador"">Simular Financiamento</a>
        </div>

        <div class=""hero-stats"">
          <div class=""stat-item"">
            <span class=""stat-number"">+1.200</span>
            <span class=""stat-label"">Carros Entregues</span>
          </div>
          <div class=""stat-divider""></div>
          <div class=""stat-item"">
            <span class=""stat-number"">100%</span>
            <span class=""stat-label"">Laudo Aprovado</span>
          </div>
          <div class=""stat-divider""></div>
          <div class=""stat-item"">
            <span class=""stat-number"">4.9 ★</span>
            <span class=""stat-label"">Avaliação Google</span>
          </div>
        </div>
      </div>

      <div class=""hero-card-visual"">
        <div class=""hero-visual-card"">
          <div class=""card-badge-top"">Destaque da Semana</div>
          <div class=""vehicle-illustration"">
            <svg viewBox=""0 0 400 220"" class=""car-svg"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
              <defs>
                <linearGradient id=""carGrad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""100%"">
                  <stop offset=""0%"" stop-color=""#2563EB"" />
                  <stop offset=""100%"" stop-color=""#1D4ED8"" />
                </linearGradient>
                <linearGradient id=""glowGrad"" x1=""0%"" y1=""0%"" x2=""100%"" y2=""0%"">
                  <stop offset=""0%"" stop-color=""#F59E0B"" stop-opacity=""0.4"" />
                  <stop offset=""100%"" stop-color=""#2563EB"" stop-opacity=""0.1"" />
                </linearGradient>
              </defs>
              <rect x=""20"" y=""20"" width=""360"" height=""180"" rx=""16"" fill=""url(#glowGrad)"" />
              <!-- Silhouette Car -->
              <path d=""M70 145 C90 145 110 135 130 115 L180 85 C200 75 250 75 275 88 L310 110 C330 120 345 140 345 145 L350 160 C350 165 340 170 330 170 L70 170 Z"" fill=""url(#carGrad)"" />
              <!-- Windows -->
              <path d=""M145 112 L185 86 C195 80 240 80 255 90 L285 112 Z"" fill=""#0B0F19"" opacity=""0.7"" />
              <!-- Headlights -->
              <path d=""M340 130 L350 135 L340 140 Z"" fill=""#F59E0B"" />
              <!-- Wheels -->
              <circle cx=""115"" cy=""170"" r=""22"" fill=""#0B0F19"" stroke=""#94A3B8"" stroke-width=""4"" />
              <circle cx=""115"" cy=""170"" r=""10"" fill=""#F59E0B"" />
              <circle cx=""295"" cy=""170"" r=""22"" fill=""#0B0F19"" stroke=""#94A3B8"" stroke-width=""4"" />
              <circle cx=""295"" cy=""170"" r=""10"" fill=""#F59E0B"" />
            </svg>
          </div>
          <div class=""hero-card-meta"">
            <h3>BMW 320i M Sport 2.0 Turbo</h3>
            <p>2024 • 14.200 km • Automático</p>
            <div class=""hero-card-price"">
              <span class=""price-tag"">R$ 319.900</span>
              <span class=""price-sub"">ou entrada + parcelas sob medida</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Seção de Estoque com Filtros -->
  <section class=""stock-section"" id=""estoque"">
    <div class=""container"">
      <div class=""section-header text-center"">
        <h2 class=""section-title"">Estoque Selecionado</h2>
        <p class=""section-desc"">Todos os veículos passam por rigorosa inspeção mecânica de 150 itens e possuem procedência garantida.</p>
      </div>

      <!-- Barra de Filtros e Busca -->
      <div class=""filter-controls"" id=""filter-bar"">
        <div class=""filter-tabs"">
          <button class=""filter-btn active"" data-category=""todos"">Todos (<span id=""count-todos"">6</span>)</button>
          <button class=""filter-btn"" data-category=""suv"">SUVs (<span id=""count-suv"">2</span>)</button>
          <button class=""filter-btn"" data-category=""sedan"">Sedans (<span id=""count-sedan"">2</span>)</button>
          <button class=""filter-btn"" data-category=""pickup"">Pick-ups (<span id=""count-pickup"">1</span>)</button>
          <button class=""filter-btn"" data-category=""hatch"">Hatchbacks (<span id=""count-hatch"">1</span>)</button>
        </div>

        <div class=""search-box"">
          <input type=""text"" id=""search-input"" placeholder=""Buscar por modelo, marca ou ano..."" aria-label=""Buscar veículo"">
        </div>
      </div>

      <!-- Grid de Veículos -->
      <div class=""vehicle-grid"" id=""vehicle-grid"">
        <!-- Renderizado dinamicamente via app.js -->
      </div>
    </div>
  </section>

  <!-- Simulador de Financiamento -->
  <section class=""simulator-section"" id=""simulador"">
    <div class=""container"">
      <div class=""simulator-card"">
        <div class=""simulator-info"">
          <div class=""badge-pill"">Simulação Online</div>
          <h2 class=""simulator-title"">Simule o Financiamento do seu Carro Novo</h2>
          <p class=""simulator-desc"">
            Sem compromisso. Escolha o valor aproximado, a entrada que deseja dar e o número de parcelas para ver a estimativa imediata.
          </p>
          <ul class=""sim-benefits"">
            <li>✔ Parcerias com os maiores bancos (Santander, BV, Itaú, Bradesco)</li>
            <li>✔ Taxas promocionais a partir de 0,99% a.m.</li>
            <li>✔ Aprovação rápida e desburocratizada</li>
          </ul>
        </div>

        <div class=""simulator-form"">
          <div class=""form-group"">
            <label for=""sim-car-select"">Escolha o Veículo:</label>
            <select id=""sim-car-select"" class=""form-control"">
              <!-- Populado via app.js -->
            </select>
          </div>

          <div class=""form-group"">
            <label for=""sim-entry-range"">Valor de Entrada: <strong id=""entry-display"">R$ 40.000</strong></label>
            <input type=""range"" id=""sim-entry-range"" min=""10000"" max=""250000"" step=""5000"" value=""40000"" class=""range-slider"">
          </div>

          <div class=""form-group"">
            <label>Número de Parcelas:</label>
            <div class=""installments-selector"" id=""installments-buttons"">
              <button type=""button"" class=""inst-btn"" data-months=""24"">24x</button>
              <button type=""button"" class=""inst-btn"" data-months=""36"">36x</button>
              <button type=""button"" class=""inst-btn active"" data-months=""48"">48x</button>
              <button type=""button"" class=""inst-btn"" data-months=""60"">60x</button>
            </div>
          </div>

          <div class=""simulator-result-box"">
            <div class=""result-label"">Estimativa de Parcela Mensal:</div>
            <div class=""result-value"" id=""installment-result"">R$ 2.450 / mês</div>
            <div class=""result-footnote"">*Taxa estimada referencial. Sujeita a análise cadastral.</div>
          </div>

          <button type=""button"" class=""btn btn-primary btn-block"" id=""btn-send-simulation"">
            Solicitar Proposta com essa Parcela via WhatsApp
          </button>
        </div>
      </div>
    </div>
  </section>

  <!-- Diferenciais e Garantias -->
  <section class=""differentiators-section"" id=""diferenciais"">
    <div class=""container"">
      <div class=""section-header text-center"">
        <h2 class=""section-title"">Por que Comprar na {spec.Cliente}?</h2>
        <p class=""section-desc"">Transparência absoluta em cada etapa da negociação para sua total tranquilidade.</p>
      </div>

      <div class=""diff-grid"">
        <div class=""diff-card"">
          <div class=""diff-icon"">🔍</div>
          <h3>Perícia Cautelar 100%</h3>
          <p>Nenhum veículo do nosso estoque possui histórico de leilão, sinistro ou alteração estrutural.</p>
        </div>

        <div class=""diff-card"">
          <div class=""diff-icon"">🛡️</div>
          <h3>Garantia Mecânica Completa</h3>
          <p>Garantia de motor e câmbio com suporte prioritário e oficina credenciada parceira.</p>
        </div>

        <div class=""diff-card"">
          <div class=""diff-icon"">🔄</div>
          <h3>Avaliação Justa na Troca</h3>
          <p>Pagamos o valor justo pelo seu veículo usado com base no mercado real, facilitando a troca imediata.</p>
        </div>

        <div class=""diff-card"">
          <div class=""diff-icon"">⚡</div>
          <h3>Documentação Expressa</h3>
          <p>Equipe de despachante dedicada para você sair com o carro rodando sem dores de cabeça burocráticas.</p>
        </div>
      </div>
    </div>
  </section>

  <!-- Depoimentos de Clientes -->
  <section class=""reviews-section"" id=""depoimentos"">
    <div class=""container"">
      <div class=""section-header text-center"">
        <h2 class=""section-title"">Quem Comprou, Recomenda</h2>
        <p class=""section-desc"">Confira o que nossos clientes dizem sobre o atendimento e a experiência.</p>
      </div>

      <div class=""reviews-grid"">
        <div class=""review-card"">
          <div class=""review-stars"">★★★★★</div>
          <p class=""review-text"">""Atendimento impecável! O processo de financiamento foi aprovado no mesmo dia e o carro estava exatamente conforme o anunciado.""</p>
          <div class=""review-author"">
            <div class=""author-avatar"">MR</div>
            <div>
              <strong>Marcelo Rocha</strong>
              <span>Comprou um Jeep Compass</span>
            </div>
          </div>
        </div>

        <div class=""review-card"">
          <div class=""review-stars"">★★★★★</div>
          <p class=""review-text"">""A {spec.Cliente} superou as expectativas. Avaliaram meu carro usado de forma honesta e a entrega foi super ágil.""</p>
          <div class=""review-author"">
            <div class=""author-avatar"">CS</div>
            <div>
              <strong>Camila Silveira</strong>
              <span>Comprou um Honda Civic</span>
            </div>
          </div>
        </div>

        <div class=""review-card"">
          <div class=""review-stars"">★★★★★</div>
          <p class=""review-text"">""Transparência do início ao fim. Mostraram todo o laudo pericial antes mesmo de fecharmos o negócio. Recomendo de olhos fechados!""</p>
          <div class=""review-author"">
            <div class=""author-avatar"">FA</div>
            <div>
              <strong>Fernando Albuquerque</strong>
              <span>Comprou uma Toyota Hilux</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- Seção de Contato e Localização -->
  <section class=""contact-section"" id=""contato"">
    <div class=""container contact-grid"">
      <div class=""contact-info"">
        <h2 class=""section-title"">Venha Tomar um Café Conosco</h2>
        <p class=""contact-lead"">Estamos prontos para atendê-lo em nosso showroom ou pelo WhatsApp com atendimento exclusivo.</p>

        <div class=""info-items"">
          <div class=""info-item"">
            <span class=""info-icon"">📍</span>
            <div>
              <strong>Endereço do Showroom</strong>
              <p>Av. das Américas, 4200 - Barra da Tijuca, RJ</p>
            </div>
          </div>

          <div class=""info-item"">
            <span class=""info-icon"">🕒</span>
            <div>
              <strong>Horário de Funcionamento</strong>
              <p>Segunda a Sexta: 08:30 às 19:00 | Sábado: 09:00 às 17:00</p>
            </div>
          </div>

          <div class=""info-item"">
            <span class=""info-icon"">💬</span>
            <div>
              <strong>Atendimento Digital</strong>
              <p>(11) 99999-9999 • contato@{spec.Slug}.com.br</p>
            </div>
          </div>
        </div>
      </div>

      <div class=""contact-action-box"">
        <h3>Atendimento Imediato</h3>
        <p>Quer tirar dúvidas sobre algum modelo do estoque ou agendar um test-drive?</p>
        <a href=""https://wa.me/5511999999999?text=Olá!+Gostaria+de+agendar+uma+visita+na+{Uri.EscapeDataString(spec.Cliente)}"" target=""_blank"" rel=""noopener noreferrer"" class=""btn btn-primary btn-block btn-lg"">
          Iniciar Conversa no WhatsApp
        </a>
      </div>
    </div>
  </section>

  <!-- Rodapé -->
  <footer class=""footer"" id=""footer"">
    <div class=""container footer-content"">
      <div class=""footer-brand"">
        <span class=""brand-icon"">⚡</span>
        <span class=""brand-name"">{spec.Cliente}</span>
        <p class=""footer-tagline"">Veículos selecionados com qualidade, garantia e procedência garantida.</p>
      </div>

      <div class=""footer-links"">
        <a href=""#estoque"">Estoque</a>
        <a href=""#simulador"">Simulador</a>
        <a href=""#diferenciais"">Diferenciais</a>
        <a href=""#contato"">Contato</a>
      </div>

      <div class=""footer-legal"">
        <p>© 2026 {spec.Cliente}. Todos os direitos reservados.</p>
        <p class=""powered-by"">Desenvolvido sob medida por <strong>SiteFactory Orchestrator</strong>.</p>
      </div>
    </div>
  </footer>

  <!-- Botão Flutuante de WhatsApp -->
  <a href=""https://wa.me/5511999999999?text=Olá!+Gostaria+de+informações+sobre+o+estoque+da+{Uri.EscapeDataString(spec.Cliente)}"" target=""_blank"" rel=""noopener noreferrer"" class=""floating-whatsapp"" id=""floating-whatsapp"" aria-label=""Conversar no WhatsApp"">
    <span>💬</span>
  </a>

  <!-- Script da Aplicação -->
  <script src=""scripts/app.js""></script>
</body>
</html>";
    }

    private static string GenerateCss()
    {
        return @":root {
  --bg-base: #0B0F19;
  --bg-surface: #111827;
  --bg-surface-elevated: #1E293B;
  --bg-card: rgba(17, 24, 39, 0.7);
  --primary: #F59E0B;
  --primary-hover: #D97706;
  --accent-blue: #2563EB;
  --accent-blue-hover: #1D4ED8;
  --text-primary: #F8FAFC;
  --text-secondary: #94A3B8;
  --text-muted: #64748B;
  --border-subtle: rgba(255, 255, 255, 0.08);
  --border-glow: rgba(245, 158, 11, 0.35);
  --success: #10B981;
  --radius-sm: 8px;
  --radius-md: 14px;
  --radius-lg: 20px;
  --transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  --shadow-card: 0 10px 30px -10px rgba(0, 0, 0, 0.5);
  --font-family: 'Plus Jakarta Sans', system-ui, -apple-system, sans-serif;
}

/* Reset Básico */
*, *::before, *::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

html {
  scroll-behavior: smooth;
  font-family: var(--font-family);
  background-color: var(--bg-base);
  color: var(--text-primary);
}

body {
  line-height: 1.6;
  overflow-x: hidden;
}

.container {
  width: 90%;
  max-width: 1200px;
  margin: 0 auto;
}

/* Tipografia */
h1, h2, h3, h4 {
  font-weight: 700;
  line-height: 1.2;
}

.text-gradient {
  background: linear-gradient(135deg, #F59E0B 0%, #FBBF24 50%, #3B82F6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.text-center {
  text-align: center;
}

/* Botões */
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 12px 24px;
  border-radius: var(--radius-sm);
  font-weight: 600;
  font-size: 0.95rem;
  text-decoration: none;
  cursor: pointer;
  border: 1px solid transparent;
  transition: var(--transition);
}

.btn-primary {
  background-color: var(--primary);
  color: #0B0F19;
}
.btn-primary:hover {
  background-color: var(--primary-hover);
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(245, 158, 11, 0.35);
}

.btn-secondary {
  background-color: rgba(255, 255, 255, 0.05);
  color: var(--text-primary);
  border-color: var(--border-subtle);
}
.btn-secondary:hover {
  background-color: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.2);
}

.btn-sm {
  padding: 8px 16px;
  font-size: 0.85rem;
}

.btn-lg {
  padding: 14px 30px;
  font-size: 1.05rem;
}

.btn-block {
  width: 100%;
}

/* Badges */
.badge-pill {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 14px;
  background: rgba(245, 158, 11, 0.12);
  border: 1px solid rgba(245, 158, 11, 0.3);
  border-radius: 999px;
  font-size: 0.82rem;
  color: var(--primary);
  font-weight: 600;
  margin-bottom: 16px;
}

.badge-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background-color: var(--primary);
  box-shadow: 0 0 8px var(--primary);
}

/* Navbar */
.navbar-header {
  position: sticky;
  top: 0;
  z-index: 100;
  background: rgba(11, 15, 25, 0.85);
  backdrop-filter: blur(14px);
  border-bottom: 1px solid var(--border-subtle);
  padding: 16px 0;
}

.nav-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.brand-logo {
  display: flex;
  align-items: center;
  gap: 10px;
  text-decoration: none;
  color: var(--text-primary);
  font-size: 1.25rem;
  font-weight: 800;
}

.brand-icon {
  font-size: 1.4rem;
}

.nav-menu {
  display: flex;
  gap: 28px;
}

.nav-link {
  color: var(--text-secondary);
  text-decoration: none;
  font-weight: 500;
  font-size: 0.95rem;
  transition: var(--transition);
}

.nav-link:hover {
  color: var(--primary);
}

/* Hero Section */
.hero-section {
  padding: 80px 0 60px;
  position: relative;
}

.hero-grid {
  display: grid;
  grid-template-columns: 1.2fr 0.8fr;
  gap: 48px;
  align-items: center;
}

.hero-title {
  font-size: clamp(2.3rem, 4.5vw, 3.4rem);
  letter-spacing: -0.02em;
  margin-bottom: 20px;
}

.hero-subtitle {
  font-size: 1.1rem;
  color: var(--text-secondary);
  margin-bottom: 32px;
  max-width: 580px;
}

.hero-actions {
  display: flex;
  gap: 16px;
  flex-wrap: wrap;
  margin-bottom: 48px;
}

.hero-stats {
  display: flex;
  align-items: center;
  gap: 24px;
}

.stat-item {
  display: flex;
  flex-direction: column;
}

.stat-number {
  font-size: 1.6rem;
  font-weight: 800;
  color: var(--text-primary);
}

.stat-label {
  font-size: 0.85rem;
  color: var(--text-muted);
}

.stat-divider {
  width: 1px;
  height: 36px;
  background-color: var(--border-subtle);
}

/* Hero Visual Card */
.hero-visual-card {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-lg);
  padding: 24px;
  box-shadow: var(--shadow-card);
  position: relative;
}

.card-badge-top {
  position: absolute;
  top: 16px;
  right: 16px;
  background: var(--accent-blue);
  color: #fff;
  font-size: 0.75rem;
  font-weight: 700;
  padding: 4px 10px;
  border-radius: 999px;
  text-transform: uppercase;
}

.vehicle-illustration {
  margin: 20px 0 16px;
}

.car-svg {
  width: 100%;
  height: auto;
}

.hero-card-meta h3 {
  font-size: 1.2rem;
  margin-bottom: 4px;
}

.hero-card-meta p {
  font-size: 0.9rem;
  color: var(--text-secondary);
  margin-bottom: 14px;
}

.hero-card-price {
  display: flex;
  flex-direction: column;
}

.price-tag {
  font-size: 1.6rem;
  font-weight: 800;
  color: var(--primary);
}

.price-sub {
  font-size: 0.8rem;
  color: var(--text-muted);
}

/* Seções Gerais */
.section-header {
  margin-bottom: 40px;
}

.section-title {
  font-size: clamp(1.8rem, 3.2vw, 2.5rem);
  margin-bottom: 12px;
}

.section-desc {
  color: var(--text-secondary);
  max-width: 600px;
  margin: 0 auto;
}

/* Estoque */
.stock-section {
  padding: 80px 0;
  background-color: #0d1320;
}

.filter-controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 16px;
  margin-bottom: 32px;
}

.filter-tabs {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.filter-btn {
  background: var(--bg-surface);
  color: var(--text-secondary);
  border: 1px solid var(--border-subtle);
  padding: 8px 16px;
  border-radius: 999px;
  font-size: 0.88rem;
  cursor: pointer;
  transition: var(--transition);
}

.filter-btn:hover {
  color: var(--text-primary);
  border-color: rgba(255, 255, 255, 0.2);
}

.filter-btn.active {
  background: var(--primary);
  color: #0B0F19;
  font-weight: 700;
  border-color: var(--primary);
}

.search-box input {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  color: var(--text-primary);
  padding: 10px 18px;
  border-radius: var(--radius-sm);
  font-size: 0.9rem;
  width: 280px;
  outline: none;
  transition: var(--transition);
}

.search-box input:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(245, 158, 11, 0.15);
}

/* Grid de Veículos */
.vehicle-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 24px;
}

.vehicle-card {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-md);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  transition: var(--transition);
  box-shadow: var(--shadow-card);
}

.vehicle-card:hover {
  transform: translateY(-5px);
  border-color: var(--border-glow);
  box-shadow: 0 16px 32px -8px rgba(0, 0, 0, 0.7);
}

.card-img-wrapper {
  position: relative;
  background: #141c2f;
  padding: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.card-tag {
  position: absolute;
  top: 12px;
  left: 12px;
  background: rgba(16, 185, 129, 0.15);
  border: 1px solid rgba(16, 185, 129, 0.4);
  color: var(--success);
  font-size: 0.72rem;
  font-weight: 700;
  padding: 3px 8px;
  border-radius: 4px;
}

.card-body {
  padding: 20px;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.card-body h3 {
  font-size: 1.15rem;
  margin-bottom: 6px;
}

.vehicle-tags {
  display: flex;
  gap: 12px;
  font-size: 0.82rem;
  color: var(--text-muted);
  margin-bottom: 16px;
}

.vehicle-pricing {
  margin-top: auto;
  padding-top: 16px;
  border-top: 1px solid var(--border-subtle);
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  margin-bottom: 16px;
}

.price-main {
  font-size: 1.35rem;
  font-weight: 800;
  color: var(--text-primary);
}

.price-installment {
  font-size: 0.8rem;
  color: var(--primary);
  font-weight: 600;
}

/* Simulador */
.simulator-section {
  padding: 80px 0;
}

.simulator-card {
  background: linear-gradient(145deg, #131b2e 0%, #0d121e 100%);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-lg);
  padding: 48px;
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 48px;
  align-items: center;
}

.simulator-title {
  font-size: 2rem;
  margin-bottom: 16px;
}

.simulator-desc {
  color: var(--text-secondary);
  margin-bottom: 24px;
}

.sim-benefits {
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: 12px;
  color: var(--text-primary);
  font-size: 0.95rem;
}

.simulator-form {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-md);
  padding: 32px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  font-size: 0.9rem;
  font-weight: 600;
  margin-bottom: 8px;
  color: var(--text-primary);
}

.form-control {
  width: 100%;
  padding: 12px 16px;
  border-radius: var(--radius-sm);
  background: #0B0F19;
  border: 1px solid var(--border-subtle);
  color: var(--text-primary);
  font-size: 0.95rem;
  outline: none;
}

.range-slider {
  width: 100%;
  accent-color: var(--primary);
  cursor: pointer;
}

.installments-selector {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 8px;
}

.inst-btn {
  background: #0B0F19;
  border: 1px solid var(--border-subtle);
  color: var(--text-secondary);
  padding: 10px;
  border-radius: var(--radius-sm);
  font-weight: 600;
  cursor: pointer;
  transition: var(--transition);
}

.inst-btn.active {
  background: var(--primary);
  color: #0B0F19;
  border-color: var(--primary);
}

.simulator-result-box {
  background: rgba(245, 158, 11, 0.08);
  border: 1px solid rgba(245, 158, 11, 0.25);
  border-radius: var(--radius-sm);
  padding: 16px;
  text-align: center;
  margin-bottom: 20px;
}

.result-label {
  font-size: 0.85rem;
  color: var(--text-secondary);
}

.result-value {
  font-size: 1.8rem;
  font-weight: 800;
  color: var(--primary);
  margin: 4px 0;
}

.result-footnote {
  font-size: 0.72rem;
  color: var(--text-muted);
}

/* Diferenciais */
.differentiators-section {
  padding: 80px 0;
  background-color: #0d1320;
}

.diff-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
  gap: 24px;
}

.diff-card {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-md);
  padding: 28px;
  transition: var(--transition);
}

.diff-card:hover {
  transform: translateY(-4px);
  border-color: rgba(255, 255, 255, 0.2);
}

.diff-icon {
  font-size: 2.2rem;
  margin-bottom: 16px;
}

.diff-card h3 {
  font-size: 1.15rem;
  margin-bottom: 8px;
}

.diff-card p {
  color: var(--text-secondary);
  font-size: 0.9rem;
}

/* Depoimentos */
.reviews-section {
  padding: 80px 0;
}

.reviews-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 24px;
}

.review-card {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-md);
  padding: 28px;
  display: flex;
  flex-direction: column;
}

.review-stars {
  color: var(--primary);
  margin-bottom: 12px;
  font-size: 1.1rem;
}

.review-text {
  color: var(--text-secondary);
  font-style: italic;
  font-size: 0.95rem;
  margin-bottom: 20px;
  flex: 1;
}

.review-author {
  display: flex;
  align-items: center;
  gap: 12px;
}

.author-avatar {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  background: var(--accent-blue);
  color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 0.9rem;
}

.review-author strong {
  display: block;
  font-size: 0.9rem;
}

.review-author span {
  font-size: 0.78rem;
  color: var(--text-muted);
}

/* Contato */
.contact-section {
  padding: 80px 0;
  background-color: #0d1320;
}

.contact-grid {
  display: grid;
  grid-template-columns: 1.2fr 0.8fr;
  gap: 48px;
  align-items: center;
}

.contact-lead {
  color: var(--text-secondary);
  margin-bottom: 32px;
}

.info-items {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.info-item {
  display: flex;
  gap: 16px;
  align-items: flex-start;
}

.info-icon {
  font-size: 1.5rem;
}

.info-item p {
  color: var(--text-secondary);
  font-size: 0.9rem;
}

.contact-action-box {
  background: var(--bg-surface);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius-lg);
  padding: 36px;
  text-align: center;
}

.contact-action-box h3 {
  font-size: 1.4rem;
  margin-bottom: 10px;
}

.contact-action-box p {
  color: var(--text-secondary);
  margin-bottom: 24px;
  font-size: 0.95rem;
}

/* Rodapé */
.footer {
  border-top: 1px solid var(--border-subtle);
  padding: 40px 0;
  background: #080c14;
}

.footer-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 20px;
}

.footer-brand .brand-name {
  font-weight: 800;
  font-size: 1.2rem;
}

.footer-tagline {
  color: var(--text-muted);
  font-size: 0.85rem;
  margin-top: 6px;
}

.footer-links {
  display: flex;
  gap: 24px;
}

.footer-links a {
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.9rem;
}

.footer-links a:hover {
  color: var(--primary);
}

.footer-legal {
  color: var(--text-muted);
  font-size: 0.8rem;
}

.powered-by strong {
  color: var(--primary);
}

/* Botão Flutuante de WhatsApp */
.floating-whatsapp {
  position: fixed;
  bottom: 24px;
  right: 24px;
  width: 58px;
  height: 58px;
  background-color: #25D366;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.8rem;
  box-shadow: 0 10px 25px rgba(37, 211, 102, 0.4);
  z-index: 999;
  text-decoration: none;
  transition: var(--transition);
}

.floating-whatsapp:hover {
  transform: scale(1.1);
}

/* Responsividade Mobile */
@media (max-width: 900px) {
  .hero-grid, .simulator-card, .contact-grid {
    grid-template-columns: 1fr;
    gap: 32px;
  }
  .nav-menu {
    display: none;
  }
  .simulator-card {
    padding: 24px;
  }
}
";
    }

    private static string GenerateJs(ProjectSpecification spec)
    {
        return $@"// SiteFactory - Aplicação Interativa do Cliente
// Cliente: {spec.Cliente} ({spec.Tipo})

const vehiclesData = [
  {{
    id: 1,
    title: 'BMW 320i M Sport 2.0 Turbo',
    category: 'sedan',
    year: '2024',
    km: '14.200 km',
    gear: 'Automático',
    price: 319900,
    tag: 'Único Dono',
    color: '#2563EB'
  }},
  {{
    id: 2,
    title: 'Jeep Compass Limited T270',
    category: 'suv',
    year: '2023',
    km: '28.500 km',
    gear: 'Automático',
    price: 169900,
    tag: 'Garantia de Fábrica',
    color: '#059669'
  }},
  {{
    id: 3,
    title: 'Toyota Hilux SRX 2.8 4x4 Diesel',
    category: 'pickup',
    year: '2023',
    km: '34.000 km',
    gear: 'Automático',
    price: 289900,
    tag: 'Laudo 100% Aprovado',
    color: '#DC2626'
  }},
  {{
    id: 4,
    title: 'Porsche Macan 2.0 Turbo PDK',
    category: 'suv',
    year: '2022',
    km: '21.000 km',
    gear: 'Automático',
    price: 459000,
    tag: 'Blindado Nível III-A',
    color: '#7C3AED'
  }},
  {{
    id: 5,
    title: 'Honda Civic Touring 1.5 Turbo',
    category: 'sedan',
    year: '2021',
    km: '42.000 km',
    gear: 'Automático',
    price: 142900,
    tag: 'IPVA 2026 Pago',
    color: '#D97706'
  }},
  {{
    id: 6,
    title: 'Volkswagen Golf GTI 2.0 TSI',
    category: 'hatch',
    year: '2020',
    km: '51.000 km',
    gear: 'DSG Automático',
    price: 185000,
    tag: 'Revisões na Concessionária',
    color: '#E11D48'
  }}
];

const clientName = '{EscapeJs(spec.Cliente)}';
let activeCategory = 'todos';
let searchQuery = '';
let selectedMonths = 48;

function formatCurrency(val) {{
  return new Intl.NumberFormat('pt-BR', {{ style: 'currency', currency: 'BRL', maximumFractionDigits: 0 }}).format(val);
}}

function renderVehicles() {{
  const grid = document.getElementById('vehicle-grid');
  if (!grid) return;

  const filtered = vehiclesData.filter(v => {{
    const matchesCategory = activeCategory === 'todos' || v.category === activeCategory;
    const matchesSearch = v.title.toLowerCase().includes(searchQuery.toLowerCase()) || 
                          v.year.includes(searchQuery);
    return matchesCategory && matchesSearch;
  }});

  if (filtered.length === 0) {{
    grid.innerHTML = `
      <div style=""grid-column: 1 / -1; text-align: center; padding: 48px; color: var(--text-secondary);"">
        <h3>Nenhum veículo encontrado</h3>
        <p>Tente ajustar os filtros ou termo de busca.</p>
      </div>`;
    return;
  }}

  grid.innerHTML = filtered.map(v => {{
    const estimatedEntry = v.price * 0.3;
    const estimatedInstallment = (v.price - estimatedEntry) * 1.35 / 48;
    const encodedMsg = encodeURIComponent(`Olá! Tenho interesse no veículo ${{v.title}} (${{formatCurrency(v.price)}}) anunciado no site da ${{clientName}}.`);

    return `
      <article class=""vehicle-card"" data-id=""${{v.id}}"">
        <div class=""card-img-wrapper"">
          <span class=""card-tag"">${{v.tag}}</span>
          <svg viewBox=""0 0 240 120"" width=""200"" height=""100"" fill=""none"">
            <rect x=""10"" y=""10"" width=""220"" height=""100"" rx=""12"" fill=""rgba(255,255,255,0.03)"" />
            <path d=""M40 75 C55 75 65 70 80 55 L115 35 C130 30 160 30 175 40 L195 55 C210 60 220 70 220 75 L225 85 C225 90 220 92 215 92 L40 92 Z"" fill=""${{v.color}}"" opacity=""0.9"" />
            <circle cx=""75"" cy=""92"" r=""12"" fill=""#0B0F19"" stroke=""#94A3B8"" stroke-width=""2"" />
            <circle cx=""180"" cy=""92"" r=""12"" fill=""#0B0F19"" stroke=""#94A3B8"" stroke-width=""2"" />
          </svg>
        </div>
        <div class=""card-body"">
          <h3>${{v.title}}</h3>
          <div class=""vehicle-tags"">
            <span>📅 ${{v.year}}</span>
            <span>🛣️ ${{v.km}}</span>
            <span>⚙️ ${{v.gear}}</span>
          </div>
          <div class=""vehicle-pricing"">
            <div>
              <div class=""price-main"">${{formatCurrency(v.price)}}</div>
              <div class=""price-installment"">Entrada ${{formatCurrency(estimatedEntry)}} + 48x de ${{formatCurrency(estimatedInstallment)}}</div>
            </div>
          </div>
          <a href=""https://wa.me/5511999999999?text=${{encodedMsg}}"" target=""_blank"" rel=""noopener noreferrer"" class=""btn btn-primary btn-block"">
            Tenho Interesse
          </a>
        </div>
      </article>
    `;
  }}).join('');
}}

function initFilters() {{
  const filterBtns = document.querySelectorAll('.filter-btn');
  filterBtns.forEach(btn => {{
    btn.addEventListener('click', () => {{
      filterBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      activeCategory = btn.getAttribute('data-category');
      renderVehicles();
    }});
  }});

  const searchInput = document.getElementById('search-input');
  if (searchInput) {{
    searchInput.addEventListener('input', (e) => {{
      searchQuery = e.target.value.trim();
      renderVehicles();
    }});
  }}
}}

function initSimulator() {{
  const carSelect = document.getElementById('sim-car-select');
  const entryRange = document.getElementById('sim-entry-range');
  const entryDisplay = document.getElementById('entry-display');
  const installmentResult = document.getElementById('installment-result');
  const instBtns = document.querySelectorAll('.inst-btn');
  const btnSend = document.getElementById('btn-send-simulation');

  if (!carSelect || !entryRange) return;

  // Popula opções de veículos
  carSelect.innerHTML = vehiclesData.map(v => 
    `<option value=""${{v.price}}"" data-title=""${{v.title}}"">${{v.title}} - ${{formatCurrency(v.price)}}</option>`
  ).join('');

  function updateCalculation() {{
    const carPrice = parseFloat(carSelect.value) || 200000;
    const maxEntry = Math.round(carPrice * 0.85);
    entryRange.max = maxEntry;
    
    let entryVal = parseFloat(entryRange.value);
    if (entryVal > maxEntry) {{
      entryVal = maxEntry;
      entryRange.value = maxEntry;
    }}

    entryDisplay.textContent = formatCurrency(entryVal);

    const financedAmount = Math.max(0, carPrice - entryVal);
    // Coeficiente de juros simples aproximado de mercado (1.2% ao mês)
    const monthlyRate = 0.0125;
    const factor = (monthlyRate * Math.pow(1 + monthlyRate, selectedMonths)) / (Math.pow(1 + monthlyRate, selectedMonths) - 1);
    const monthlyInstallment = financedAmount * factor;

    installmentResult.textContent = `${{formatCurrency(monthlyInstallment)}} / mês`;
  }}

  carSelect.addEventListener('change', updateCalculation);
  entryRange.addEventListener('input', updateCalculation);

  instBtns.forEach(btn => {{
    btn.addEventListener('click', () => {{
      instBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      selectedMonths = parseInt(btn.getAttribute('data-months'), 10);
      updateCalculation();
    }});
  }});

  if (btnSend) {{
    btnSend.addEventListener('click', () => {{
      const selectedOption = carSelect.options[carSelect.selectedIndex];
      const carName = selectedOption.getAttribute('data-title');
      const entryVal = entryDisplay.textContent;
      const installmentVal = installmentResult.textContent;
      
      const text = encodeURIComponent(`Olá! Fiz uma simulação no site da ${{clientName}} para o veículo ${{carName}}:\n- Entrada: ${{entryVal}}\n- Prazo: ${{selectedMonths}}x\n- Parcela estimada: ${{installmentVal}}\nGostaria de consultar as condições de aprovação.`);
      window.open(`https://wa.me/5511999999999?text=${{text}}`, '_blank');
    }});
  }}

  updateCalculation();
}}

document.addEventListener('DOMContentLoaded', () => {{
  renderVehicles();
  initFilters();
  initSimulator();
}});
";
    }

    private static string EscapeJs(string text) => text.Replace("'", "\\'").Replace("\"", "\\\"");
}
