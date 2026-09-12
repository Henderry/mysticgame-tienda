using Libreria.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tienda.Application.Services.Interfaces;
using Tienda.Web.Models;
using Tienda.Web.Services;

namespace Tienda.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceCategoria _serviceCategoria;
        private readonly IServiceProducto _serviceProducto;
        private readonly IServicePromocion _servicePromocion;
        private readonly ICatalogoOfertas _catalogoOfertas;

        public HomeController(
            IServiceCategoria serviceCategoria,
            IServiceProducto serviceProducto,
            IServicePromocion servicePromocion,
            ICatalogoOfertas catalogoOfertas)
        {
            _serviceCategoria = serviceCategoria;
            _serviceProducto = serviceProducto;
            _servicePromocion = servicePromocion;
            _catalogoOfertas = catalogoOfertas;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _serviceCategoria.ListAsync();
            var productos = await _serviceProducto.ListAsync();
            var ofertas = await _catalogoOfertas.ObtenerOfertasAsync(productos);
            var tarjetas = ProductoCardViewModel.Crear(productos, ofertas);

            var modelo = new HomeViewModel
            {
                Categorias = categorias.OrderBy(c => c.Categoria1).ToList(),
                TotalProductos = productos.Count,
                Novedades = tarjetas.OrderByDescending(t => t.Producto.IdProducto).Take(8).ToList(),
                Ofertas = tarjetas.Where(t => t.Oferta != null)
                                  .OrderByDescending(t => t.Oferta!.Descuento)
                                  .Take(4)
                                  .ToList(),
                Promociones = (await _servicePromocion.ListAsync())
                    .Where(p => p.Estado() == EstadoPromocion.Activa)
                    .OrderBy(p => p.FechaFin)
                    .Take(3)
                    .ToList()
            };

            return View(modelo);
        }

        /// <summary>Productos de una categoría.</summary>
        public async Task<IActionResult> CategoriaxProducto(int id)
        {
            var categoria = await _serviceCategoria.FindByIdAsync(id);
            if (categoria == null) return NotFound();

            // Se usa el listado general porque incluye categoría y reseñas de cada producto
            var productos = (await _serviceProducto.ListAsync())
                .Where(p => p.IdCategoria == id)
                .ToList();
            var ofertas = await _catalogoOfertas.ObtenerOfertasAsync(productos);

            ViewBag.Categoria = categoria;
            return View(ProductoCardViewModel.Crear(productos, ofertas));
        }

        /// <summary>Página de error en producción (UseExceptionHandler).</summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        /// <summary>Página para rutas o registros que no existen (404).</summary>
        [Route("no-encontrado")]
        public IActionResult NoEncontrado()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View();
        }

        /// <summary>Página a la que redirige ErrorHandlingMiddleware cuando ocurre una excepción.</summary>
        public IActionResult ErrorHandler(string? messagesJson)
        {
            ErrorMiddlewareViewModel? error = null;
            if (!string.IsNullOrWhiteSpace(messagesJson))
            {
                try
                {
                    error = JsonConvert.DeserializeObject<ErrorMiddlewareViewModel>(messagesJson);
                }
                catch (JsonException)
                {
                    error = null;
                }
            }

            ViewBag.ErrorMessages = error;
            return View("~/Views/Shared/ErrorHandler.cshtml");
        }
    }
}
