using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProducto
    {
        Task<ICollection<Producto>> ListAsync();
        Task<Producto> FindByIdAsync(int id);

        Task<int> AddAsync(Producto producto);
        Task<int> UpdateAsync(Producto producto);

    }
}
