
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Reflection;
using System.Text;
using ProInterface.Models;
using ProInterface;
using System.Linq.Expressions;
using LINQExtensions;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Validation;

namespace ProServer
{
    public partial class Service : IRole
    {
        public bool RoleSave(string loginKey, ref ErrorInfo err, TRole inEnt, IList<string> allPar)
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
                        ent = Fun.ClassToCopy<ProInterface.Models.ROLE, fa_role>(inEnt);
                        ent.ID = Fun.GetSeqID<fa_role>();
                    }
                    else
                    {
                        ent = Fun.ClassToCopy<ProInterface.Models.ROLE, fa_role>(inEnt, ent, allPar);
                    }

                    ent.fa_module.Clear();
                    IList<int> moduleID = inEnt.ModuleAllStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    ent.fa_module = db.fa_module.Where(x => moduleID.Contains(x.ID)).ToList();
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
                    db.SaveChanges();
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
                var reEnt = db.fa_role.Where(x=>x.ID==id).ToList();
                TRole tmp = new TRole();
                IList<KV> allPara = new List<KV>();

                if (reEnt.Count > 0)
                {
                    tmp = Fun.ClassToCopy<fa_role, ProInterface.Models.TRole>(reEnt[0]);
                    tmp.ModuleAllStr = ",";
                    foreach(var t in reEnt[0].fa_module.ToList())
                    {
                        tmp.ModuleAllStr += t.ID + ",";
                    }
                    tmp.RoleConfigs = Fun.ClassListToCopy<fa_role_config, ROLE_CONFIG>(reEnt[0].fa_role_config.ToList());
                }
                //添加
                foreach (var t in allPara)
                {
                    var cfg=tmp.RoleConfigs.SingleOrDefault(x => x.NAME == t.K);
                    if (cfg == null)
                    {
                        tmp.RoleConfigs.Add(new ROLE_CONFIG { NAME = t.K, REMARK = t.V });
                    }
                    else
                    {
                        cfg.REMARK = t.V;
                    }
                }
                //删除
                foreach (var t in tmp.RoleConfigs)
                {
                    if (allPara.SingleOrDefault(x => x.K == t.NAME) == null)
                    {
                        tmp.RoleConfigs.Remove(t);
                    }
                }
                tmp.RoleConfigsStr = JSON.DecodeToStr(tmp.RoleConfigs);
                return tmp;
            }
        }


        public bool RoleDelete(string loginKey, ref ErrorInfo err, int id)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_role.SingleOrDefault(a => a.ID == id);
                    ent.fa_bulletin.Clear();
                    ent.fa_flow_flownode_flow.Clear();
                    ent.fa_function.Clear();
                    ent.fa_module.Clear();
                    ent.fa_user.Clear();
                    //ent.YL_APP_MEUN.Clear();
                    foreach (var t in ent.fa_role_config.ToList())
                    {
                        db.fa_role_config.Remove(t);
                    }
                    db.fa_role.Remove(ent);
                    db.SaveChanges();
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Delete);
                    return true;
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    return false;
                }
            }
        }

        public IList<SelectListItem> RoleGetNoAuthority(int roleId, int queryId)
        {
            using (DBEntities db = new DBEntities())
            {
                var queryEnt = db.fa_query.SingleOrDefault(x => x.ID == queryId);
                IList<SelectListItem> reEnt = new List<SelectListItem>();
                IList<QueryRowBtn> rb = new List<QueryRowBtn>();
                IList<QueryRowBtn> hb = new List<QueryRowBtn>();
                try
                {
                    if (!string.IsNullOrEmpty(queryEnt.ROWS_BTN))
                    {
                        rb = JSON.EncodeToEntity<IList<QueryRowBtn>>(queryEnt.ROWS_BTN);
                    }
                }
                catch { }
                try
                {
                    if (!string.IsNullOrEmpty(queryEnt.HEARD_BTN))
                    {
                        hb = JSON.EncodeToEntity<IList<QueryRowBtn>>(queryEnt.HEARD_BTN);
                    }
                }
                catch { }
                foreach (var t in hb)
                {
                    rb.Add(t);
                }

                if (queryEnt.AUTO_LOAD != 1)
                {
                    rb.Add(new QueryRowBtn { Name = "查询" });
                }



                reEnt = rb.Select(x => new SelectListItem { Value = x.Name, Text = x.Name, Selected = false }).ToList();
                var roleAuth = db.fa_role_query_authority.SingleOrDefault(x => x.ROLE_ID == roleId && x.QUERY_ID == queryId);
                if (roleAuth != null && roleAuth.NO_AUTHORITY!=null)
                {
                    foreach (var t in roleAuth.NO_AUTHORITY.Split(','))
                    {
                        var tmp = reEnt.Where(x => x.Value == t).ToList();
                        foreach (var x in tmp)
                        {
                            x.Selected = true;
                        }
                    }
                }
                return reEnt;
            }
        }


        public bool RoleSaveNoAuthority(string loginKey, ref ErrorInfo err, int roleId, int queryId, string AuthArr)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                var roleAuth = db.fa_role_query_authority.SingleOrDefault(x => x.ROLE_ID == roleId && x.QUERY_ID == queryId);
                if (roleAuth == null)
                {
                    db.fa_role_query_authority.Add(new fa_role_query_authority { NO_AUTHORITY = AuthArr, QUERY_ID = queryId, ROLE_ID = roleId });
                }
                else
                {
                    roleAuth.NO_AUTHORITY = AuthArr;
                }
                db.SaveChanges();
            }
            return true;
        }
    }
}
