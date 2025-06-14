using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPromocionProducto
    {
        Task<ICollection<PromocionProducto>> ListAsync();
        Task<PromocionProducto> FindByIdAsync(int id);
        Task<int> AddAsync(PromocionProducto producto);
        Task DeleteAsync(PromocionProducto producto);
    }
}
