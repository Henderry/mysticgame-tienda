/* ==========================================================================
   Catálogo de productos: búsqueda, filtro por categoría, solo ofertas y orden.
   Todo ocurre en el navegador sobre las tarjetas ya cargadas.
   ========================================================================== */
document.addEventListener('DOMContentLoaded', () => {
    const grid = document.getElementById('gridProductos');
    if (!grid) return;

    const tarjetas = [...grid.querySelectorAll('.producto-card')];
    const buscar = document.getElementById('buscarProducto');
    const soloOfertas = document.getElementById('soloOfertas');
    const ordenar = document.getElementById('ordenarProductos');
    const sinResultados = document.getElementById('sinResultados');
    const botonesCategoria = document.querySelectorAll('[data-filtro-categoria]');

    const params = new URLSearchParams(location.search);
    const estado = {
        texto: params.get('q') ?? '',
        categoria: params.get('categoria') ?? '',
        ofertas: params.get('ofertas') === '1',
        orden: params.get('orden') ?? 'nombre'
    };

    const normalizar = t => t.toLowerCase().normalize('NFD').replace(/[̀-ͯ]/g, '');

    function aplicar() {
        const texto = normalizar(estado.texto.trim());
        let visibles = 0;

        tarjetas.forEach(t => {
            const coincide =
                (!texto || normalizar(t.dataset.nombre).includes(texto)) &&
                (!estado.categoria || t.dataset.categoria === estado.categoria) &&
                (!estado.ofertas || t.dataset.oferta === '1');
            t.hidden = !coincide;
            if (coincide) visibles++;
        });

        const ordenadas = [...tarjetas].sort((a, b) => {
            if (estado.orden === 'precio-asc') return a.dataset.precio - b.dataset.precio;
            if (estado.orden === 'precio-desc') return b.dataset.precio - a.dataset.precio;
            return a.dataset.nombre.localeCompare(b.dataset.nombre, 'es');
        });
        ordenadas.forEach(t => grid.appendChild(t));

        sinResultados.hidden = visibles > 0;
        botonesCategoria.forEach(b => b.classList.toggle('activo', b.dataset.filtroCategoria === estado.categoria));

        // Guardar los filtros en la URL para poder compartirlos
        const url = new URL(location);
        const valores = { q: estado.texto, categoria: estado.categoria, ofertas: estado.ofertas ? '1' : '', orden: estado.orden === 'nombre' ? '' : estado.orden };
        Object.entries(valores).forEach(([k, v]) => v ? url.searchParams.set(k, v) : url.searchParams.delete(k));
        history.replaceState(null, '', url);
    }

    buscar.value = estado.texto;
    soloOfertas.checked = estado.ofertas;
    ordenar.value = estado.orden;

    let espera;
    buscar.addEventListener('input', () => {
        clearTimeout(espera);
        espera = setTimeout(() => { estado.texto = buscar.value; aplicar(); }, 120);
    });
    soloOfertas.addEventListener('change', () => { estado.ofertas = soloOfertas.checked; aplicar(); });
    ordenar.addEventListener('change', () => { estado.orden = ordenar.value; aplicar(); });
    botonesCategoria.forEach(b => b.addEventListener('click', () => { estado.categoria = b.dataset.filtroCategoria; aplicar(); }));

    aplicar();
});
