using Tienda.Application.DTOs;
using Tienda.Application.Services.Interfaces;
using Tienda.Web.Models;

namespace Tienda.Web.Services
{
    public interface ICatalogoOfertas
    {
        /// <summary>
        /// Devuelve, por IdProducto, la mejor promoción vigente hoy
        /// (ya sea asignada al producto o a su categoría).
        /// </summary>
        Task<IReadOnlyDictionary<int, OfertaActiva>> ObtenerOfertasAsync(IEnumerable<ProductoDTO> productos);
    }

    public class CatalogoOfertas : ICatalogoOfertas
    {
        private readonly IServicePromocionProducto _promocionesProducto;
        private readonly IServicePromocionCategoria _promocionesCategoria;

        public CatalogoOfertas(
            IServicePromocionProducto promocionesProducto,
            IServicePromocionCategoria promocionesCategoria)
        {
            _promocionesProducto = promocionesProducto;
            _promocionesCategoria = promocionesCategoria;
        }

        public async Task<IReadOnlyDictionary<int, OfertaActiva>> ObtenerOfertasAsync(IEnumerable<ProductoDTO> productos)
        {
            var ofertas = new Dictionary<int, OfertaActiva>();

            void Registrar(int idProducto, PromocionDTO promocion)
            {
                var oferta = OfertaActiva.Desde(promocion);
                if (!ofertas.TryGetValue(idProducto, out var actual) || oferta.Descuento > actual.Descuento)
                    ofertas[idProducto] = oferta;
            }

            static bool Vigente(PromocionDTO? promocion) =>
                promocion is not null
                && (promocion.Descuento ?? 0) > 0
                && promocion.Estado() == EstadoPromocion.Activa;

            foreach (var relacion in await _promocionesProducto.ListAsync())
            {
                if (Vigente(relacion.IdPromocionNavigation))
                    Registrar(relacion.IdProducto, relacion.IdPromocionNavigation!);
            }

            var porCategoria = (await _promocionesCategoria.ListAsync())
                .Where(r => Vigente(r.IdPromocionNavigation))
                .ToList();

            foreach (var producto in productos)
            {
                foreach (var relacion in porCategoria.Where(r => r.IdCategoria == producto.IdCategoria))
                    Registrar(producto.IdProducto, relacion.IdPromocionNavigation!);
            }

            return ofertas;
        }
    }
}
