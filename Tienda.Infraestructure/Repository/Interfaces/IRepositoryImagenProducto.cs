using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryImagenProducto
    {
        Task<ICollection<ImagenProducto>> ListAsync();
        Task AddAsync(ImagenProducto producto);
        Task DeleteAsync(int id);
    }
}
