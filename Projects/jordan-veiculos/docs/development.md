# Documentação Técnica de Desenvolvimento Frontend
**Projeto:** Jordan Veículos  
**Segmento:** automotivo  
**Data da Implementação:** 14/10/2026 16:23:40 UTC  

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
