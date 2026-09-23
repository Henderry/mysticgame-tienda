/* ==========================================================================
   Administración: vista previa de imágenes, filtros de tablas y
   comportamiento del formulario de promociones.
   ========================================================================== */
document.addEventListener('DOMContentLoaded', () => {

    // ------------------------------------------------------------ Imágenes (arrastrar o elegir)
    const entradaImagenes = document.getElementById('imageUpload');
    const vistaPrevia = document.getElementById('vistaPrevia');
    const zona = document.querySelector('.zona-carga');

    const mostrarVistaPrevia = () => {
        vistaPrevia.innerHTML = '';
        [...entradaImagenes.files].forEach((archivo, i) => {
            const figura = document.createElement('figure');
            figura.className = 'imagen-admin';
            const img = document.createElement('img');
            img.alt = archivo.name;
            img.src = URL.createObjectURL(archivo);
            figura.appendChild(img);
            if (i === 0 && !document.querySelector('.imagen-principal')) {
                figura.insertAdjacentHTML('beforeend', '<span class="chip imagen-principal"><i class="bi bi-star-fill"></i>Principal</span>');
            }
            if (archivo.size > 5 * 1024 * 1024) {
                figura.classList.add('con-error');
                figura.insertAdjacentHTML('beforeend', '<span class="chip chip-oferta imagen-error">Más de 5 MB</span>');
            }
            vistaPrevia.appendChild(figura);
        });
    };

    if (entradaImagenes && vistaPrevia) {
        entradaImagenes.addEventListener('change', mostrarVistaPrevia);
        ['dragenter', 'dragover'].forEach(ev => zona.addEventListener(ev, e => { e.preventDefault(); zona.classList.add('arrastrando'); }));
        ['dragleave', 'drop'].forEach(ev => zona.addEventListener(ev, e => { e.preventDefault(); zona.classList.remove('arrastrando'); }));
        zona.addEventListener('drop', e => {
            const imagenes = [...e.dataTransfer.files].filter(f => f.type.startsWith('image/'));
            if (!imagenes.length) return;
            const transferencia = new DataTransfer();
            imagenes.forEach(f => transferencia.items.add(f));
            entradaImagenes.files = transferencia.files;
            mostrarVistaPrevia();
        });
        entradaImagenes.form.addEventListener('reset', () => { vistaPrevia.innerHTML = ''; });
    }

    // ------------------------------------------------------------ Confirmaciones
    document.querySelectorAll('form[data-confirmar]').forEach(form => {
        form.addEventListener('submit', e => { if (!confirm(form.dataset.confirmar)) e.preventDefault(); });
    });

    // ------------------------------------------------------------ Filtro de tablas
    document.querySelectorAll('[data-filtrar-tabla]').forEach(entrada => {
        const filas = document.querySelectorAll(`${entrada.dataset.filtrarTabla} tbody tr`);
        entrada.addEventListener('input', () => {
            const texto = entrada.value.trim().toLowerCase();
            filas.forEach(fila => { fila.hidden = texto && !fila.textContent.toLowerCase().includes(texto); });
        });
    });

    // ------------------------------------------------------------ Formulario de promociones
    const formPromo = document.getElementById('formPromocion');
    if (!formPromo) return;

    const porcentaje = document.getElementById('descuentoPorcentaje');
    const descuento = document.getElementById('descuento');
    const secciones = formPromo.querySelectorAll('[data-seleccion-tipo]');
    const error = formPromo.querySelector('[data-error-promocion]');
    const tipoActual = () => formPromo.querySelector('input[name="IdTipoPromocion"]:checked')?.value ?? '';

    const sincronizarDescuento = () => {
        const valor = Number(porcentaje.value);
        descuento.value = valor > 0 ? (valor / 100).toFixed(2) : '';
    };

    const actualizarContadores = () => {
        secciones.forEach(seccion => {
            const marcados = seccion.querySelectorAll('input[type="checkbox"]:checked').length;
            const contador = seccion.querySelector('[data-contador-seleccion]');
            const palabra = seccion.dataset.seleccionTipo === '1' ? 'seleccionado' : 'seleccionada';
            contador.textContent = `${marcados} ${palabra}${marcados === 1 ? '' : 's'}`;
        });
    };

    const mostrarSeccion = () => {
        const tipo = tipoActual();
        secciones.forEach(s => { s.hidden = s.dataset.seleccionTipo !== tipo; });
    };

    formPromo.querySelectorAll('input[name="IdTipoPromocion"]').forEach(r => r.addEventListener('change', mostrarSeccion));
    formPromo.addEventListener('change', e => { if (e.target.type === 'checkbox') actualizarContadores(); });
    porcentaje.addEventListener('input', sincronizarDescuento);

    const buscador = formPromo.querySelector('[data-buscar-opciones]');
    buscador?.addEventListener('input', () => {
        const texto = buscador.value.trim().toLowerCase();
        formPromo.querySelectorAll('.lista-opciones .opcion').forEach(o => { o.hidden = texto && !o.textContent.toLowerCase().includes(texto); });
    });

    formPromo.addEventListener('reset', () => setTimeout(() => { mostrarSeccion(); actualizarContadores(); sincronizarDescuento(); }));

    formPromo.addEventListener('submit', e => {
        sincronizarDescuento();
        const tipo = tipoActual();
        const inicio = formPromo.querySelector('[name="FechaInicio"]').value;
        const fin = formPromo.querySelector('[name="FechaFin"]').value;
        let mensaje = '';
        if (!tipo) mensaje = 'Elige si la promoción aplica a productos o a categorías.';
        else if (tipo === '1' && !formPromo.querySelector('input[name="selectedProducto"]:checked')) mensaje = 'Selecciona al menos un producto.';
        else if (tipo !== '1' && !formPromo.querySelector('input[name="selectedCategoria"]:checked')) mensaje = 'Selecciona al menos una categoría.';
        else if (inicio && fin && fin < inicio) mensaje = 'La fecha de fin no puede ser anterior a la de inicio.';

        error.textContent = mensaje;
        error.hidden = !mensaje;
        if (mensaje) {
            e.preventDefault();
            error.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    });

    mostrarSeccion();
    actualizarContadores();
    sincronizarDescuento();
});
