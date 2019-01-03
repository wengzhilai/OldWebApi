
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
using AutoMapper;
using System.Data.Entity.Validation;

namespace ProServer
{
    public partial class Z_Login : IZ_Login
    {
        #region 默认方法

        /// <summary>
        /// 修改登录
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="inEnt">实体类</param>
        /// <param name="allPar">更新的参数</param>
        /// <returns>修改登录</returns>
        public object Login_Save(string loginKey, ref ProInterface.ErrorInfo err, ProInterface.Models.LOGIN inEnt, IList<string> allPar)
        {
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = Login_Save(db, loginKey,ref err, inEnt, allPar);
                    db.SaveChanges();
                    return ent;
                }
                catch (DbEntityValidationException e)
                {
                    err.IsError = true;
                    err.Message = Fun.GetDbEntityErrMess(e);
                    return null;
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    return null;
                }
            }
        }
        public object Login_Save(DBEntities db, string loginKey, ref ProInterface.ErrorInfo err, ProInterface.Models.LOGIN inEnt, IList<string> allPar)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            var ent = db.fa_login.SingleOrDefault(a => a.ID == inEnt.ID);
            bool isAdd = false;
            if (ent == null)
            {
                isAdd = true;
                ent = Mapper.Map<LOGIN, fa_login>(inEnt);
                ent.ID = Fun.GetCurrvalSeqID<fa_login>();
            }
            else
            {
                ent = Fun.ClassToCopy<ProInterface.Models.LOGIN, fa_login>(inEnt, ent, allPar);
            }
            if (isAdd)
            {
                ent = db.fa_login.Add(ent);
            }
            GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
            return ent;
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="keyId">主键ID</param>
        /// <returns>查询一条</returns>
        public ProInterface.Models.LOGIN Login_SingleId(string loginKey, ref ProInterface.ErrorInfo err, int keyId)
        {

            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                var ent = db.fa_login.SingleOrDefault(x => x.ID == keyId);
                var reEnt = new ProInterface.Models.LOGIN();
                if (ent != null)
                {
                    reEnt = Fun.ClassToCopy<fa_login, ProInterface.Models.LOGIN>(ent);
                }
                return reEnt;
            }
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <returns>查询一条</returns>
        public ProInterface.Models.LOGIN Login_Single(string loginKey, ref ProInterface.ErrorInfo err, string whereLambda)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                IList<fa_login> content = new List<fa_login>();
                Expression<Func<fa_login, bool>> whereFunc;
                try
                {
                    whereFunc = StringToLambda.LambdaParser.Parse<Func<fa_login, bool>>(whereLambda);
                }
                catch
                {
                    err.IsError = true;
                    err.Message = "条件表态式有误";
                    return null;
                }
                var reEnt = db.fa_login.Where(whereFunc).ToList();
                if (reEnt.Count > 0)
                {
                    return Fun.ClassToCopy<fa_login, ProInterface.Models.LOGIN>(reEnt[0]);
                }
                return null;
            }
        }

        /// <summary>
        /// 删除登录
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="keyId">删除登录</param>
        /// <returns>删除登录</returns>
        public bool Login_Delete(string loginKey, ref ProInterface.ErrorInfo err, int keyId)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_login.SingleOrDefault(a => a.ID == keyId);
                    db.fa_login.Remove(ent);

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
        /// <summary>
        /// 满足条件记录数
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <returns></returns>
        public int Login_Count(string loginKey, ref ProInterface.ErrorInfo err, string whereLambda)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return 0;
            using (DBEntities db = new DBEntities())
            {
                IList<fa_login> content = new List<fa_login>();
                Expression<Func<fa_login, bool>> whereFunc;
                try
                {
                    if (whereLambda == null || whereLambda.Trim() == "")
                    {
                        return db.fa_login.Count();
                    }
                    whereFunc = StringToLambda.LambdaParser.Parse<Func<fa_login, bool>>(whereLambda);
                    return db.fa_login.Where(whereFunc).Count();
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
        public IList<ProInterface.Models.LOGIN> Login_Where(string loginKey, ref ProInterface.ErrorInfo err, int pageIndex, int pageSize, string whereLambda, string orderField, string orderBy)
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
                var allList = db.fa_login.AsQueryable();
                if (whereLambda != null && whereLambda != "")
                {
                    try
                    {
                        Expression<Func<fa_login, bool>> whereFunc = StringToLambda.LambdaParser.Parse<Func<fa_login, bool>>(whereLambda);
                        allList = db.fa_login.Where(whereFunc);
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
                return Fun.ClassListToCopy<fa_login, ProInterface.Models.LOGIN>(content);
            }
        }


        #endregion
    }
}
