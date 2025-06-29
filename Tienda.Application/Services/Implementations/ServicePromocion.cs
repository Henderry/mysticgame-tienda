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
    public class ServicePromocion : IServicePromocion
    {
        private readonly IRepositoryPromocion _repository;
        private readonly IMapper _mapper;

        public ServicePromocion(IRepositoryPromocion repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(PromocionDTO producto)
        {
            var objectM = _mapper.Map<Promocion>(producto);
            return await _repository.AddAsync(objectM);
        }

        public async Task<PromocionDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectM = _mapper.Map<PromocionDTO>(@object);
            return objectM;
        }

        public async Task<ICollection<PromocionDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PromocionDTO>>(list);
            return collection;
        }
        public async Task<int> UpdateAsync(PromocionDTO dto)    // ← implementación
        {
            var entity = _mapper.Map<Promocion>(dto);
            return await _repository.UpdateAsync(entity);
        }
    }
}
