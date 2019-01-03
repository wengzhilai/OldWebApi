using AutoMapper;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<ROLE_CONFIG, fa_role_config>();
            CreateMap<fa_role_config, ROLE_CONFIG>();


            CreateMap<TRole, fa_role>()
                .ForMember(d => d.TYPE, opt => { opt.MapFrom(m => m.TYPE); })
                ;
            CreateMap<fa_role, TRole>()
                .ForMember(d => d.ModuleAllStr, opt => { opt.MapFrom(m =>string.Join(",",m.fa_module.Select(x=>x.ID))); })
                .ForMember(d =>
                d.RoleConfigs,
                opt =>
                {
                    opt.MapFrom(m => Mapper.Map<List<fa_role_config>>(m.fa_role_config.ToList()));
                });

        }
    }
}
