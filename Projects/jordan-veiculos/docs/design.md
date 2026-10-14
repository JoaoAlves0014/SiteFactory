# Design System & Identidade Visual
**Projeto:** Jordan Veículos  
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
