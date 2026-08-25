using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiEmociones.Data;
using ApiEmociones.Models;
using AutoMapper;

namespace ApiEmociones.Infraestructura
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Nota, NotaDTO>().ReverseMap();
            CreateMap<Nota, NotaInputDTO>().ReverseMap();
            CreateMap<Usuarios, UsuarioDTO>().ReverseMap();
            CreateMap<Emociones, EmocionDTO>().ReverseMap();
        }
    }
}