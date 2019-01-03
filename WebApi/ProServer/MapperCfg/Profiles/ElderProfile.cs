using AutoMapper;
using ProInterface.Models;

namespace ProServer.MapperCfg.Profiles
{
    public class ElderProfile : Profile
    {
        public ElderProfile()
        {
            CreateMap<FA_ELDER, fa_elder>()
                .ForMember(d => d.ID, opt => { opt.MapFrom(m => m.ID == 0 ? Fun.GetSeqID<FA_ELDER>() : m.ID); })
                ;
            CreateMap<fa_elder, FA_ELDER>()
                ;
        }
    }
}
