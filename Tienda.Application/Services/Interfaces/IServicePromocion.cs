using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Services.Interfaces
{
    public interface IServicePromocion
    {
        Task<ICollection<PromocionDTO>> ListAsync();
        Task<PromocionDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PromocionDTO producto);
        Task<int> UpdateAsync(PromocionDTO promocion);
    }
}
