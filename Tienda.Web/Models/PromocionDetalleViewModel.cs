using Tienda.Application.DTOs;

namespace Tienda.Web.Models
{
    public class PromocionDetalleViewModel
    {
        public PromocionDTO Promocion { get; init; } = null!;

        /// <summary>"Productos seleccionados" o el nombre de las categorías incluidas.</summary>
        public string Alcance { get; init; } = string.Empty;

        public bool EsPorCategoria { get; init; }

        public List<ProductoCardViewModel> Productos { get; init; } = new();
    }
}
