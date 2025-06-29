using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Application.DTOs;
using Tienda.Infraestructure.Models;

namespace Tienda.Application.Profiles
{
    public class EtiquetaProductoProfile : Profile
    {
        public EtiquetaProductoProfile()
        {
            CreateMap<EtiquetaProductoDTO, EtiquetaProducto>()
                .ForMember(dest => dest.IdEtiquetaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
