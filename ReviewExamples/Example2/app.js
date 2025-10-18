/* ===== Utilities ===== */
const $ = (sel, ctx = document) => ctx.querySelector(sel);
const $$ = (sel, ctx = document) => Array.from(ctx.querySelectorAll(sel));

/* ===== Theme toggle with localStorage ===== */
const root = document.documentElement;
const themeToggle = $('#theme-toggle');

function setTheme(t) {
  document.documentElement.setAttribute('data-theme', t);
  localStorage.setItem('theme', t);
  themeToggle?.setAttribute('aria-pressed', String(t === 'dark'));
}
setTheme(localStorage.getItem('theme') || (matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'));
themeToggle?.addEventListener('click', () => {
  const next = document.documentElement.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
  setTheme(next);
});

/* ===== Tabs (ARIA) ===== */
const tabs = $$('.tab');
const panels = $$('.tabpanel');
tabs.forEach(tab =>
  tab.addEventListener('click', () => {
    tabs.forEach(t => { t.classList.remove('is-active'); t.setAttribute('aria-selected', 'false'); });
    panels.forEach(p => p.classList.remove('is-active'));
    tab.classList.add('is-active');
    tab.setAttribute('aria-selected', 'true');
    const panel = $('#' + tab.getAttribute('aria-controls'));
    panel?.classList.add('is-active');
    panel?.focus();
  })
);

/* ===== Dialog helpers ===== */
function openDialog(sel) { const d = $(sel); if (d && typeof d.showModal === 'function') d.showModal(); }
function closeDialog(el) {
  const d = el.closest('dialog'); if (d && typeof d.close === 'function') d.close();
}
document.addEventListener('click', e => {
  const t = e.target;
  if (t.matches('[data-open-dialog]')) { e.preventDefault(); openDialog(t.getAttribute('data-open-dialog')); }
  if (t.matches('[data-close-dialog]')) { e.preventDefault(); closeDialog(t); }
});

/* ===== Gallery data & render via <template> ===== */
const items = [
  { title: 'Button Kit', tag: 'ui', text: 'Button states, sizes, and variants.', img: 'https://picsum.photos/400/225' },
  { title: 'Card Layout', tag: 'ui', text: 'Responsive cards with Grid.', img: 'https://picsum.photos/seed/card/400/225' },
  { title: 'Sparkline', tag: 'data', text: 'Canvas-based sparkline chart.', img: 'https://picsum.photos/seed/spark/400/225' },
  { title: 'CSV Table', tag: 'data', text: 'Sortable, filterable table.', img: 'https://picsum.photos/seed/table/400/225' },
  { title: 'Video Player', tag: 'media', text: 'HTML5 video with captions.', img: 'https://picsum.photos/seed/video/400/225' },
  { title: 'Audio Widget', tag: 'media', text: 'Audio element with controls.', img: 'https://picsum.photos/seed/audio/400/225' },
];

const grid = $('#gallery-grid');
const tpl = $('#card-tpl');

function renderGallery(list) {
  grid.innerHTML = '';
  if (!list.length) {
    grid.innerHTML = `<p style="grid-column: 1/-1; opacity:.8">${grid.dataset.emptyMsg}</p>`;
    return;
  }
  list.forEach(({ title, tag, text, img }) => {
    const node = tpl.content.cloneNode(true);
    const card = $('.card', node);
    card.dataset.tag = tag;
    $('.card__title', node).textContent = title;
    $('.badge', node).textContent = tag.toUpperCase();
    $('.card__text', node).textContent = text;
    const image = $('img', node);
    image.src = img;
    image.alt = `${title} preview`;
    grid.appendChild(node);
  });
}
renderGallery(items);

/* Filter + Shuffle */
$('#filter').addEventListener('change', e => {
  const val = e.target.value;
  renderGallery(val === 'all' ? items : items.filter(i => i.tag === val));
});
$('#shuffle').addEventListener('click', () => {
  const copy = [...$$('.card', grid)];
  copy.sort(() => Math.random() - 0.5).forEach(node => grid.appendChild(node));
});

/* Preview dialog: event delegation */
grid.addEventListener('click', e => {
  const btn = e.target.closest('[data-open-dialog="#preview-dialog"]');
  if (!btn) return;
  const card = e.target.closest('.card');
  const title = $('.card__title', card).textContent;
  const img = $('.card__media img', card).src;
  $('#preview-content').innerHTML = `
    <figure style="display:grid; gap:.5rem">
      <img src="${img}" alt="${title} large preview" />
      <figcaption>${title}</figcaption>
    </figure>
  `;
  openDialog('#preview-dialog');
});

/* ===== Table data & filter ===== */
const rowsData = [
  { item: 'Alpha', cat: 'UI', score: 87, prog: 0.8 },
  { item: 'Beta', cat: 'Data', score: 72, prog: 0.45 },
  { item: 'Gamma', cat: 'Media', score: 90, prog: 0.9 },
  { item: 'Delta', cat: 'UI', score: 66, prog: 0.35 },
  { item: 'Epsilon', cat: 'Data', score: 78, prog: 0.6 },
];

function renderRows(list) {
  $('#data-rows').innerHTML = list.map(r => `
    <tr>
      <td>${r.item}</td>
      <td>${r.cat}</td>
      <td>${r.score}</td>
      <td><progress value="${r.prog}" max="1">${Math.round(r.prog*100)}%</progress></td>
    </tr>
  `).join('');
}
renderRows(rowsData);

$('#table-filter').addEventListener('input', e => {
  const q = e.target.value.toLowerCase().trim();
  const filtered = rowsData.filter(r => Object.values(r).some(v => String(v).toLowerCase().includes(q)));
  renderRows(filtered);
});

/* ===== Form validation + output ===== */
const form = $('#contact-form');
const out = $('#form-output');
form.addEventListener('submit', e => {
  e.preventDefault();
  if (!form.checkValidity()) {
    out.value = 'Please fill all required fields correctly.';
    out.style.color = 'tomato';
    return;
  }
  const data = Object.fromEntries(new FormData(form).entries());
  out.value = `Thanks, ${data.name}! We will contact you at ${data.email} about ${data.topic}.`;
  out.style.color = 'inherit';
  form.reset();
});

/* ===== Canvas sparkline ===== */
(function drawSpark() {
  const c = $('#spark');
  const ctx = c.getContext('2d');
  const W = c.width, H = c.height;
  ctx.clearRect(0,0,W,H);
  const N = 50;
  const pts = Array.from({length:N}, (_,i)=>({
    x: (i/(N-1))*W,
    y: H*0.5 + Math.sin(i/3)*20 + (Math.random()*20-10)
  }));
  ctx.lineWidth = 2;
  // gradient stroke
  const g = ctx.createLinearGradient(0,0,W,0);
  g.addColorStop(0, '#7c3aed'); g.addColorStop(1, '#06b6d4');
  ctx.strokeStyle = g;
  ctx.beginPath();
  pts.forEach((p,i)=> i? ctx.lineTo(p.x,p.y) : ctx.moveTo(p.x,p.y));
  ctx.stroke();

  // fill fade
  const gf = ctx.createLinearGradient(0,0,0,H);
  gf.addColorStop(0, 'rgba(124,58,237,0.25)');
  gf.addColorStop(1, 'rgba(124,58,237,0.0)');
  ctx.fillStyle = gf;
  ctx.lineTo(W,H); ctx.lineTo(0,H); ctx.closePath(); ctx.fill();
})();

/* ===== Small niceties ===== */
$('#year').textContent = new Date().getFullYear();
