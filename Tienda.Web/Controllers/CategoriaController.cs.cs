using Microsoft.AspNetCore.Mvc;

namespace Tienda.Web.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
