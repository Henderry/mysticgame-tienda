using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryTipoPromocion
    {
        Task<ICollection<TipoPromocion>> ListAsync();
    }
}
