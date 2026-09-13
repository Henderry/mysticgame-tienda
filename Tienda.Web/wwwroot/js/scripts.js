/* ==========================================================================
   MysticGame — comportamiento global
   - Carrito guardado en el navegador (localStorage)
   - Notificaciones (toasts) de Bootstrap
   - Barra superior con borde al desplazar y animaciones de aparición
   ========================================================================== */
const MysticCarrito = (() => {
    const CLAVE = 'mysticgame.carrito';
    const formato = new Intl.NumberFormat('es-CR', { maximumFractionDigits: 0 });
    const colones = monto => '₡' + formato.format(Math.round(monto)).replace(/ | |\./g, ' ');

    const leer = () => {
        try { return JSON.parse(localStorage.getItem(CLAVE)) ?? []; } catch { return []; }
    };
    const guardar = items => {
        try { localStorage.setItem(CLAVE, JSON.stringify(items)); } catch { /* almacenamiento lleno o bloqueado */ }
        render();
    };

    /** Crea una miniatura pequeña (JPEG ~5 KB) para no guardar la imagen original en localStorage */
    const miniatura = img => {
        try {
            const lienzo = document.createElement('canvas');
            lienzo.width = lienzo.height = 96;
            const ctx = lienzo.getContext('2d');
            ctx.fillStyle = '#fff';
            ctx.fillRect(0, 0, 96, 96);
            const escala = Math.min(96 / img.naturalWidth, 96 / img.naturalHeight);
            const w = img.naturalWidth * escala, h = img.naturalHeight * escala;
            ctx.drawImage(img, (96 - w) / 2, (96 - h) / 2, w, h);
            return lienzo.toDataURL('image/jpeg', 0.75);
        } catch { return null; }
    };

    const agregar = (producto, cantidad = 1) => {
        const items = leer();
        const existente = items.find(i => i.id === String(producto.id));
        if (existente) existente.cantidad += cantidad;
        else items.push({ ...producto, id: String(producto.id), cantidad });
        guardar(items);
        notificar(`«${producto.nombre}» se agregó al carrito`);
    };

    const cambiar = (id, delta) => {
        const items = leer();
        const item = items.find(i => i.id === id);
        if (!item) return;
        item.cantidad += delta;
        guardar(item.cantidad <= 0 ? items.filter(i => i.id !== id) : items);
    };

    const quitar = id => guardar(leer().filter(i => i.id !== id));
    const vaciar = () => guardar([]);

    const escapar = texto => String(texto ?? '').replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));

    function render() {
        const items = leer();
        const unidades = items.reduce((t, i) => t + i.cantidad, 0);
        const total = items.reduce((t, i) => t + i.precio * i.cantidad, 0);

        document.querySelectorAll('[data-carrito-contador]').forEach(el => {
            el.textContent = unidades;
            el.hidden = unidades === 0;
        });

        const lista = document.querySelector('[data-carrito-lista]');
        if (!lista) return;
        lista.innerHTML = items.map(i => `
            <div class="carrito-item">
                ${i.imagen ? `<img src="${i.imagen}" alt="">` : '<span class="sin-foto"><i class="bi bi-controller"></i></span>'}
                <div>
                    <h3>${escapar(i.nombre)}</h3>
                    <div class="precio-linea">${colones(i.precio * i.cantidad)}</div>
                    <div class="cantidad mt-2">
                        <button type="button" data-carrito-cambiar="${i.id}" data-delta="-1" aria-label="Menos">−</button>
                        <span>${i.cantidad}</span>
                        <button type="button" data-carrito-cambiar="${i.id}" data-delta="1" aria-label="Más">+</button>
                    </div>
                </div>
                <button type="button" class="carrito-quitar" data-carrito-quitar="${i.id}" aria-label="Quitar"><i class="bi bi-trash3"></i></button>
            </div>`).join('');

        document.querySelector('[data-carrito-vacio]').hidden = items.length > 0;
        document.querySelector('[data-carrito-resumen]').hidden = items.length === 0;
        document.querySelector('[data-carrito-total]').textContent = colones(total);
    }

    function notificar(mensaje, tipo = 'exito') {
        const contenedor = document.getElementById('notificaciones');
        if (!contenedor || !window.bootstrap) return;
        const toast = document.createElement('div');
        toast.className = `toast aviso aviso-${tipo}`;
        toast.setAttribute('role', 'status');
        toast.innerHTML = `<div class="toast-body"><i class="bi ${tipo === 'exito' ? 'bi-check-circle-fill' : 'bi-info-circle-fill'}"></i><span>${escapar(mensaje)}</span></div>`;
        contenedor.appendChild(toast);
        toast.addEventListener('hidden.bs.toast', () => toast.remove());
        bootstrap.Toast.getOrCreateInstance(toast, { delay: 3000 }).show();
    }

    document.addEventListener('click', e => {
        const agregarBtn = e.target.closest('[data-agregar-carrito]');
        if (agregarBtn) {
            const tarjeta = agregarBtn.closest('.producto-card');
            const img = tarjeta?.querySelector('.producto-imagen img');
            agregar({
                id: agregarBtn.dataset.id,
                nombre: agregarBtn.dataset.nombre,
                precio: Number(agregarBtn.dataset.precio),
                imagen: img && img.complete ? miniatura(img) : null
            });
            agregarBtn.classList.add('agregado');
            agregarBtn.innerHTML = '<i class="bi bi-check2"></i>';
            setTimeout(() => { agregarBtn.classList.remove('agregado'); agregarBtn.innerHTML = '<i class="bi bi-bag-plus"></i>'; }, 1400);
            return;
        }
        const cambiarBtn = e.target.closest('[data-carrito-cambiar]');
        if (cambiarBtn) return cambiar(cambiarBtn.dataset.carritoCambiar, Number(cambiarBtn.dataset.delta));
        const quitarBtn = e.target.closest('[data-carrito-quitar]');
        if (quitarBtn) return quitar(quitarBtn.dataset.carritoQuitar);
        if (e.target.closest('[data-carrito-vaciar]')) return vaciar();
        if (e.target.closest('[data-carrito-pagar]')) {
            vaciar();
            notificar('¡Pedido recibido! (demostración: no se procesan pagos reales)');
        }
    });

    // Mantener el carrito sincronizado entre pestañas
    window.addEventListener('storage', e => { if (e.key === CLAVE) render(); });

    return { agregar, miniatura, notificar, render };
})();

document.addEventListener('DOMContentLoaded', () => {
    MysticCarrito.render();

    // Notificaciones que vienen del servidor (TempData)
    document.querySelectorAll('.toast[data-mostrar]').forEach(t => bootstrap.Toast.getOrCreateInstance(t).show());

    // Borde de la barra superior al desplazar
    const barra = document.getElementById('barraSuperior');
    const alDesplazar = () => barra?.classList.toggle('con-borde', window.scrollY > 10);
    window.addEventListener('scroll', alDesplazar, { passive: true });
    alDesplazar();

    // Aparición suave de secciones
    const elementos = document.querySelectorAll('.revelar');
    if ('IntersectionObserver' in window) {
        const observador = new IntersectionObserver(entradas => {
            entradas.forEach(entrada => {
                if (entrada.isIntersecting) {
                    entrada.target.classList.add('visible');
                    observador.unobserve(entrada.target);
                }
            });
        }, { rootMargin: '0px 0px -8% 0px' });
        elementos.forEach(el => observador.observe(el));
    } else {
        elementos.forEach(el => el.classList.add('visible'));
    }
});
