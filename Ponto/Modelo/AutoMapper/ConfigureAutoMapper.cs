using AutoMapper;

namespace Modelo.AutoMapper
{
    public static class ConfigureAutoMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Modelo.Marcacao, Modelo.Marcacao>();
                cfg.CreateMap<Modelo.BilhetesImp, Modelo.BilhetesImp>();
            });
        }
    }
}
