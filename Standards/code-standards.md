# Padrões de Código do SiteFactory

1. **HTML5**:
   - Sempre incluir `<!DOCTYPE html>`, `<html lang="pt-BR">`.
   - Utilizar tags semânticas: `<header>`, `<nav>`, `<main>`, `<section>`, `<article>`, `<footer>`.
   - Incluir metadados completos (`viewport`, `description`, `theme-color`, Open Graph básicos).
   - Um único `<h1>` por página.
   - IDs únicos e descritivos em todos os elementos interativos para facilitar testes e automações.

2. **CSS**:
   - Utilizar CSS Custom Properties (`--color-primary`, `--color-bg`, etc.) no `:root`.
   - Design Responsivo Mobile-First ou Desktop com breakpoints fluidos (`clamp`, `grid`, `flex`).
   - Efeitos visuais modernos: sombras suaves (`box-shadow`), gradientes elegantes, glassmorphism com `backdrop-filter`.

3. **JavaScript**:
   - Código limpo, moderno (ES6+), modular e sem bibliotecas pesadas desnecessárias.
   - Tratar exceções e garantir feedback visual imediato para ações do usuário (ex: cálculo de parcelas, filtragem de catálogo).
