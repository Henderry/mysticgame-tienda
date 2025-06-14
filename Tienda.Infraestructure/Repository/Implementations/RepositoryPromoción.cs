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
    public class RepositoryPromoción : IRepositoryPromocion
    {
        private readonly VideoGameContext _contex;

        public RepositoryPromoción(VideoGameContext contex)
        {
            _contex = contex;
        }

        public async Task<Promocion> FindByIdAsync(int id)
        {
            var @object = await _contex.Promocion
                .Include(p => p.IdTipoPromocionNavigation)
                .FirstOrDefaultAsync(P => P.IdPromocion == id);
            return @object!;
        }

        public async Task<ICollection<Promocion>> ListAsync()
        {
            return await _contex.Promocion
               .Include(p => p.IdTipoPromocionNavigation)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<int> AddAsync(Promocion producto)
        {
            await _contex.Set<Promocion>().AddAsync(producto);
            await _contex.SaveChangesAsync();
            return (int)producto.IdPromocion;
        }
        public async Task<int> UpdateAsync(Promocion producto)
        {
            var objeto = await _contex.Promocion
                                            .FirstOrDefaultAsync(x => x.IdPromocion == producto.IdPromocion);
            if (objeto != null)

                // Actualizar propiedades
                _contex.Entry(objeto).CurrentValues.SetValues(producto);

            await _contex.SaveChangesAsync();
            return objeto.IdPromocion;
        }
    }
}
