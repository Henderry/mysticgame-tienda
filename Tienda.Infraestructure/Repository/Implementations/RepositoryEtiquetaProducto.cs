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
    public class RepositoryEtiquetaProducto : IRepositoryEtiquetaProducto
    {
        private readonly VideoGameContext _contex;

        public RepositoryEtiquetaProducto(VideoGameContext contex)
        {
            _contex = contex;
        }

        public async Task<int> AddAsync(EtiquetaProducto producto)
        {
            await _contex.Set<EtiquetaProducto>().AddAsync(producto);
            await _contex.SaveChangesAsync();
            return (int)producto.IdEtiqueta;
        }

        public async Task DeleteAsync(EtiquetaProducto producto)
        {
            var objeto = await _contex.EtiquetaProducto
            .FirstOrDefaultAsync(e => e.IdProducto == producto.IdProducto &&
                                    e.IdEtiqueta == producto.IdEtiqueta);

            if (objeto != null)
            {
                _contex.EtiquetaProducto.Remove(objeto);
                await _contex.SaveChangesAsync();
            }
        }

        public async Task<ICollection<EtiquetaProducto>> ListAsync()
        {
            return await _contex.EtiquetaProducto
                .Include(e => e.IdEtiquetaNavigation)
                .Include(e => e.IdProductoNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
