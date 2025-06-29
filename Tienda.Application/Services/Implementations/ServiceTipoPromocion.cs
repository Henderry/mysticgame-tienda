using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Application.Services.Interfaces;
using Tienda.Infraestructure.Repository.Interfaces;

namespace Tienda.Application.Services.Implementations
{
    public class ServiceTipoPromocion : IServiceTipoPromocion
    {
        private readonly IRepositoryTipoPromocion _repository;
        private readonly IMapper _mapper;

        public ServiceTipoPromocion(IRepositoryTipoPromocion repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ICollection<TipoPromocionDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<TipoPromocionDTO>>(list);
            return collection;
        }
    }
}
