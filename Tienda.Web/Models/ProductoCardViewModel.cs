using Tienda.Application.DTOs;

namespace Tienda.Web.Models
{
    /// <summary>Datos que necesita la tarjeta de producto (_ProductoCard).</summary>
    public class ProductoCardViewModel
    {
        public ProductoDTO Producto { get; init; } = null!;
        public OfertaActiva? Oferta { get; init; }

        public decimal PrecioBase => Producto.Precio ?? 0m;
        public decimal PrecioFinal => Oferta?.PrecioFinal(PrecioBase) ?? PrecioBase;

        public ImagenProductoDTO? ImagenPrincipal =>
            Producto.ImagenProducto?.FirstOrDefault(i => i.Principal) ?? Producto.ImagenProducto?.FirstOrDefault();

        /// <summary>Solo cuenta valoraciones válidas (1 a 5 estrellas).</summary>
        private IEnumerable<ResenaDTO> ResenasValidas =>
            Producto.Resena?.Where(r => r.Valoracion is >= 1 and <= 5) ?? Enumerable.Empty<ResenaDTO>();

        public int CantidadValoraciones => ResenasValidas.Count();

        public double? PromedioValoracion =>
            CantidadValoraciones > 0 ? ResenasValidas.Average(r => (double)r.Valoracion) : null;

        public static List<ProductoCardViewModel> Crear(
            IEnumerable<ProductoDTO> productos,
            IReadOnlyDictionary<int, OfertaActiva> ofertas) =>
            productos
                .Select(p => new ProductoCardViewModel
                {
                    Producto = p,
                    Oferta = ofertas.TryGetValue(p.IdProducto, out var oferta) ? oferta : null
                })
                .ToList();
    }
}
