using AutoMapper;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg.Profiles
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<FaUserInfo, fa_user_info>()
                ;
            CreateMap<fa_user_info, FaUserInfo>()
                .ForMember(d => d.RoleAllIDStr, opt => { opt.MapFrom(m => string.Join(",", m.fa_user.fa_role.Select(x => x.ID))); })
                .ForMember(d => d.RoleAllNameStr, opt => { opt.MapFrom(m => string.Join(",", m.fa_user.fa_role.Select(x => x.NAME))); })
                .ForMember(d => d.AllModuleIdStr, opt => { opt.MapFrom(m => string.Join(",", m.fa_user.fa_module.Select(x => x.ID))); })
                .ForMember(d => d.UserDistrict, opt => { opt.MapFrom(m => string.Join(",", m.fa_user.fa_district1.Select(x => x.NAME))); })
                .ForMember(d => d.Family, opt => { opt.MapFrom(m => Mapper.Map<FA_FAMILY>(m.fa_family)); })
                .ForMember(d => d.Elder, opt => { opt.MapFrom(m => Mapper.Map<FA_ELDER>(m.fa_elder)); })
                .ForMember(d => d.FriendList, opt => { opt.MapFrom(m => Mapper.Map<IList<USER>>(m.fa_user1.ToList())); })
                .ForMember(d => d.EventList, opt => { opt.MapFrom(m => Mapper.Map<IList<FA_USER_EVENT>>(m.fa_user_event.ToList())); })
                .ForMember(d => d.FatherName, opt => { opt.MapFrom(m => m.fa_user_info2 != null?m.fa_user_info2.fa_user.NAME:""); })
                .ForMember(d => d.NAME, opt => { opt.MapFrom(m =>m.fa_user.NAME); })
                ;

            CreateMap<TUser, FaUserInfo>();

            CreateMap<fa_user_info, FaUserInfoRelativeItem>()
                .ForMember(d => d.ElderId, opt => { opt.MapFrom(m =>m.ELDER_ID); })
                .ForMember(d => d.ElderName, opt => { opt.MapFrom(m =>m.fa_elder.NAME); })
                .ForMember(d => d.FatherId, opt => { opt.MapFrom(m =>m.FATHER_ID); })
                .ForMember(d => d.IcoUrl, opt => { opt.MapFrom(m =>m.fa_user.ICON_FILES_ID); })
                .ForMember(d => d.Id, opt => { opt.MapFrom(m =>m.ID); })
                .ForMember(d => d.Name, opt => { opt.MapFrom(m =>m.fa_user.NAME); })
                .ForMember(d => d.Sex, opt => { opt.MapFrom(m =>m.SEX); })
                ;
        }
    }
}
