using Microsoft.AspNetCore.Mvc;
using Tienda.Application.DTOs;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Models;
using Tienda.Web.Models;
using Tienda.Web.Services;

namespace Tienda.Web.Controllers
{
    public class ProductoController : Controller
    {
        // Usuario de ejemplo para las reseñas (todavía no hay inicio de sesión)
        private const int UsuarioDemoId = 2;

        private readonly IServiceEtiqueta _serviceEtiqueta;
        private readonly IServiceEtiquetaProducto _serviceProductoEtiqueta;
        private readonly IServiceResena _serviceResena;
        private readonly IServiceProducto _serviceProducto;
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IServiceImagenProducto _serviceImagenProducto;
        private readonly ICatalogoOfertas _catalogoOfertas;
        private readonly VideoGameContext _context;

        public ProductoController(
            IServiceEtiqueta serviceEtiqueta,
            IServiceEtiquetaProducto serviceProductoEtiqueta,
            IServiceResena serviceResena,
            IServiceProducto serviceProducto,
            IServiceImagenProducto serviceImagenProducto,
            IServiceCategoria serviceCategoria,
            ICatalogoOfertas catalogoOfertas,
            VideoGameContext context)
        {
            _serviceEtiqueta = serviceEtiqueta;
            _serviceProducto = serviceProducto;
            _serviceProductoEtiqueta = serviceProductoEtiqueta;
            _serviceResena = serviceResena;
            _serviceImagenProducto = serviceImagenProducto;
            _serviceCategoria = serviceCategoria;
            _catalogoOfertas = catalogoOfertas;
            _context = context;
        }

        // ---------------------------------------------------------------- Catálogo

        public async Task<IActionResult> Index()
        {
            var productos = await _serviceProducto.ListAsync();
            var ofertas = await _catalogoOfertas.ObtenerOfertasAsync(productos);

            ViewBag.Categorias = (await _serviceCategoria.ListAsync())
                .OrderBy(c => c.Categoria1)
                .ToList();

            return View(ProductoCardViewModel.Crear(productos.OrderBy(p => p.Nombre), ofertas));
        }

        public async Task<IActionResult> Details(int id)
        {
            var producto = await _serviceProducto.FindByIdAsync(id);
            if (producto == null) return NotFound();

            await CargarDetalleAsync(producto);
            return View(producto);
        }

        private async Task CargarDetalleAsync(ProductoDTO producto)
        {
            var relaciones = await _serviceProductoEtiqueta.ListAsync();
            ViewBag.EtiquetasDelProducto = relaciones
                .Where(x => x.IdProducto == producto.IdProducto)
                .Select(x => x.IdEtiquetaNavigation?.Etiqueta1)
                .Where(nombre => !string.IsNullOrEmpty(nombre))
                .ToList();

            // Oferta vigente y productos relacionados de la misma categoría
            var productos = await _serviceProducto.ListAsync();
            var ofertas = await _catalogoOfertas.ObtenerOfertasAsync(productos);
            ViewBag.Oferta = ofertas.TryGetValue(producto.IdProducto, out var oferta) ? oferta : null;
            ViewBag.Relacionados = ProductoCardViewModel.Crear(
                productos.Where(p => p.IdCategoria == producto.IdCategoria && p.IdProducto != producto.IdProducto)
                         .Take(4),
                ofertas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarResena(ResenaDTO resenaDTO)
        {
            if (resenaDTO.Valoracion is < 1 or > 5)
            {
                TempData["ErrorMessage"] = "Selecciona una valoración de 1 a 5 estrellas.";
                return RedirectToAction(nameof(Details), new { id = resenaDTO.IdProducto });
            }

            if (string.IsNullOrWhiteSpace(resenaDTO.Comentario))
            {
                TempData["ErrorMessage"] = "Escribe un comentario para publicar tu reseña.";
                return RedirectToAction(nameof(Details), new { id = resenaDTO.IdProducto });
            }

            resenaDTO.IdUsuario = UsuarioDemoId;
            resenaDTO.Comentario = resenaDTO.Comentario.Trim();
            resenaDTO.Fecha = DateTime.Now;

            var idResena = await _serviceResena.AddAsync(resenaDTO);
            TempData[idResena > 0 ? "SuccessMessage" : "ErrorMessage"] =
                idResena > 0 ? "¡Gracias por tu opinión!" : "No se pudo registrar la reseña.";

            return RedirectToAction(nameof(Details), new { id = resenaDTO.IdProducto });
        }

        // ---------------------------------------------------------------- Mantenimiento

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarDatosViewBag();
            return View(new ProductoDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            ProductoDTO productoDto,
            List<int> selectedEtiqueta,
            List<IFormFile> imageFiles)
        {
            ValidarImagenes(imageFiles);

            if (!ModelState.IsValid)
            {
                await CargarDatosViewBag();
                return View(productoDto);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var producto = new Producto
                {
                    Nombre = productoDto.Nombre?.Trim(),
                    Descripcion = productoDto.Descripcion?.Trim(),
                    Precio = productoDto.Precio,
                    Stock = productoDto.Stock,
                    IdCategoria = productoDto.IdCategoria
                };

                await _context.Producto.AddAsync(producto);
                await _context.SaveChangesAsync();

                foreach (var idEtiqueta in selectedEtiqueta ?? new List<int>())
                {
                    await _serviceProductoEtiqueta.AddAsync(new EtiquetaProductoDTO
                    {
                        IdProducto = producto.IdProducto,
                        IdEtiqueta = idEtiqueta
                    });
                }

                var primera = true;
                foreach (var archivo in imageFiles.Where(f => f.Length > 0))
                {
                    await _context.ImagenProducto.AddAsync(new ImagenProducto
                    {
                        IdProducto = producto.IdProducto,
                        Foto = await LeerBytesAsync(archivo),
                        Principal = primera
                    });
                    primera = false;
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                TempData["SuccessMessage"] = $"Producto «{producto.Nombre}» creado correctamente.";
                return RedirectToAction(nameof(Crear));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error al crear el producto: {ex.Message}");
                await CargarDatosViewBag();
                return View(productoDto);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var producto = await _serviceProducto.FindByIdAsync(id);
            if (producto == null) return NotFound();

            await CargarEtiquetasSeleccionadas(id);
            await CargarDatosViewBag();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(
            ProductoDTO productoDto,
            List<int> selectedEtiquetas,
            List<IFormFile> imageFiles)
        {
            ValidarImagenes(imageFiles);

            // Las imágenes actuales no viajan en el formulario: se leen de la base de datos
            var actual = await _serviceProducto.FindByIdAsync(productoDto.IdProducto);
            if (actual == null) return NotFound();
            productoDto.ImagenProducto = actual.ImagenProducto;

            if (!ModelState.IsValid)
                return await VolverAEditar(productoDto, selectedEtiquetas);

            try
            {
                var productoId = await _serviceProducto.UpdateAsync(productoDto);
                if (productoId <= 0)
                {
                    ModelState.AddModelError("", "No se pudo actualizar el producto.");
                    return await VolverAEditar(productoDto, selectedEtiquetas);
                }

                // Reemplazar etiquetas
                var asociadas = (await _serviceProductoEtiqueta.ListAsync())
                    .Where(e => e.IdProducto == productoId)
                    .ToList();
                foreach (var etiqueta in asociadas)
                    await _serviceProductoEtiqueta.DeleteAsync(etiqueta);

                foreach (var idEtiqueta in selectedEtiquetas ?? new List<int>())
                {
                    await _serviceProductoEtiqueta.AddAsync(new EtiquetaProductoDTO
                    {
                        IdProducto = productoId,
                        IdEtiqueta = idEtiqueta
                    });
                }

                // Nuevas imágenes: la primera es la principal solo si el producto no tiene una
                var hayPrincipal = actual.ImagenProducto?.Any(i => i.Principal) == true;
                foreach (var archivo in imageFiles.Where(f => f.Length > 0))
                {
                    await _serviceImagenProducto.AddAsync(new ImagenProductoDTO
                    {
                        IdProducto = productoId,
                        Foto = await LeerBytesAsync(archivo),
                        Principal = !hayPrincipal
                    });
                    hayPrincipal = true;
                }

                TempData["SuccessMessage"] = "Cambios guardados correctamente.";
                return RedirectToAction(nameof(Editar), new { id = productoId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar: {ex.Message}");
                return await VolverAEditar(productoDto, selectedEtiquetas);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEImagen(int idProducto, int idImagen)
        {
            await _serviceImagenProducto.DeleteAsync(idImagen);
            TempData["SuccessMessage"] = "Imagen eliminada.";
            return RedirectToAction(nameof(Editar), new { id = idProducto });
        }

        // ---------------------------------------------------------------- Auxiliares

        private const long TamanoMaximoImagen = 5 * 1024 * 1024;

        private void ValidarImagenes(List<IFormFile>? archivos)
        {
            foreach (var archivo in archivos ?? new List<IFormFile>())
            {
                if (!archivo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError("", $"«{archivo.FileName}» no es una imagen.");
                else if (archivo.Length > TamanoMaximoImagen)
                    ModelState.AddModelError("", $"«{archivo.FileName}» supera los 5 MB.");
            }
        }

        private static async Task<byte[]> LeerBytesAsync(IFormFile archivo)
        {
            using var memoria = new MemoryStream();
            await archivo.CopyToAsync(memoria);
            return memoria.ToArray();
        }

        private async Task<ActionResult> VolverAEditar(ProductoDTO productoDto, List<int>? etiquetas)
        {
            ViewBag.EtiquetasDelProducto = etiquetas ?? new List<int>();
            await CargarDatosViewBag();
            return View(nameof(Editar), productoDto);
        }

        private async Task CargarEtiquetasSeleccionadas(int productoId)
        {
            var relaciones = await _serviceProductoEtiqueta.ListAsync();
            ViewBag.EtiquetasDelProducto = relaciones
                .Where(x => x.IdProducto == productoId)
                .Select(x => x.IdEtiqueta)
                .ToList();
        }

        private async Task CargarDatosViewBag()
        {
            ViewBag.ListCategorias = await _serviceCategoria.ListAsync() ?? new List<CategoriaDTO>();
            ViewBag.ListEtiquetas = await _serviceEtiqueta.ListAsync() ?? new List<EtiquetaDTO>();
            ViewBag.Productos = await _serviceProducto.ListAsync() ?? new List<ProductoDTO>();
        }
    }
}
