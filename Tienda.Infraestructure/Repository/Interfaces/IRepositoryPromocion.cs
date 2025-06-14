using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPromocion
    {
        Task<ICollection<Promocion>> ListAsync();
        Task<Promocion> FindByIdAsync(int id);

        Task<int> AddAsync(Promocion producto);
        Task<int> UpdateAsync(Promocion producto);
    }
}
