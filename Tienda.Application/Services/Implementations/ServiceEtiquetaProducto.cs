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
    public class ServiceEtiquetaProducto : IServiceEtiquetaProducto
    {
        private readonly IRepositoryEtiquetaProducto _repository;
        private readonly IMapper _mapper;

        public ServiceEtiquetaProducto(IRepositoryEtiquetaProducto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(EtiquetaProductoDTO producto)
        {
            var objectM = _mapper.Map<EtiquetaProducto>(producto);
            return await _repository.AddAsync(objectM);
        }

        public async Task DeleteAsync(EtiquetaProductoDTO producto)
        {
            var objectM = _mapper.Map<EtiquetaProducto>(producto);
            await _repository.DeleteAsync(objectM);
        }

        public async Task<ICollection<EtiquetaProductoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<EtiquetaProductoDTO>>(list);
            return collection;
        }
    }
}
