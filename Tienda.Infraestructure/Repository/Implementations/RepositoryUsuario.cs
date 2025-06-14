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
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly VideoGameContext _contex;

        public RepositoryUsuario(VideoGameContext contex)
        {
            _contex = contex;
        }
        public async Task<Usuario> FindByIdAsync(int id)
        {
            var @object = await _contex.Usuario
                .Include(p => p.IdRolNavigation)
                .FirstOrDefaultAsync(P => P.IdUsuario == id);
            return @object!;
        }

        public async Task<ICollection<Usuario>> ListAsync()
        {
            return await _contex.Usuario
               .Include(p => p.IdRolNavigation)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
