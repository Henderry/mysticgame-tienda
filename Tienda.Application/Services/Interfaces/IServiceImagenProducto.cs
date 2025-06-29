using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Services.Interfaces
{
    public interface IServiceImagenProducto
    {
        Task<ICollection<ImagenProductoDTO>> ListAsync();
        Task AddAsync(ImagenProductoDTO producto);
        Task DeleteAsync(int id);
    }
}
