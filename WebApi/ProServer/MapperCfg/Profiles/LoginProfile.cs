using AutoMapper;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LOGIN, fa_login>()
                .ForMember(d => d.PHONE_NO, opt => { opt.MapFrom(m => m.LOGIN_NAME); })
                .ForMember(d => d.PASSWORD, opt => { opt.MapFrom(m => string.IsNullOrEmpty(m.PASSWORD) ?ProInterface.AppSet.DefaultPwd.Md5(): m.PASSWORD.Md5()); })
                .ForMember(d => d.IS_LOCKED, opt => { opt.MapFrom(m => m.IS_LOCKED==null ?0: m.IS_LOCKED); })
                .ForMember(d => d.FAIL_COUNT, opt => { opt.MapFrom(m => m.FAIL_COUNT == null ?0: m.FAIL_COUNT); })
                .ForMember(d => d.REGION, opt => { opt.UseValue(1); })
                ;
            CreateMap<fa_login,LOGIN>()
                .ForMember(d => d.EMAIL_ADDR, opt => { opt.MapFrom(m => m.EMAIL_ADDR); });

        }
    }
}
