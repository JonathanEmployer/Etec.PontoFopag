using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.App_Start
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Models.Ponto.RegistroPontoMetaData, Models.Ponto.RegistroPonto>().ReverseMap();
        }
    }
}