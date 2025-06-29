using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Services.Interfaces
{
    public interface IServiceEtiquetaProducto
    {
        Task<ICollection<EtiquetaProductoDTO>> ListAsync();
        Task<int> AddAsync(EtiquetaProductoDTO producto);
        Task DeleteAsync(EtiquetaProductoDTO producto);
    }
}
