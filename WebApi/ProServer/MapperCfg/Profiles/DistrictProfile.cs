using AutoMapper;
using ProInterface.Models;

namespace ProServer.MapperCfg.Profiles
{
    public class DistrictProfile: Profile
    {
        public DistrictProfile()
        {
            CreateMap<DISTRICT, fa_district>()
                .ForMember(d => d.CODE, opt => { opt.MapFrom(m => string.IsNullOrEmpty(m.CODE) ? m.ID.ToString() : m.CODE); })
                .ForMember(d => d.IN_USE, opt => { opt.MapFrom(m =>1); })
                .ForMember(d => d.REGION, opt => { opt.MapFrom(m =>1); })
                ;
            CreateMap<fa_district, DISTRICT>();
        }
    }
}
