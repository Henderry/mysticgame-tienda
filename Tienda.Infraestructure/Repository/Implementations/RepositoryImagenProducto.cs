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
    public class RepositoryImagenProducto : IRepositoryImagenProducto
    {
        private readonly VideoGameContext _contex;

        public RepositoryImagenProducto(VideoGameContext contex)
        {
            _contex = contex;
        }


        public async Task<ICollection<ImagenProducto>> ListAsync()
        {
            return await _contex.ImagenProducto
                .Include(p => p.IdProductoNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(ImagenProducto producto)
        {
            await _contex.Set<ImagenProducto>().AddAsync(producto);
            await _contex.SaveChangesAsync();

        }
        public async Task DeleteAsync(int id)
        {
            var imagen = await _contex.Set<ImagenProducto>().FindAsync(id);
            if (imagen != null)
            {
                _contex.Set<ImagenProducto>().Remove(imagen);
                await _contex.SaveChangesAsync();
            }
        }
    }
}
