using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Models;
using Tienda.Infraestructure.Repository.Interfaces;

namespace Tienda.Application.Services.Implementations
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IMapper _mapper;

        public ServiceProducto(IRepositoryProducto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectM = _mapper.Map<ProductoDTO>(@object);
            return objectM;
        }

        public async Task<ICollection<ProductoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ProductoDTO>>(list);
            return collection;
        }

        public async Task<int> UpdateAsync(ProductoDTO producto)
        {
            var objectM = _mapper.Map<Producto>(producto);
            return await _repository.UpdateAsync(objectM);
        }

        public async Task<int> AddAsync(ProductoDTO producto)
        {
            var objectM = _mapper.Map<Producto>(producto);
            return await _repository.AddAsync(objectM);
        }
    }
}
