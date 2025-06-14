using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEtiquetaProducto
    {
        Task<ICollection<EtiquetaProducto>> ListAsync();
        Task<int> AddAsync(EtiquetaProducto producto);
        Task DeleteAsync(EtiquetaProducto producto);
    }
}
