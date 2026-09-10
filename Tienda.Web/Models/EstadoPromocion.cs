using Tienda.Application.DTOs;

namespace Tienda.Web.Models
{
    public enum EstadoPromocion
    {
        Activa,
        Proxima,
        Finalizada
    }

    public static class EstadoPromocionExtensions
    {
        public static EstadoPromocion Estado(this PromocionDTO promocion, DateTime? hoy = null)
        {
            var fecha = (hoy ?? DateTime.Today).Date;
            if (promocion.FechaInicio?.Date > fecha) return EstadoPromocion.Proxima;
            if (promocion.FechaFin?.Date < fecha) return EstadoPromocion.Finalizada;
            return EstadoPromocion.Activa;
        }

        public static string Texto(this EstadoPromocion estado) => estado switch
        {
            EstadoPromocion.Activa => "Activa",
            EstadoPromocion.Proxima => "Próximamente",
            _ => "Finalizada"
        };

        public static string Clase(this EstadoPromocion estado) => estado switch
        {
            EstadoPromocion.Activa => "activa",
            EstadoPromocion.Proxima => "proxima",
            _ => "finalizada"
        };

        /// <summary>Días que faltan para que termine (0 si termina hoy, null si no aplica).</summary>
        public static int? DiasRestantes(this PromocionDTO promocion, DateTime? hoy = null)
        {
            if (promocion.FechaFin is null) return null;
            var dias = (promocion.FechaFin.Value.Date - (hoy ?? DateTime.Today).Date).Days;
            return dias < 0 ? null : dias;
        }
    }
}
