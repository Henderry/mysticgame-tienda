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
    public class ServiceEtiqueta : IServiceEtiqueta
    {
        private readonly IRepositoryEtiqueta _repository;
        private readonly IMapper _mapper;

        public ServiceEtiqueta(IRepositoryEtiqueta repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ICollection<EtiquetaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<EtiquetaDTO>>(list);
            return collection;
        }
    }
}
