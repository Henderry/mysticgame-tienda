using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Services.Interfaces
{
    public interface IServicePromocionCategoria
    {
        Task<ICollection<PromocionCategoriaDTO>> ListAsync();
        Task<PromocionCategoriaDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PromocionCategoriaDTO producto);
        Task DeleteAsync(PromocionCategoriaDTO producto);
    }
}
