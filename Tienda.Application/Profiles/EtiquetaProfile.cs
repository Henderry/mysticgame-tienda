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
    public class EtiquetaProfile : Profile
    {
        public EtiquetaProfile()
        {
            CreateMap<EtiquetaDTO, Etiqueta>().ReverseMap();
            CreateMap<EtiquetaDTO, Etiqueta>();
        }
    }
}
