// -- THEME --------------------------------------------------------------
function setTheme(theme, btn) {
    document.querySelectorAll('.pill').forEach(p => p.classList.remove('active'));
    if (btn) btn.classList.add('active');
    document.body.className = theme === 'default' ? '' : `theme-${theme}`;
    try { localStorage.setItem('tb-theme', theme); } catch(e) {}
}

function loadSavedTheme() {
    try {
        var theme = localStorage.getItem('tb-theme') || 'default';
        if (theme !== 'default') {
            document.body.className = `theme-${theme}`;
        }
        // Activate the matching pill on whichever page we're on
        document.querySelectorAll('.pill').forEach(p => {
            p.classList.remove('active');
            var onclick = p.getAttribute('onclick') || '';
            if (onclick.includes("'" + theme + "'")) {
                p.classList.add('active');
            } else if (theme === 'default' && onclick.includes("'default'")) {
                p.classList.add('active');
            }
        });
    } catch(e) {}
}

// ── CART ───────────────────────────────────────────────────────────────────
function toggleCart() {
    document.getElementById('cartSidebar').classList.toggle('open');
    document.getElementById('cartOverlay').classList.toggle('open');
    if (document.getElementById('cartSidebar').classList.contains('open')) {
        loadCart();
    }
}

function loadCart() {
    fetch('/Cart/Summary')
        .then(r => r.json())
        .then(data => {
            renderCartItems(data.items, data.total, data.count);
        })
        .catch(() => {});
}

function renderCartItems(items, total, count) {
    const container = document.getElementById('cartItems');
    const footer = document.getElementById('cartFooter');
    const countEl = document.getElementById('cartCount');

    if (countEl) countEl.textContent = count;

    if (!items || items.length === 0) {
        container.innerHTML = `<div class="empty-cart"><div class="empty-cart-icon">🛍️</div><p>Your bag is empty</p></div>`;
        footer.style.display = 'none';
        return;
    }

    footer.style.display = 'block';
    container.innerHTML = items.map(item => `
        <div class="cart-item">
            <div class="cart-item-img"><span>${item.emoji}</span></div>
            <div class="cart-item-details">
                <div class="cart-item-name">${item.name}</div>
                <div class="cart-item-variant">Size: ${item.size}</div>
                <div class="cart-item-row">
                    <div class="qty-ctrl">
                        <button class="qty-btn" onclick="changeQty(${item.productId}, '${item.size}', -1)">−</button>
                        <span class="qty-num">${item.quantity}</span>
                        <button class="qty-btn" onclick="changeQty(${item.productId}, '${item.size}', 1)">+</button>
                    </div>
                    <span class="cart-item-price">A$${(item.price * item.quantity).toLocaleString()}</span>
                </div>
                <button class="remove-item" onclick="removeFromCart(${item.productId}, '${item.size}')">Remove</button>
            </div>
        </div>
    `).join('');

    document.getElementById('cartTotal').textContent = `A$${total.toLocaleString()}`;
}

function addToCart(productId, size) {
    fetch('/Cart/Add', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `productId=${productId}&size=${encodeURIComponent(size || 'M')}`
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) {
            showToast(data.message);
            const el = document.getElementById('cartCount');
            if (el) el.textContent = data.cartCount;
        } else {
            showToast(data.message);
        }
    })
    .catch(() => showToast('Something went wrong. Please try again.'));
}

function removeFromCart(productId, size) {
    fetch('/Cart/Remove', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `productId=${productId}&size=${encodeURIComponent(size)}`
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) loadCart();
    });
}

function changeQty(productId, size, delta) {
    fetch('/Cart/UpdateQty', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `productId=${productId}&size=${encodeURIComponent(size)}&delta=${delta}`
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) loadCart();
    });
}

// updateCartUI is an alias for loadCart
function updateCartUI() { loadCart(); }
// handleCheckout is defined in _Layout.cshtml to support auth-aware checkout

// ── MODAL ──────────────────────────────────────────────────────────────────
function openModal() {
    document.getElementById('loginModal').classList.add('open');
}
function closeModal() {
    document.getElementById('loginModal').classList.remove('open');
}
function closeModalOutside(e) {
    if (e.target === document.getElementById('loginModal')) closeModal();
}
function switchTab(tab, btn) {
    document.querySelectorAll('.modal-tab').forEach(t => t.classList.remove('active'));
    btn.classList.add('active');
    document.getElementById('loginForm').style.display = tab === 'login' ? '' : 'none';
    document.getElementById('registerForm').style.display = tab === 'register' ? '' : 'none';
}
// handleLogin and handleRegister are defined in _Layout.cshtml

// ── TOAST ──────────────────────────────────────────────────────────────────
function showToast(msg) {
    const t = document.getElementById('toast');
    t.textContent = msg;
    t.classList.add('show');
    setTimeout(() => t.classList.remove('show'), 3000);
}

// ── COLOUR SWATCH ──────────────────────────────────────────────────────────
function selectColor(el) {
    el.closest('.color-swatches').querySelectorAll('.swatch').forEach(s => {
        s.style.borderColor = 'transparent';
        s.classList.remove('selected');
    });
    el.style.borderColor = 'var(--accent)';
    el.classList.add('selected');
}

// ── SCROLL REVEAL ──────────────────────────────────────────────────────────
function observeReveal() {
    const els = document.querySelectorAll('.reveal:not(.visible)');
    if (!els.length) return;
    const io = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting) { e.target.classList.add('visible'); io.unobserve(e.target); }
        });
    }, { threshold: 0.1 });
    els.forEach(el => io.observe(el));
}

// ── NAV SCROLL ─────────────────────────────────────────────────────────────
window.addEventListener('scroll', () => {
    const nav = document.getElementById('mainNav');
    if (nav) nav.classList.toggle('scrolled', window.scrollY > 60);
});

// -- INIT ───────────────────────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    loadSavedTheme();

    // Load cart count on every page
    fetch('/Cart/Summary')
        .then(r => r.json())
        .then(data => {
            const el = document.getElementById('cartCount');
            if (el) el.textContent = data.count;
        })
        .catch(() => {});

    observeReveal();
    setTimeout(observeReveal, 500);
});
