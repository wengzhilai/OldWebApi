using AutoMapper;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg.Profiles
{
    public class FamilyProfile : Profile
    {
        public FamilyProfile()
        {
            CreateMap<FA_FAMILY, fa_family>()
                ;
            CreateMap<fa_family, FA_FAMILY>()
                ;
        }
    }
}
