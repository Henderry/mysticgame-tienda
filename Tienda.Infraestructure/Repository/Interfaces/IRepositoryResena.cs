using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Infraestructure.Models;

namespace Tienda.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryResena
    {
        Task<ICollection<Resena>> ListAsync();
        Task<Resena> FindByIdAsync(int id);
        Task<int> AddAsync(Resena resena);
    }
}
