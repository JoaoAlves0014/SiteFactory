# Especificação Anatômica de Componentes
**Cliente:** Jordan Veículos  

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
