using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Services.Implementations;
using Tienda.Application.Services.Interfaces;

namespace Tienda.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IServiceCategoria _serviceCategoria;

        public HomeController(
            IServiceCategoria serviceCategoria,
            ILogger<HomeController> logger

            )
        {
            _serviceCategoria = serviceCategoria;
            _logger = logger;

        }


        public async Task<IActionResult> Index()
        {
            var productos = await _serviceCategoria.ListAsync();
            return View(productos);
        }

        public async Task<IActionResult> CategoriaxProducto(int id)
        {
            var producto = await _serviceCategoria.FindByIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

    }
}
