using Microsoft.AspNetCore.Mvc;
using Tienda.Application.DTOs;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Models;
using Tienda.Web.Models;

namespace Tienda.Web.Controllers
{
    public class PromocionController : Controller
    {
        private readonly IServicePromocion _servicePromocion;
        private readonly IServicePromocionProducto _servicePromocionProducto;
        private readonly IServicePromocionCategoria _servicePromocionCategoria;
        private readonly IServiceTipoPromocion _serviceTipoPromocion;
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly VideoGameContext _contex;

        public PromocionController(
            IServicePromocion servicePromocion,
            IServicePromocionProducto servicePromocionProducto,
            IServicePromocionCategoria servicePromocionCategoria,
            IServiceTipoPromocion serviceTipoPromocion,
            IServiceProducto serviceProducto,
            IServiceCategoria serviceCategoria,
            VideoGameContext contex)
        {
            _servicePromocion = servicePromocion;
            _servicePromocionProducto = servicePromocionProducto;
            _servicePromocionCategoria = servicePromocionCategoria;
            _serviceTipoPromocion = serviceTipoPromocion;
            _serviceProducto = serviceProducto;
            _serviceCategoria = serviceCategoria;
            _contex = contex;
        }

        public async Task<IActionResult> Index()
        {
            var promociones = await _servicePromocion.ListAsync();

            // Primero las activas, luego las próximas y al final las finalizadas
            var ordenadas = promociones
                .OrderBy(p => p.Estado())
                .ThenBy(p => p.FechaFin)
                .ToList();
            return View(ordenadas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var promocion = await _servicePromocion.FindByIdAsync(id);
            if (promocion == null) return NotFound();

            var productos = await _serviceProducto.ListAsync();
            var oferta = OfertaActiva.Desde(promocion);
            var esPorCategoria = promocion.IdTipoPromocion != 1;
            string alcance;
            IEnumerable<ProductoDTO> incluidos;

            if (!esPorCategoria)
            {
                var idsProducto = (await _servicePromocionProducto.ListAsync())
                    .Where(r => r.IdPromocion == id)
                    .Select(r => r.IdProducto)
                    .ToHashSet();
                incluidos = productos.Where(p => idsProducto.Contains(p.IdProducto));
                alcance = idsProducto.Count == 1 ? "Producto seleccionado" : $"{idsProducto.Count} productos seleccionados";
            }
            else
            {
                var categorias = (await _servicePromocionCategoria.ListAsync())
                    .Where(r => r.IdPromocion == id)
                    .ToList();
                var idsCategoria = categorias.Select(r => r.IdCategoria).ToHashSet();
                incluidos = productos.Where(p => idsCategoria.Contains(p.IdCategoria));
                var nombres = categorias
                    .Select(r => r.IdCategoriaNavigation?.Categoria1)
                    .Where(n => !string.IsNullOrWhiteSpace(n));
                alcance = "Categoría: " + string.Join(", ", nombres);
            }

            var modelo = new PromocionDetalleViewModel
            {
                Promocion = promocion,
                Alcance = alcance,
                EsPorCategoria = esPorCategoria,
                Productos = incluidos
                    .OrderBy(p => p.Nombre)
                    .Select(p => new ProductoCardViewModel { Producto = p, Oferta = oferta })
                    .ToList()
            };

            return View("Details", modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarDatosViewBag();
            return View(new PromocionDTO
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(7)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            PromocionDTO dto,
            List<int>? selectedProducto,
            List<int>? selectedCategoria)
        {
            await CargarDatosViewBag();
            // Si el formulario vuelve con errores, conserva lo que el usuario había marcado
            ViewBag.SelectedProductos = selectedProducto ?? new List<int>();
            ViewBag.SelectedCategorias = selectedCategoria ?? new List<int>();

            // Validaciones básicas
            ValidarDescuento(dto);
            if (dto.IdTipoPromocion == 1 && (selectedProducto is null || selectedProducto.Count == 0))
                ModelState.AddModelError("", "Seleccione al menos un producto para la promoción.");
            if (dto.IdTipoPromocion != 1 && (selectedCategoria is null || selectedCategoria.Count == 0))
                ModelState.AddModelError("", "Seleccione al menos una categoría para la promoción.");
            if (dto.FechaInicio < DateTime.Today)
                ModelState.AddModelError(nameof(dto.FechaInicio),
                    "La fecha de inicio no puede ser anterior a hoy");
            if (dto.FechaFin < dto.FechaInicio)
                ModelState.AddModelError(nameof(dto.FechaFin),
                    "La fecha fin no puede ser anterior a la fecha de inicio");

            if (!ModelState.IsValid)
                return View(dto);

            using var tx = await _contex.Database.BeginTransactionAsync();
            try
            {
                // Crear promoción
                var prom = new Promocion
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Descuento = dto.Descuento ?? 0m,
                    FechaInicio = dto.FechaInicio,
                    FechaFin = dto.FechaFin,
                    IdTipoPromocion = dto.IdTipoPromocion
                };
                await _contex.Promocion.AddAsync(prom);
                await _contex.SaveChangesAsync();
                int promoId = prom.IdPromocion;

                // Asociar según tipo
                if (dto.IdTipoPromocion == 1)
                {
                    if (selectedProducto == null || !selectedProducto.Any())
                        throw new InvalidOperationException("Seleccione al menos un producto");
                    foreach (var pid in selectedProducto)
                    {
                        await _contex.PromocionProducto.AddAsync(new PromocionProducto
                        {
                            IdPromocion = promoId,
                            IdProducto = pid
                        });
                    }
                }
                else
                {
                    if (selectedCategoria == null || !selectedCategoria.Any())
                        throw new InvalidOperationException("Seleccione al menos una categoría");
                    foreach (var cid in selectedCategoria)
                    {
                        await _contex.PromocionCategoria.AddAsync(new PromocionCategoria
                        {
                            IdPromocion = promoId,
                            IdCategoria = cid
                        });
                    }
                }

                await _contex.SaveChangesAsync();
                await tx.CommitAsync();

                TempData["SuccessMessage"] = $"Promoción «{prom.Nombre}» creada correctamente.";
                return RedirectToAction(nameof(Crear));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(dto);
            }
        }

        /// <summary>El descuento se guarda como fracción (0.15 = 15 %).</summary>
        private void ValidarDescuento(PromocionDTO dto)
        {
            if (dto.Descuento is null or <= 0m or >= 1m)
                ModelState.AddModelError(nameof(dto.Descuento), "El descuento debe estar entre 1 % y 99 %.");
        }

        private async Task CargarDatosViewBag()
        {
            ViewBag.ListCategorias = await _serviceCategoria.ListAsync();
            ViewBag.Productos = await _serviceProducto.ListAsync();
            ViewBag.TipoPromocion = await _serviceTipoPromocion.ListAsync();
            ViewBag.ListPromocion = await _servicePromocion.ListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            // 1) Obtiene el DTO existente
            var dto = await _servicePromocion.FindByIdAsync(id);
            if (dto == null) return NotFound();

            // 2) Carga los dropdowns y la tabla
            await CargarDatosViewBag();

            // 3) Carga los IDs seleccionados según el tipo
            if (dto.IdTipoPromocion == 1)
            {
                // promociones-producto
                var allRelP = await _servicePromocionProducto.ListAsync();
                ViewBag.SelectedProductos = allRelP
                    .Where(r => r.IdPromocion == id)
                    .Select(r => r.IdProducto)
                    .ToList();
            }
            else
            {
                // promociones-categoría
                var allRelC = await _servicePromocionCategoria.ListAsync();
                ViewBag.SelectedCategorias = allRelC
                    .Where(r => r.IdPromocion == id)
                    .Select(r => r.IdCategoria)
                    .ToList();
            }

            return View(dto);
        }

        // POST: Promocion/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            PromocionDTO dto,
            List<int>? selectedProducto,
            List<int>? selectedCategoria)
        {
            // 1) Recarga dropdowns
            await CargarDatosViewBag();

            // 2) Validaciones: una promoción ya iniciada se puede editar, solo se valida el rango
            ValidarDescuento(dto);
            if (dto.FechaFin < dto.FechaInicio)
                ModelState.AddModelError(nameof(dto.FechaFin),
                    "La fecha fin no puede ser anterior a la fecha de inicio");
            if (!ModelState.IsValid)
            {
                // Vuelve a cargar las listas de IDs seleccionados para el view
                ViewBag.SelectedProductos = selectedProducto ?? new List<int>();
                ViewBag.SelectedCategorias = selectedCategoria ?? new List<int>();
                return View(dto);
            }

            using var tx = await _contex.Database.BeginTransactionAsync();
            try
            {
                // 3) Actualiza la propia entidad Promocion
                var updatedId = await _servicePromocion.UpdateAsync(dto);
                if (updatedId <= 0)
                    throw new InvalidOperationException("Error al actualizar la promoción");

                // 4) Borra relaciones previas
                // 4.1 Producto
                var relsP = await _servicePromocionProducto.ListAsync();
                foreach (var rel in relsP.Where(r => r.IdPromocion == dto.IdPromocion))
                {
                    await _servicePromocionProducto.DeleteAsync(rel);
                }
                // 4.2 Categoría
                var relsC = await _servicePromocionCategoria.ListAsync();
                foreach (var rel in relsC.Where(r => r.IdPromocion == dto.IdPromocion))
                {
                    await _servicePromocionCategoria.DeleteAsync(rel);
                }

                // 5) Re-asocia según el tipo
                if (dto.IdTipoPromocion == 1)
                {
                    if (selectedProducto == null || !selectedProducto.Any())
                        throw new InvalidOperationException("Debe seleccionar al menos un producto");
                    foreach (var pid in selectedProducto)
                    {
                        await _servicePromocionProducto.AddAsync(new PromocionProductoDTO
                        {
                            IdPromocion = dto.IdPromocion,
                            IdProducto = pid
                        });
                    }
                }
                else // tipo 2
                {
                    if (selectedCategoria == null || !selectedCategoria.Any())
                        throw new InvalidOperationException("Debe seleccionar al menos una categoría");
                    foreach (var cid in selectedCategoria)
                    {
                        await _servicePromocionCategoria.AddAsync(new PromocionCategoriaDTO
                        {
                            IdPromocion = dto.IdPromocion,
                            IdCategoria = cid
                        });
                    }
                }

                await tx.CommitAsync();
                TempData["SuccessMessage"] = "Cambios guardados correctamente.";
                return RedirectToAction(nameof(Editar), new { id = dto.IdPromocion });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                ModelState.AddModelError("", $"Error al actualizar: {ex.Message}");

                // Volver a inyectar las selecciones para que no se pierdan
                ViewBag.SelectedProductos = selectedProducto ?? new List<int>();
                ViewBag.SelectedCategorias = selectedCategoria ?? new List<int>();
                return View(dto);
            }
        }
    }

}

