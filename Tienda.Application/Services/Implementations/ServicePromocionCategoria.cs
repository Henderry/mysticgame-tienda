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
    public class ServicePromocionCategoria : IServicePromocionCategoria
    {
        private readonly IRepositoryPromocionCategoria _repository;
        private readonly IMapper _mapper;

        public ServicePromocionCategoria(IRepositoryPromocionCategoria repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(PromocionCategoriaDTO producto)
        {
            var objectM = _mapper.Map<PromocionCategoria>(producto);
            return await _repository.AddAsync(objectM);
        }

        public async Task DeleteAsync(PromocionCategoriaDTO producto)
        {
            var objectM = _mapper.Map<PromocionCategoria>(producto);
            await _repository.DeleteAsync(objectM);
        }

        public async Task<PromocionCategoriaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectM = _mapper.Map<PromocionCategoriaDTO>(@object);
            return objectM;
        }

        public async Task<ICollection<PromocionCategoriaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PromocionCategoriaDTO>>(list);
            return collection;
        }
    }
}
