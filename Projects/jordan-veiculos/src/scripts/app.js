// SiteFactory - Aplicação Interativa do Cliente
// Cliente: Jordan Veículos (automotivo)

const vehiclesData = [
  {
    id: 1,
    title: 'BMW 320i M Sport 2.0 Turbo',
    category: 'sedan',
    year: '2024',
    km: '14.200 km',
    gear: 'Automático',
    price: 319900,
    tag: 'Único Dono',
    color: '#2563EB'
  },
  {
    id: 2,
    title: 'Jeep Compass Limited T270',
    category: 'suv',
    year: '2023',
    km: '28.500 km',
    gear: 'Automático',
    price: 169900,
    tag: 'Garantia de Fábrica',
    color: '#059669'
  },
  {
    id: 3,
    title: 'Toyota Hilux SRX 2.8 4x4 Diesel',
    category: 'pickup',
    year: '2023',
    km: '34.000 km',
    gear: 'Automático',
    price: 289900,
    tag: 'Laudo 100% Aprovado',
    color: '#DC2626'
  },
  {
    id: 4,
    title: 'Porsche Macan 2.0 Turbo PDK',
    category: 'suv',
    year: '2022',
    km: '21.000 km',
    gear: 'Automático',
    price: 459000,
    tag: 'Blindado Nível III-A',
    color: '#7C3AED'
  },
  {
    id: 5,
    title: 'Honda Civic Touring 1.5 Turbo',
    category: 'sedan',
    year: '2021',
    km: '42.000 km',
    gear: 'Automático',
    price: 142900,
    tag: 'IPVA 2026 Pago',
    color: '#D97706'
  },
  {
    id: 6,
    title: 'Volkswagen Golf GTI 2.0 TSI',
    category: 'hatch',
    year: '2020',
    km: '51.000 km',
    gear: 'DSG Automático',
    price: 185000,
    tag: 'Revisões na Concessionária',
    color: '#E11D48'
  }
];

const clientName = 'Jordan Veículos';
let activeCategory = 'todos';
let searchQuery = '';
let selectedMonths = 48;

function formatCurrency(val) {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL', maximumFractionDigits: 0 }).format(val);
}

function renderVehicles() {
  const grid = document.getElementById('vehicle-grid');
  if (!grid) return;

  const filtered = vehiclesData.filter(v => {
    const matchesCategory = activeCategory === 'todos' || v.category === activeCategory;
    const matchesSearch = v.title.toLowerCase().includes(searchQuery.toLowerCase()) || 
                          v.year.includes(searchQuery);
    return matchesCategory && matchesSearch;
  });

  if (filtered.length === 0) {
    grid.innerHTML = `
      <div style="grid-column: 1 / -1; text-align: center; padding: 48px; color: var(--text-secondary);">
        <h3>Nenhum veículo encontrado</h3>
        <p>Tente ajustar os filtros ou termo de busca.</p>
      </div>`;
    return;
  }

  grid.innerHTML = filtered.map(v => {
    const estimatedEntry = v.price * 0.3;
    const estimatedInstallment = (v.price - estimatedEntry) * 1.35 / 48;
    const encodedMsg = encodeURIComponent(`Olá! Tenho interesse no veículo ${v.title} (${formatCurrency(v.price)}) anunciado no site da ${clientName}.`);

    return `
      <article class="vehicle-card" data-id="${v.id}">
        <div class="card-img-wrapper">
          <span class="card-tag">${v.tag}</span>
          <svg viewBox="0 0 240 120" width="200" height="100" fill="none">
            <rect x="10" y="10" width="220" height="100" rx="12" fill="rgba(255,255,255,0.03)" />
            <path d="M40 75 C55 75 65 70 80 55 L115 35 C130 30 160 30 175 40 L195 55 C210 60 220 70 220 75 L225 85 C225 90 220 92 215 92 L40 92 Z" fill="${v.color}" opacity="0.9" />
            <circle cx="75" cy="92" r="12" fill="#0B0F19" stroke="#94A3B8" stroke-width="2" />
            <circle cx="180" cy="92" r="12" fill="#0B0F19" stroke="#94A3B8" stroke-width="2" />
          </svg>
        </div>
        <div class="card-body">
          <h3>${v.title}</h3>
          <div class="vehicle-tags">
            <span>📅 ${v.year}</span>
            <span>🛣️ ${v.km}</span>
            <span>⚙️ ${v.gear}</span>
          </div>
          <div class="vehicle-pricing">
            <div>
              <div class="price-main">${formatCurrency(v.price)}</div>
              <div class="price-installment">Entrada ${formatCurrency(estimatedEntry)} + 48x de ${formatCurrency(estimatedInstallment)}</div>
            </div>
          </div>
          <a href="https://wa.me/5511999999999?text=${encodedMsg}" target="_blank" rel="noopener noreferrer" class="btn btn-primary btn-block">
            Tenho Interesse
          </a>
        </div>
      </article>
    `;
  }).join('');
}

function initFilters() {
  const filterBtns = document.querySelectorAll('.filter-btn');
  filterBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      filterBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      activeCategory = btn.getAttribute('data-category');
      renderVehicles();
    });
  });

  const searchInput = document.getElementById('search-input');
  if (searchInput) {
    searchInput.addEventListener('input', (e) => {
      searchQuery = e.target.value.trim();
      renderVehicles();
    });
  }
}

function initSimulator() {
  const carSelect = document.getElementById('sim-car-select');
  const entryRange = document.getElementById('sim-entry-range');
  const entryDisplay = document.getElementById('entry-display');
  const installmentResult = document.getElementById('installment-result');
  const instBtns = document.querySelectorAll('.inst-btn');
  const btnSend = document.getElementById('btn-send-simulation');

  if (!carSelect || !entryRange) return;

  // Popula opções de veículos
  carSelect.innerHTML = vehiclesData.map(v => 
    `<option value="${v.price}" data-title="${v.title}">${v.title} - ${formatCurrency(v.price)}</option>`
  ).join('');

  function updateCalculation() {
    const carPrice = parseFloat(carSelect.value) || 200000;
    const maxEntry = Math.round(carPrice * 0.85);
    entryRange.max = maxEntry;
    
    let entryVal = parseFloat(entryRange.value);
    if (entryVal > maxEntry) {
      entryVal = maxEntry;
      entryRange.value = maxEntry;
    }

    entryDisplay.textContent = formatCurrency(entryVal);

    const financedAmount = Math.max(0, carPrice - entryVal);
    // Coeficiente de juros simples aproximado de mercado (1.2% ao mês)
    const monthlyRate = 0.0125;
    const factor = (monthlyRate * Math.pow(1 + monthlyRate, selectedMonths)) / (Math.pow(1 + monthlyRate, selectedMonths) - 1);
    const monthlyInstallment = financedAmount * factor;

    installmentResult.textContent = `${formatCurrency(monthlyInstallment)} / mês`;
  }

  carSelect.addEventListener('change', updateCalculation);
  entryRange.addEventListener('input', updateCalculation);

  instBtns.forEach(btn => {
    btn.addEventListener('click', () => {
      instBtns.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      selectedMonths = parseInt(btn.getAttribute('data-months'), 10);
      updateCalculation();
    });
  });

  if (btnSend) {
    btnSend.addEventListener('click', () => {
      const selectedOption = carSelect.options[carSelect.selectedIndex];
      const carName = selectedOption.getAttribute('data-title');
      const entryVal = entryDisplay.textContent;
      const installmentVal = installmentResult.textContent;
      
      const text = encodeURIComponent(`Olá! Fiz uma simulação no site da ${clientName} para o veículo ${carName}:\n- Entrada: ${entryVal}\n- Prazo: ${selectedMonths}x\n- Parcela estimada: ${installmentVal}\nGostaria de consultar as condições de aprovação.`);
      window.open(`https://wa.me/5511999999999?text=${text}`, '_blank');
    });
  }

  updateCalculation();
}

document.addEventListener('DOMContentLoaded', () => {
  renderVehicles();
  initFilters();
  initSimulator();
});
