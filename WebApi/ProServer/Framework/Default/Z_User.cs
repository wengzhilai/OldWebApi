
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

namespace ProServer
{
    public partial class Service : IZ_User
    {
        #region 默认方法

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <returns>返回满足条件的泛型</returns>
        public IList<ProInterface.Models.USER> User_FindAll(string loginKey, ref ProInterface.ErrorInfo err)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                var allList = db.fa_user.ToList();
                return Fun.ClassListToCopy<fa_user, ProInterface.Models.USER>(allList);
            }
        }

        /// <summary>
        /// 添加系统用户
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="inEnt">实体类</param>
        /// <returns>添加系统用户</returns>
        public object User_Add(string loginKey, ref ProInterface.ErrorInfo err, ProInterface.Models.USER inEnt)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                fa_user reEnt = Fun.ClassToCopy<ProInterface.Models.USER, fa_user>(inEnt);
                reEnt = db.fa_user.Add(reEnt);
                try
                {
                    db.SaveChanges();
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Add);
                    return reEnt.ID;
                }
                catch(Exception e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    err.Excep = e;
                    return null;
                }
            }
        }
        /// <summary>
        /// 修改系统用户
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="inEnt">实体类</param>
        /// <returns>修改系统用户</returns>
        public bool User_Edit(string loginKey, ref ProInterface.ErrorInfo err, ProInterface.Models.USER inEnt)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_user.SingleOrDefault(a => a.ID == inEnt.ID);
                    ent = Fun.ClassToCopy<ProInterface.Models.USER, fa_user>(inEnt, ent);

                    db.SaveChanges();
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
                    return true;
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    err.Excep = e;
                    return false;
                }
            }
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="keyId">主键ID</param>
        /// <returns>查询一条</returns>
        public ProInterface.Models.USER User_SingleId(string loginKey, ref ProInterface.ErrorInfo err, int keyId)
        {

            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                var ent = db.fa_user.SingleOrDefault(x => x.ID == keyId);
                var reEnt = new ProInterface.Models.USER();
                if (ent != null)
                {
                    reEnt = Fun.ClassToCopy<fa_user, ProInterface.Models.USER>(ent);
                }
                return reEnt;
            }
        }


        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="roleId">删除系统用户</param>
        /// <returns>删除系统用户</returns>
        public bool User_Delete(string loginKey, ref ProInterface.ErrorInfo err, int entId)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_user.SingleOrDefault(a => a.ID == entId);
                    db.fa_user.Remove(ent);

                    db.SaveChanges();
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Delete);
                    return true;
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    err.Excep = e;
                    return false;
                }
            }
        }
        /// <summary>
        /// 满足条件记录数
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <returns></returns>
        public int User_Count(string loginKey, ref ProInterface.ErrorInfo err, string whereLambda)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return 0;
            using (DBEntities db = new DBEntities())
            {
                IList<USER> content = new List<USER>();
                Expression<Func<fa_user, bool>> whereFunc;
                try
                {
                    if (whereLambda == null || whereLambda.Trim() == "")
                    {
                        return db.fa_user.Count();
                    }
                    whereFunc = StringToLambda.LambdaParser.Parse<Func<fa_user, bool>>(whereLambda);
                    return db.fa_user.Where(whereFunc).Count();
                }
                catch
                {
                    err.IsError = true;
                    err.Message = "条件表态式有误";
                    return 0;
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns>返回满足条件的泛型</returns>
        public IList<ProInterface.Models.USER> User_Where(string loginKey, ref ProInterface.ErrorInfo err, int pageIndex, int pageSize, string whereLambda, string orderField, string orderBy)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 1;
            int skipCount = (pageIndex - 1) * pageSize;
            if (orderField == null || orderField == "")
            {
                err.IsError = true;
                err.Message = "排序表态式不能为空";
                return null;
            }
            using (DBEntities db = new DBEntities())
            {
                var allList = db.fa_user.AsQueryable();
                if (whereLambda != null && whereLambda != "")
                {
                    try
                    {
                        Expression<Func<fa_user, bool>> whereFunc = StringToLambda.LambdaParser.Parse<Func<fa_user, bool>>(whereLambda);
                        allList = db.fa_user.Where(whereFunc);
                    }
                    catch
                    {
                        err.IsError = true;
                        err.Message = "条件表态式有误";
                        return null;
                    }
                }

                if (orderBy == "asc")
                {
                    allList = StringFieldNameSortingSupport.OrderBy(allList, orderField);
                }
                else
                {
                    allList = StringFieldNameSortingSupport.OrderByDescending(allList, orderField);
                }

                var content = allList.Skip(skipCount).Take(pageSize).ToList();
                return Fun.ClassListToCopy<fa_user, ProInterface.Models.USER>(content);
            }           
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <returns>查询一条</returns>
        public ProInterface.Models.USER User_Single(string loginKey, ref ProInterface.ErrorInfo err, string whereLambda)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                IList<USER> content = new List<USER>();
                Expression<Func<fa_user, bool>> whereFunc;
                try
                {
                    whereFunc = StringToLambda.LambdaParser.Parse<Func<fa_user, bool>>(whereLambda);
                }
                catch
                {
                    err.IsError = true;
                    err.Message = "条件表态式有误";
                    return null;
                }
                var reEnt = db.fa_user.Where(whereFunc).ToList();
                if (reEnt.Count>0)
                {
                    return Fun.ClassToCopy<fa_user, ProInterface.Models.USER>(reEnt[0]);
                }
                return null;
            }
        }

        #endregion

    }
}
