using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Services.Interfaces
{
    public interface IServiceResena
    {
        Task<ICollection<ResenaDTO>> ListAsync();
        Task<ResenaDTO> FindByIdAsync(int id);
        Task<int> AddAsync(ResenaDTO resena);
    }
}
