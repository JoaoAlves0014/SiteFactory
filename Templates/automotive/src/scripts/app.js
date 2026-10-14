// Identificador de WhatsApp para atendimento
const WHATSAPP_NUMBER = "{{WHATSAPP_NUMBER}}";

/**
 * Retorna link seguro para o WhatsApp ou âncora para contato se o número não estiver configurado
 */
function getWhatsAppLink(message) {
  if (WHATSAPP_NUMBER && WHATSAPP_NUMBER.length >= 10) {
    return `https://wa.me/${WHATSAPP_NUMBER}?text=${encodeURIComponent(message)}`;
  }
  return "#contato";
}

// Dados demonstrativos de veículos claramente identificados
const demonstrativeVehicles = [
  {
    id: 1,
    name: "SUV Vision 1.6 Flex Turbo",
    category: "suv",
    year: 2023,
    mileage: "24.500 km",
    fuel: "Flex",
    transmission: "Automático",
    price: 98000,
    tag: "Destaque"
  },
  {
    id: 2,
    name: "Sedan Elegance 2.0 Turbo",
    category: "sedan",
    year: 2022,
    mileage: "36.200 km",
    fuel: "Gasolina",
    transmission: "Automático",
    price: 125000,
    tag: "Laudo 100%"
  },
  {
    id: 3,
    name: "Hatch Sport 1.0 Turbo TSI",
    category: "hatch",
    year: 2023,
    mileage: "18.900 km",
    fuel: "Flex",
    transmission: "Manual",
    price: 76000,
    tag: "Oportunidade"
  },
  {
    id: 4,
    name: "Pick-up Robust 2.8 4x4 Diesel",
    category: "pickup",
    year: 2021,
    mileage: "52.000 km",
    fuel: "Diesel",
    transmission: "Automático",
    price: 165000,
    tag: "Tração 4x4"
  },
  {
    id: 5,
    name: "SUV Premium Titanium Hybrid",
    category: "suv",
    year: 2024,
    mileage: "8.400 km",
    fuel: "Híbrido",
    transmission: "Automático",
    price: 148000,
    tag: "Seminovo Único"
  },
  {
    id: 6,
    name: "Hatch Confort 1.0 Flex",
    category: "hatch",
    year: 2022,
    mileage: "29.800 km",
    fuel: "Flex",
    transmission: "Manual",
    price: 68000,
    tag: "Econômico"
  }
];

// Estado da aplicação
let activeCategory = "todos";
let searchTerm = "";
let selectedTermMonths = 48;

/**
 * Formata valores numéricos para a moeda brasileira (BRL)
 */
function formatCurrency(value) {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL"
  }).format(value);
}

/**
 * Gera ilustração vetorial (SVG) limpa para os cards de veículos
 */
function getVehicleSvg(category) {
  return `
    <svg viewBox="0 0 200 100" class="card-car-svg" fill="none" xmlns="http://www.w3.org/2000/svg" style="width: 75%; height: auto; filter: drop-shadow(0 4px 6px rgba(0,0,0,0.4));">
      <path d="M20 70 C40 70 50 50 70 45 C90 40 120 40 140 50 C155 58 170 70 180 70" stroke="#E5A93C" stroke-width="3" stroke-linecap="round" />
      <path d="M60 48 L80 28 L120 28 L138 48 Z" fill="rgba(37, 99, 235, 0.25)" stroke="#2563EB" stroke-width="1.5" />
      <circle cx="50" cy="70" r="12" fill="#111827" stroke="#E5A93C" stroke-width="3" />
      <circle cx="50" cy="70" r="4" fill="#E5A93C" />
      <circle cx="150" cy="70" r="12" fill="#111827" stroke="#E5A93C" stroke-width="3" />
      <circle cx="150" cy="70" r="4" fill="#E5A93C" />
      <line x1="65" y1="70" x2="135" y2="70" stroke="#475569" stroke-width="2" />
    </svg>
  `;
}

/**
 * Renderiza a grade de veículos filtrada (Requisito TesterAgent)
 */
function renderVehicles(vehicles = demonstrativeVehicles) {
  const container = document.getElementById("vehicle-grid");
  if (!container) return;

  const filtered = vehicles.filter(v => {
    const matchCategory = activeCategory === "todos" || v.category === activeCategory;
    const matchSearch = searchTerm === "" || 
      v.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      v.year.toString().includes(searchTerm);
    return matchCategory && matchSearch;
  });

  if (filtered.length === 0) {
    container.innerHTML = `
      <div style="grid-column: 1 / -1; text-align: center; padding: 3rem; background: var(--bg-surface-card); border-radius: var(--radius-md); border: 1px solid var(--border-subtle);">
        <p style="font-size: 1.1rem; color: var(--text-secondary);">Nenhum veículo encontrado para a busca selecionada.</p>
        <button onclick="resetFilters()" class="btn btn-secondary btn-sm" style="margin-top: 1rem;">Limpar Filtros</button>
      </div>
    `;
    return;
  }

  container.innerHTML = filtered.map(v => `
    <article class="vehicle-card" data-category="${v.category}">
      <div class="vehicle-card-visual">
        <span class="vehicle-tag">${v.tag}</span>
        ${getVehicleSvg(v.category)}
      </div>
      <div class="vehicle-body">
        <h3 class="vehicle-name">${v.name}</h3>
        <div class="vehicle-specs">
          <span>📅 ${v.year}</span>
          <span>🛣️ ${v.mileage}</span>
          <span>🕹️ ${v.transmission}</span>
        </div>
        <div class="vehicle-price-box">
          <div>
            <span class="price-label">Preço Sugerido (Demonstrativo):</span>
            <span class="price-val">${formatCurrency(v.price)}</span>
          </div>
          <a href="${getWhatsAppLink(`Olá! Tenho interesse no veículo demonstrativo ${v.name} (R$ ${v.price}).`)}" 
             ${WHATSAPP_NUMBER && WHATSAPP_NUMBER.length >= 10 ? 'target="_blank" rel="noopener noreferrer"' : ''}
             class="btn btn-primary btn-sm" aria-label="Consultar veículo ${v.name} pelo WhatsApp">
            Consultar
          </a>
        </div>
      </div>
    </article>
  `).join("");
}

/**
 * Limpa os filtros de busca
 */
function resetFilters() {
  activeCategory = "todos";
  searchTerm = "";
  const searchInput = document.getElementById("search-input");
  if (searchInput) searchInput.value = "";
  
  document.querySelectorAll(".category-btn").forEach(btn => {
    btn.classList.toggle("active", btn.dataset.category === "todos");
  });
  
  renderVehicles();
}

/**
 * Inicializa e calcula o simulador financeiro (Requisito TesterAgent)
 */
function initSimulator() {
  const vehicleSelect = document.getElementById("sim-vehicle-select");
  const entryInput = document.getElementById("sim-entry-input");
  const monthlyDisplay = document.getElementById("sim-monthly-installment");
  const totalDisplay = document.getElementById("sim-total-display");
  const entryDisplay = document.getElementById("sim-entry-display");
  const balanceDisplay = document.getElementById("sim-balance-display");
  const termDisplay = document.getElementById("sim-term-display");
  const sendWhatsAppBtn = document.getElementById("sim-send-whatsapp");
  const termButtons = document.querySelectorAll(".term-btn");

  if (!vehicleSelect || !entryInput || !monthlyDisplay) return;

  function calculate() {
    const totalValue = parseFloat(vehicleSelect.value) || 0;
    let entryValue = parseFloat(entryInput.value) || 0;

    // Regras de validação do valor de entrada
    if (entryValue < 0) entryValue = 0;
    if (entryValue > totalValue * 0.9) entryValue = totalValue * 0.9;

    const balance = totalValue - entryValue;
    const months = selectedTermMonths;

    // Taxa ilustrativa de 1.49% a.m. (cálculo pela Tabela Price para fins puramente demonstrativos)
    const monthlyRate = 0.0149;
    let monthlyInstallment = 0;

    if (balance > 0 && months > 0) {
      monthlyInstallment = balance * (monthlyRate * Math.pow(1 + monthlyRate, months)) / (Math.pow(1 + monthlyRate, months) - 1);
    }

    // Atualiza a interface
    totalDisplay.textContent = formatCurrency(totalValue);
    entryDisplay.textContent = formatCurrency(entryValue);
    balanceDisplay.textContent = formatCurrency(balance);
    termDisplay.textContent = `${months} meses`;
    monthlyDisplay.textContent = formatCurrency(monthlyInstallment);

    // Atualiza botão do WhatsApp com link dinâmico wa.me
    const selectedOptionText = vehicleSelect.options[vehicleSelect.selectedIndex].text.split(" - ")[0];
    const message = `Olá! Realizei uma simulação demonstrativa pelo site:\n- Veículo: ${selectedOptionText}\n- Valor: ${formatCurrency(totalValue)}\n- Entrada: ${formatCurrency(entryValue)}\n- Prazo: ${months}x de aprox. ${formatCurrency(monthlyInstallment)}.\nGostaria de mais informações!`;
    
    if (sendWhatsAppBtn) {
      sendWhatsAppBtn.href = getWhatsAppLink(message);
      if (WHATSAPP_NUMBER && WHATSAPP_NUMBER.length >= 10) {
        sendWhatsAppBtn.setAttribute("target", "_blank");
        sendWhatsAppBtn.setAttribute("rel", "noopener noreferrer");
      } else {
        sendWhatsAppBtn.removeAttribute("target");
        sendWhatsAppBtn.removeAttribute("rel");
      }
    }
  }

  // Event Listeners dos Controles
  vehicleSelect.addEventListener("change", calculate);
  entryInput.addEventListener("input", calculate);

  termButtons.forEach(btn => {
    btn.addEventListener("click", () => {
      termButtons.forEach(b => b.classList.remove("active"));
      btn.classList.add("active");
      selectedTermMonths = parseInt(btn.dataset.months, 10);
      calculate();
    });
  });

  // Executa o primeiro cálculo
  calculate();
}

/**
 * Inicialização dos Módulos
 */
document.addEventListener("DOMContentLoaded", () => {
  // 1. Renderiza catálogo inicial
  renderVehicles();

  // 2. Inicializa simulador
  initSimulator();

  // 3. Configura abas de categoria
  document.querySelectorAll(".category-btn").forEach(btn => {
    btn.addEventListener("click", () => {
      document.querySelectorAll(".category-btn").forEach(b => b.classList.remove("active"));
      btn.classList.add("active");
      activeCategory = btn.dataset.category;
      renderVehicles();
    });
  });

  // 4. Configura campo de busca
  const searchInput = document.getElementById("search-input");
  if (searchInput) {
    searchInput.addEventListener("input", (e) => {
      searchTerm = e.target.value.trim();
      renderVehicles();
    });
  }

  // 5. Menu Mobile
  const menuToggle = document.getElementById("mobile-menu-toggle");
  const navMenu = document.getElementById("nav-menu");
  if (menuToggle && navMenu) {
    menuToggle.addEventListener("click", () => {
      navMenu.classList.toggle("open");
      const expanded = navMenu.classList.contains("open");
      menuToggle.setAttribute("aria-expanded", expanded);
    });

    // Fecha o menu ao clicar em um link
    navMenu.querySelectorAll(".nav-link").forEach(link => {
      link.addEventListener("click", () => {
        navMenu.classList.remove("open");
        menuToggle.setAttribute("aria-expanded", false);
      });
    });
  }
});
