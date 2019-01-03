using AutoMapper;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<TUser, fa_user>()
                .ForMember(d => d.DISTRICT_ID, opt => { opt.MapFrom(m => (m.DISTRICT_ID == 0) ? 1 : m.DISTRICT_ID); })
                .ForMember(d => d.CREATE_TIME, opt => { opt.MapFrom(m => (m.CREATE_TIME == null) ? DateTime.Now : m.CREATE_TIME); })
                .ForMember(d => d.LAST_ACTIVE_TIME, opt => { opt.MapFrom(m => (m.LAST_ACTIVE_TIME == null) ? DateTime.Now : m.LAST_ACTIVE_TIME); })
                .ForMember(d => d.LAST_LOGIN_TIME, opt => { opt.MapFrom(m => (m.LAST_LOGIN_TIME == null) ? DateTime.Now : m.LAST_LOGIN_TIME); })
                .ForMember(d => d.LOGIN_COUNT, opt => { opt.MapFrom(m => (m.LOGIN_COUNT == 0) ? 1 : m.LOGIN_COUNT); })
                .ForMember(d => d.REGION, opt => { opt.MapFrom(m => (m.REGION == null) ? "1" : m.REGION); })
                ;
            CreateMap<fa_user, TUser>()
                .ForMember(d => d.RoleAllIDStr, opt => { opt.MapFrom(m =>string.Join(",",m.fa_role.Select(x=>x.ID))); })
                .ForMember(d => d.RoleAllNameStr, opt => { opt.MapFrom(m =>string.Join(",",m.fa_role.Select(x=>x.NAME))); })
                .ForMember(d => d.AllModuleIdStr, opt => { opt.MapFrom(m =>string.Join(",",m.fa_module.Select(x=>x.ID))); })
                .ForMember(d => d.UserDistrict, opt => { opt.MapFrom(m =>string.Join(",",m.fa_district1.Select(x=>x.NAME))); })
                ;
        }
    }
}
