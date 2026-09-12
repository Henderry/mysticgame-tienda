using Tienda.Application.DTOs;

namespace Tienda.Web.Models
{
    /// <summary>
    /// Descuento que se aplica a un producto por una promoción (por producto o por categoría).
    /// El descuento se guarda en la base de datos como fracción: 0.15 = 15 %.
    /// </summary>
    public class OfertaActiva
    {
        public int IdPromocion { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public decimal Descuento { get; init; }
        public DateTime? FechaFin { get; init; }

        public int Porcentaje => (int)Math.Round(Descuento * 100);

        public decimal PrecioFinal(decimal precio) => Math.Round(precio * (1 - Descuento), 0);

        public static OfertaActiva Desde(PromocionDTO promocion) => new()
        {
            IdPromocion = promocion.IdPromocion,
            Nombre = promocion.Nombre ?? "Promoción",
            Descuento = promocion.Descuento ?? 0m,
            FechaFin = promocion.FechaFin
        };
    }
}
