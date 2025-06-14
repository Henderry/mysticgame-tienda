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
    public class RepositoryTipoPromoción : IRepositoryTipoPromocion
    {
        private readonly VideoGameContext _contex;

        public RepositoryTipoPromoción(VideoGameContext contex)
        {
            _contex = contex;
        }
        public async Task<ICollection<TipoPromocion>> ListAsync()
        {
            return await _contex.Set<TipoPromocion>()
            .AsNoTracking()
            .ToListAsync();
        }
    }
}
