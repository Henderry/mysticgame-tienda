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
    public class ServicePromocionProducto : IServicePromocionProducto
    {
        private readonly IRepositoryPromocionProducto _repository;
        private readonly IMapper _mapper;

        public ServicePromocionProducto(IRepositoryPromocionProducto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(PromocionProductoDTO producto)
        {
            var objectM = _mapper.Map<PromocionProducto>(producto);
            return await _repository.AddAsync(objectM);
        }

        public async Task DeleteAsync(PromocionProductoDTO producto)
        {
            var objectM = _mapper.Map<PromocionProducto>(producto);
            await _repository.DeleteAsync(objectM);
        }

        public async Task<PromocionProductoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectM = _mapper.Map<PromocionProductoDTO>(@object);
            return objectM;
        }

        public async Task<ICollection<PromocionProductoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PromocionProductoDTO>>(list);
            return collection;
        }
    }
}
