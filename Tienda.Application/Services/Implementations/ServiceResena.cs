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
    public class ServiceResena : IServiceResena
    {
        private readonly IRepositoryResena _repository;
        private readonly IMapper _mapper;

        public ServiceResena(IRepositoryResena repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(ResenaDTO resena)
        {
            var Objeto = _mapper.Map<Resena>(resena);
            return await _repository.AddAsync(Objeto);
        }

        public async Task<ResenaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectM = _mapper.Map<ResenaDTO>(@object);
            return objectM;
        }

        public async Task<ICollection<ResenaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ResenaDTO>>(list);
            return collection;
        }
    }
}
