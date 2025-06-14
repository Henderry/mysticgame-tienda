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
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly VideoGameContext _contex;

        public RepositoryProducto(VideoGameContext contex)
        {
            _contex = contex;
        }


        public async Task<Producto> FindByIdAsync(int id)
        {
            var @object = await _contex.Producto
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.ImagenProducto)
                .Include(p => p.Resena).ThenInclude(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(P => P.IdProducto == id);
            return @object!;
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            return await _contex.Producto
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.ImagenProducto)
                .Include(p => p.Resena)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<int> AddAsync(Producto producto)
        {
            await _contex.Set<Producto>().AddAsync(producto);
            await _contex.SaveChangesAsync();
            return producto.IdProducto;
        }
        public async Task<int> UpdateAsync(Producto producto)
        {
            var objeto = await _contex.Producto
                                           .FirstOrDefaultAsync(x => x.IdProducto == producto.IdProducto);
            if (objeto != null)

                // Actualizar propiedades
                _contex.Entry(objeto).CurrentValues.SetValues(producto);

            await _contex.SaveChangesAsync();
            return objeto.IdProducto;
        }
    }
}
