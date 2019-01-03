using ProInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using ProInterface.Models;
using System.Reflection;
using System.Data.Entity.Validation;
using AutoMapper;
using System.Web;
using System.Text;
using ProServer.ApiAdmin;

namespace ProServer.ApiWeb
{
    public class FamilyApi : IFamilyApi
    {
        ServeWeb api;

        public FamilyApi(ServeWeb _serveWeb)
        {
            api = _serveWeb;
        }

       

        /* 2017-5-6 16:17:41 */
        public ApiPagingDataBean<FA_ELDER> OlderList(ref ErrorInfo err, ApiRequesPageBean<ApiPagingDataBean<FA_ELDER>> inEnt)
        {
            GlobalUser gu = Global.GetUser(inEnt.authToken);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            using (DBEntities db = new DBEntities())
            {
                ApiPagingDataBean<FA_ELDER> reEnt = new ApiPagingDataBean<FA_ELDER>();
                var allList = db.fa_elder.Where(x => x.FAMILY_ID == inEnt.id).OrderBy(x=>x.SORT).ToList();
                reEnt.data = Mapper.Map<List<FA_ELDER>>(allList);
                return reEnt;
            }
        }

        /* 2017-5-6 16:17:20 */
        public ErrorInfo OlderSave(ref ErrorInfo err, ApiRequesSaveEntityBean<List<FA_ELDER>> inEnt)
        {
            GlobalUser gu = Global.GetUser(inEnt.authToken);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            ErrorInfo reEnt = new ErrorInfo();

            using (DBEntities db = new DBEntities())
            {
                inEnt.entity = inEnt.entity.OrderBy(x => x.SORT).ToList();
                #region 添加新的和删除重复排序的
                foreach (var t in inEnt.entity)
                {
                    var nowEnt = db.fa_elder.Where(x => x.FAMILY_ID == t.FAMILY_ID && x.SORT == t.SORT).ToList();
                    if (nowEnt.Count() > 0)
                    {
                        #region 删除重复排序
                        for (var i = 1; i < nowEnt.Count; i++)
                        {
                            db.fa_elder.Remove(nowEnt[i]);
                        } 
                        #endregion
                        nowEnt[0].NAME = t.NAME;
                    }
                    else
                    {
                        db.fa_elder.Add(Mapper.Map<fa_elder>(t));
                    }
                }
                #endregion

                #region 删除数据
                if (inEnt.entity.Count() > 0)
                {
                    var familyId = inEnt.entity[0].FAMILY_ID;
                    var allList = db.fa_elder.Where(x => x.FAMILY_ID == familyId).ToList();
                    foreach (var t in allList)
                    {
                        var delEnt = inEnt.entity.Where(x => x.FAMILY_ID == t.FAMILY_ID && x.SORT == t.SORT).ToList();
                        #region 如果有重复的世，将不做任何操作
                        if (delEnt.Count() > 1)
                        {
                            err.IsError = true;
                            err.Message = string.Format("第{0}世有重复数据", t.SORT);
                            return err;
                        }
                        #endregion
                        else if (delEnt.Count() == 0)
                        {
                            db.fa_elder.Remove(t);
                        }
                    }
                } 
                #endregion

                Fun.DBEntitiesCommit(db, ref err);
                return reEnt;
            }
        }

        
    }
}
