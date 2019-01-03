using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServer.MapperCfg
{
    public class Configuration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<Profiles.DistrictProfile>();
                cfg.AddProfile<Profiles.ElderProfile>();
                cfg.AddProfile<Profiles.FamilyProfile>();
                cfg.AddProfile<Profiles.LoginProfile>();
                cfg.AddProfile<Profiles.RoleProfile>();
                cfg.AddProfile<Profiles.UserEventProfile>();
                cfg.AddProfile<Profiles.UserInfoProfile>();
                cfg.AddProfile<Profiles.UserProfile>();
            });
        }
    }
}
