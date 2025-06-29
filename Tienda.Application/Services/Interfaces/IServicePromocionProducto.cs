using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Services.Interfaces
{
    public interface IServicePromocionProducto
    {
        Task<ICollection<PromocionProductoDTO>> ListAsync();
        Task<PromocionProductoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PromocionProductoDTO producto);
        Task DeleteAsync(PromocionProductoDTO producto);
    }
}
