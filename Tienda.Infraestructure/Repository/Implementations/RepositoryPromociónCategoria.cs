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
    public class RepositoryPromociónCategoria : IRepositoryPromocionCategoria
    {
        private readonly VideoGameContext _contex;

        public RepositoryPromociónCategoria(VideoGameContext contex)
        {
            _contex = contex;
        }

        public async Task<int> AddAsync(PromocionCategoria producto)
        {
            await _contex.Set<PromocionCategoria>().AddAsync(producto);
            await _contex.SaveChangesAsync();
            return (int)producto.IdCategoria;
        }

        public async Task DeleteAsync(PromocionCategoria producto)
        {
            var objeto = await _contex.PromocionCategoria
            .FirstOrDefaultAsync(e => e.IdPromocion == producto.IdPromocion &&
                                    e.IdCategoria == producto.IdCategoria);

            if (objeto != null)
            {
                _contex.PromocionCategoria.Remove(objeto);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<PromocionCategoria> FindByIdAsync(int id)
        {
            var @object = await _contex.PromocionCategoria
             .Include(x => x.IdPromocionNavigation)
            .Include(x => x.IdCategoriaNavigation).ThenInclude(e => e.Producto).ThenInclude(i => i.ImagenProducto)
                 .FirstOrDefaultAsync(P => P.IdPromocion == id);
            return @object!;
        }

        public async Task<ICollection<PromocionCategoria>> ListAsync()
        {
            return await _contex.PromocionCategoria
            .Include(x => x.IdPromocionNavigation)
            .Include(x => x.IdCategoriaNavigation).ThenInclude(e => e.Producto).ThenInclude(i => i.ImagenProducto)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
