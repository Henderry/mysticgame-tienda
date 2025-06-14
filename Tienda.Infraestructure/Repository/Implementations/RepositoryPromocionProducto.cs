using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Models;
using Tienda.Infraestructure.Repository.Interfaces;

namespace Tienda.Infraestructure.Repository.Implementations
{
    public class RepositoryPromocionProducto : IRepositoryPromocionProducto
    {
        private readonly VideoGameContext _contex;

        public RepositoryPromocionProducto(VideoGameContext contex)
        {
            _contex = contex;
        }

        public async Task<int> AddAsync(PromocionProducto producto)
        {
            await _contex.Set<PromocionProducto>().AddAsync(producto);
            await _contex.SaveChangesAsync();
            return (int)producto.IdProducto;
        }

        public async Task DeleteAsync(PromocionProducto producto)
        {
            var objeto = await _contex.PromocionProducto
            .FirstOrDefaultAsync(e => e.IdPromocion == producto.IdPromocion &&
                                    e.IdProducto == producto.IdProducto);

            if (objeto != null)
            {
                _contex.PromocionProducto.Remove(objeto);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<PromocionProducto> FindByIdAsync(int id)
        {
            var @object = await _contex.PromocionProducto
             .Include(x => x.IdPromocionNavigation)
            .Include(x => x.IdProductoNavigation).ThenInclude(i => i.ImagenProducto)
                 .FirstOrDefaultAsync(P => P.IdPromocion == id);
            return @object!;
        }

        public async Task<ICollection<PromocionProducto>> ListAsync()
        {
            return await _contex.PromocionProducto
            .Include(x => x.IdPromocionNavigation)
            .Include(x => x.IdProductoNavigation).ThenInclude(i => i.ImagenProducto)
            .AsNoTracking()
            .ToListAsync();

        }
    }
}
