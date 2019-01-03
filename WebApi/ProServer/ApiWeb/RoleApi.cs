using ProInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using ProInterface.Models;
using System.Reflection;
using System.Data.Entity.Validation;
using AutoMapper;

namespace ProServer.ApiWeb
{
    public class RoleApi : IRoleApi
    {
        public bool RoleSave(string loginKey, ref ErrorInfo err, TRole inEnt, IList<string> allPar=null)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_role.SingleOrDefault(a => a.ID == inEnt.ID);
                    bool isAdd = false;
                    if (ent == null)
                    {
                        isAdd = true;
                        ent= Mapper.Map<fa_role>(inEnt);
                        ent.ID = Fun.GetSeqID<fa_role>();
                    }
                    else
                    {
                        ent = Fun.ClassToCopy<ProInterface.Models.ROLE, fa_role>(inEnt, ent, allPar);
                    }

                    ent.fa_module.Clear();
                    if (!string.IsNullOrEmpty(inEnt.ModuleAllStr))
                    {
                        IList<int> moduleID = inEnt.ModuleAllStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        ent.fa_module = db.fa_module.Where(x => moduleID.Contains(x.ID)).ToList();
                    }
                    if (!string.IsNullOrEmpty(inEnt.RoleConfigsStr))
                    {
                        inEnt.RoleConfigs = JSON.EncodeToEntity<IList<ROLE_CONFIG>>(inEnt.RoleConfigsStr);
                        foreach (var t in inEnt.RoleConfigs)
                        {
                            var cfg = ent.fa_role_config.SingleOrDefault(x => x.NAME == t.NAME);
                            if (cfg == null)
                            {
                                ent.fa_role_config.Add(new fa_role_config
                                {
                                    NAME = t.NAME,
                                    ROLE_ID = ent.ID,
                                    VALUE = t.VALUE
                                });
                            }
                            else
                            {
                                cfg.VALUE = t.VALUE;
                            }
                        }
                    }
                    foreach (var t in ent.fa_role_config.ToList())
                    {
                        if (inEnt.RoleConfigs.SingleOrDefault(x => x.NAME == t.NAME) == null)
                        {
                            db.fa_role_config.Remove(t);
                        }
                    }

                    if (isAdd)
                    {
                        db.fa_role.Add(ent);
                    }
                    Fun.DBEntitiesCommit(db, ref err);

                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    err.Excep = e;
                    return false;
                }
            }
        }

        public TRole RoleSingle(string loginKey, ref ErrorInfo err, int id)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                var reEnt = db.fa_role.SingleOrDefault(x => x.ID == id);
                TRole tmp = new TRole();
                IList<KV> allPara = new List<KV>();

                if (reEnt!=null)
                {
                    tmp = Mapper.Map<TRole>(reEnt);
                }
                return tmp;
            }
        }
    }
}
