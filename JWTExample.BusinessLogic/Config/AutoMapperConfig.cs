using AutoMapper;
using JWTExample.DataAccess.Models;
using JWTExample.Dto.Account;

namespace JWTExample.BusinessLogic.Config
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterUserDto, User>()
                    .ForMember(dest => dest.UserName,
                        opts => opts.MapFrom(src => src.Email));
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
