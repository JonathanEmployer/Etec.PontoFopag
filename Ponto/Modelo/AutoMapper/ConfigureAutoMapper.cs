using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.AutoMapper
{
    public static class ConfigureAutoMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Modelo.Marcacao, Modelo.Marcacao>();
            });
        }
    }
}
