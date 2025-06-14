using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Data;
using Tienda.Infraestructure.Models;
using Tienda.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Tienda.Infraestructure.Repository.Implementations
{
    public class RepositoryCategoria : IRepositoryCategoria
    {
        private readonly VideoGameContext _contex;

        public RepositoryCategoria(VideoGameContext contex)
        {
            _contex = contex;
        }

        public async Task<Categoria> FindByIdAsync(int id)
        {
            var @object = await _contex.Categoria
                 .Include(p => p.Producto).ThenInclude(i => i.ImagenProducto)
                 .FirstOrDefaultAsync(P => P.IdCategoria == id);
            return @object!;
        }

        public async Task<ICollection<Categoria>> ListAsync()
        {
            return await _contex.Set<Categoria>()
             .Include(p => p.Producto).ThenInclude(i => i.ImagenProducto)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
