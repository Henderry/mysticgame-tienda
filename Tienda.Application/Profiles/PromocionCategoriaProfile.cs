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
    public class PromocionCategoriaProfile : Profile
    {
        public PromocionCategoriaProfile()
        {
            CreateMap<PromocionCategoriaDTO, PromocionCategoria>().ReverseMap();
            CreateMap<PromocionCategoriaDTO, PromocionCategoria>();
        }
    }
}
