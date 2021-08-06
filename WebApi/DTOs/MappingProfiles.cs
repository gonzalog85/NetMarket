using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Producto, ProductoDTO>()
                .ForMember(d => d.CategoriaNombre, x => x.MapFrom(p => p.Categoria.Nombre))
                .ForMember(d => d.MarcaNombre, x => x.MapFrom(p => p.Marca.Nombre));
        }
    }
}
