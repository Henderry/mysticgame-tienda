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
    public class ServiceImagenProducto : IServiceImagenProducto
    {
        private readonly IRepositoryImagenProducto _repository;
        private readonly IMapper _mapper;

        public ServiceImagenProducto(IRepositoryImagenProducto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ICollection<ImagenProductoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ImagenProductoDTO>>(list);
            return collection;
        }

        public async Task AddAsync(ImagenProductoDTO producto)
        {
            if (producto.Foto == null || producto.Foto.Length == 0)
                throw new ArgumentException("La imagen no puede estar vacía");

            var entity = new ImagenProducto
            {
                IdProducto = producto.IdProducto,
                Foto = producto.Foto,
                Principal = producto.Principal
            };

            await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
