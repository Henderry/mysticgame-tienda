using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPromocionCategoria
    {
        Task<ICollection<PromocionCategoria>> ListAsync();
        Task<PromocionCategoria> FindByIdAsync(int id);
        Task<int> AddAsync(PromocionCategoria producto);
        Task DeleteAsync(PromocionCategoria producto);
    }
}
