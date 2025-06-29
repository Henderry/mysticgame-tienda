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
    public class PromocionProfile : Profile
    {
        public PromocionProfile()
        {
            CreateMap<PromocionDTO, Promocion>().ReverseMap();
            CreateMap<PromocionDTO, Promocion>();
        }
    }
}
