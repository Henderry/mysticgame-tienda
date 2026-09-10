using Tienda.Application.DTOs;

namespace Tienda.Web.Models
{
    public class HomeViewModel
    {
        public List<CategoriaDTO> Categorias { get; init; } = new();
        public List<ProductoCardViewModel> Novedades { get; init; } = new();
        public List<ProductoCardViewModel> Ofertas { get; init; } = new();
        public List<PromocionDTO> Promociones { get; init; } = new();
        public int TotalProductos { get; init; }
    }
}
