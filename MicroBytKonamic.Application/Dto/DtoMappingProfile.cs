using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Dto
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            // Posts
            CreateMap<Postale, FelicitacionDto>();
            CreateMap<FelicitacionDto, Postale>();
            CreateMap<Fortune, FortuneOfDayDto>();
            CreateMap<Language, LanguageDto>();
        }
    }
}
