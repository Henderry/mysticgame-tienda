using Microsoft.AspNetCore.Mvc;
using Tienda.Application.DTOs;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Models;

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
            return View(promociones);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _servicePromocion.FindByIdAsync(id);
            if (dto == null) return NotFound();

            if (dto.IdTipoPromocion == 1)
            {
                // Producto
                var relsP = await _servicePromocionProducto.ListAsync();
                var itemP = relsP
                    .Where(r => r.IdPromocion == id)
                    .Select(r => new {
                        Producto = r.IdProductoNavigation,
                        Imagenes = r.IdProductoNavigation?.ImagenProducto?.ToList()
                    })
                    .FirstOrDefault();
                if (itemP == null) return NotFound();

                ViewBag.Promocion = dto;
                ViewBag.Producto = itemP.Producto;
                ViewBag.Imagenes = itemP.Imagenes;
                return View("DetailsProducto");
            }
            else
            {
                // Categoría
                var relsC = await _servicePromocionCategoria.ListAsync();
                var cat = relsC
                    .Where(r => r.IdPromocion == id)
                    .Select(r => r.IdCategoriaNavigation)
                    .FirstOrDefault();
                if (cat == null) return NotFound();

                var lista = cat.Producto?
                    .Select(p => new {
                        Producto = p,
                        ImagenPrincipal = p.ImagenProducto?.FirstOrDefault(i => i.Principal)
                                         ?? p.ImagenProducto?.FirstOrDefault()
                    })
                    .ToList();
                ViewBag.Promocion = dto;
                ViewBag.Categoria = cat;
                ViewBag.Productos = lista;
                return View("DetailsCategoria");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarDatosViewBag();
            return View(new PromocionDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            PromocionDTO dto,
            List<int>? selectedProducto,
            List<int>? selectedCategoria)
        {
            await CargarDatosViewBag();

            // Validaciones básicas
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

                TempData["SuccessMessage"] = "Promoción creada exitosamente!";
                return RedirectToAction(nameof(Crear));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(dto);
            }
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

            // 2) Validaciones de fechas
            if (dto.FechaInicio < DateTime.Today)
                ModelState.AddModelError(nameof(dto.FechaInicio),
                    "La fecha de inicio no puede ser anterior a hoy");
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
                TempData["SuccessMessage"] = "Promoción actualizada correctamente!";
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

