using AutoMapper;
using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg.Profiles
{
    public class UserEventProfile : Profile
    {
        public UserEventProfile()
        {
            CreateMap<FA_USER_EVENT, fa_user_event>()
                ;
            CreateMap<fa_user_event, FA_USER_EVENT>()
                ;
        }
    }
}
