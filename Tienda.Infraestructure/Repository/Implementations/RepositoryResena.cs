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
    public class RepositoryResena : IRepositoryResena
    {
        private readonly VideoGameContext _contex;

        public RepositoryResena(VideoGameContext contex)
        {
            _contex = contex;
        }

        public async Task<int> AddAsync(Resena resena)
        {
            await _contex.Set<Resena>().AddAsync(resena);
            await _contex.SaveChangesAsync();
            return resena.IdResena;
        }

        public async Task<Resena> FindByIdAsync(int id)
        {
            var @object = await _contex.Resena
             .Include(p => p.IdProductoNavigation)
             .Include(p => p.IdUsuarioNavigation)
             .FirstOrDefaultAsync(p => p.IdResena == id);
            return @object!;
        }

        public async Task<ICollection<Resena>> ListAsync()
        {
            return await _contex.Resena
            .Include(x => x.IdProductoNavigation)
            .Include(x => x.IdUsuarioNavigation)
            .AsNoTracking()
            .ToListAsync();
        }


    }
}
