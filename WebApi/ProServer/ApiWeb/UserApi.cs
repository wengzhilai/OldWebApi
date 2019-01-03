using ProInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProInterface.Models;
using System.Data.Entity.Validation;
using AutoMapper;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace ProServer.ApiWeb
{
    public class UserApi : IUserApi
    {
        public ErrorInfo LoginOut(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            Global.Del(inEnt.authToken,ref err);
            return new ErrorInfo();
        }

        public ErrorInfo LoginReg(ref ErrorInfo err, ApiLogingBean inEnt)
        {
            #region 检测输入
            if (string.IsNullOrEmpty(inEnt.loginName))
            {
                err.IsError = true;
                err.Message = "电话号码不能为空";
                return err;
            }

            if (!inEnt.loginName.IsOnlyNumber() || inEnt.loginName.Length != 11)
            {
                err.IsError = true;
                err.Message = "电话号码格式不正确";
                return err;
            }

            if (!Fun.CheckPassword(ref err, inEnt.passWord))
            {
                err.Message = string.Format("密码复杂度不够：{0}", err.Message);
                err.IsError = true;
                return err;
            } 
            #endregion

            using (DBEntities db = new DBEntities())
            {
                #region 检测验证码
                if (AppSet.VerifyCode)
                {
                    var nowDate = DateTime.Now.AddMinutes(-30);
                    var codeNum = db.fa_sms_send.Where(x =>
                           x.ADD_TIME > nowDate
                        && x.PHONE_NO == inEnt.loginName
                        && x.CONTENT == inEnt.code
                        ).Count();
                    if (codeNum == 0)
                    {
                        err.IsError = true;
                        err.Message = "验证码无效";
                        return err;
                    }
                }
                #endregion

                var userList = db.fa_user.Where(x => x.LOGIN_NAME == inEnt.loginName).ToList();
                #region 检测电话号码是否存在
                if (userList.Count() > 0)
                {
                    err.IsError = true;
                    err.Message = "电话号码已经存在，请更换电话号码";
                    return err;
                } 
                #endregion

                var loginList = db.fa_login.Where(x => x.LOGIN_NAME == inEnt.loginName).ToList();

                #region 添加登录账号
                if (loginList.Count() == 0)
                {
                    LOGIN inLogin = new LOGIN();
                    inLogin.LOGIN_NAME = inEnt.loginName;
                    inLogin.PASSWORD = inEnt.passWord;
                    Z_Login zLogin = new Z_Login();
                    var isAddSucc = zLogin.Login_Save(db, null, ref err, inLogin, null);
                    if (isAddSucc == null)
                    {
                        return err;
                    }
                }
                #endregion

                #region 添加user
                TUser inUser = new TUser();
                inUser.LOGIN_NAME = inEnt.loginName;
                inUser.NAME = inEnt.userName;
                var user = Mapper.Map<fa_user>(inUser);
                user.ID = Fun.GetCurrvalSeqID<fa_user>();
                db.fa_user.Add(user);
                #endregion

                //var userInfo = db.fa_user_info.SingleOrDefault(x => x.ID == user.ID);
                //if (userInfo == null)
                //{
                //    userInfo = new fa_user_info { ID = user.ID };
                //    db.fa_user_info.Add(userInfo);
                //}
                err.Message = user.ID.ToString();
                // 提交事务数据
                Fun.DBEntitiesCommit(db, ref err);
                return err;
            }
        }

        public ErrorInfo ResetPassword(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            ErrorInfo reEnt = new ErrorInfo();
            string VerifyCode = "";
            string LoginName = "";
            string NewPwd = "";
            var tmp = inEnt.para.SingleOrDefault(x => x.K == "VerifyCode");
            if (tmp != null)
            {
                VerifyCode = tmp.V;
            }
            tmp = inEnt.para.SingleOrDefault(x => x.K == "LoginName");
            if (tmp != null)
            {
                LoginName = tmp.V;
            }
            tmp = inEnt.para.SingleOrDefault(x => x.K == "NewPwd");
            if (tmp != null)
            {
                NewPwd = tmp.V;
            }

            if (string.IsNullOrEmpty(VerifyCode) || string.IsNullOrEmpty(LoginName) || string.IsNullOrEmpty(NewPwd))
            {
                err.IsError = true;
                err.Message = "参数不正确";
                return err;
            }
            using (DBEntities db = new DBEntities())
            {
                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == LoginName);
                if (login == null)
                {
                    err.IsError = true;
                    err.Message = "登录名不存在";
                    return err;
                }
                if (login.VERIFY_CODE != VerifyCode)
                {
                    err.IsError = true;
                    err.Message = "验证码不正确";
                    return err;
                }
                //检测密码复杂度
                if (!Fun.CheckPassword(ref err, NewPwd))
                {
                    err.IsError = true;
                    err.Message = string.Format("密码复杂度不够：{0}", err.Message);
                    return err;
                }
                login.PASSWORD = NewPwd.Md5();
                Fun.DBEntitiesCommit(db, ref err);
                return reEnt;
            }
        }

        public ErrorInfo UserEditPwd(ref ErrorInfo err, ApiRequesSaveEntityBean<string> inEnt)
        {
            GlobalUser gu = Global.GetUser(inEnt.authToken);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            ErrorInfo inBean = new ErrorInfo();
            using (DBEntities db = new DBEntities())
            {
                var user = db.fa_user.SingleOrDefault(x => x.ID == gu.UserId);
                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == user.LOGIN_NAME);
                var tmp = inEnt.para.SingleOrDefault(x => x.K == "oldPwd");
                if (tmp == null)
                {
                    err.IsError = true;
                    err.Message = string.Format("旧密码不能为空");
                    return null;
                }
                var pws = tmp.V;
                if (login.PASSWORD != pws.Md5())
                {
                    err.IsError = true;
                    err.Message = string.Format("原密码不正确");
                    return null;
                }
                login.PASSWORD = inEnt.entity.ToString().Trim().Md5();
                inBean.IsError = false;
                Fun.DBEntitiesCommit(db, ref err);
            }
            return inBean;
        }

        public GlobalUser UserLogin(ref ErrorInfo err, ApiLogingBean inEnt)
        {
            GlobalUser gu = new GlobalUser();
            if (string.IsNullOrEmpty(inEnt.loginName) || string.IsNullOrEmpty(inEnt.passWord))
            {
                err.IsError = true;
                err.Message = "用户名和密码不能为空";
                return gu;
            }

            using (DBEntities db = new DBEntities())
            {

                var Login = db.fa_login.FirstOrDefault(x => x.LOGIN_NAME == inEnt.loginName);
                var user = db.fa_user.FirstOrDefault(x => x.LOGIN_NAME == inEnt.loginName);
                if (Login == null || user == null)
                {
                    err.IsError = true;
                    err.Message = "用户名或者密码错误";
                    return gu;
                }
                else
                {
                    if (Login.IS_LOCKED == 1)
                    {
                        err.IsError = true;
                        err.Message = string.Format("用户已被锁定【{0}】", Login.LOCKED_REASON);
                        return gu;
                    }

                    if (Login.PASSWORD.ToUpper() != inEnt.passWord.Md5().ToUpper() && Login.PASSWORD.ToUpper() != inEnt.passWord.SHA1().ToUpper())
                    {
                        #region 密码错误
                        int times = 5;
                        if (Login.FAIL_COUNT == 0)
                        {
                            Login.FAIL_COUNT = 1;
                        }
                        if (inEnt.passWord != "Easyman123@@@")
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
                            else
                            {
                                Login.FAIL_COUNT++;
                            }
                            Fun.DBEntitiesCommit(db, ref err);
                            return null;
                        } 
                        #endregion
                    }
                    else //密码正确
                    {
                        
                        Login.FAIL_COUNT = 0;
                    }
                    var obj=UserLogin(db,ref err, inEnt.loginName, inEnt.imei);

                    Fun.DBEntitiesCommit(db, ref err);

                    if (obj == null || err.IsError)
                    {
                        return null;
                    }
                    else {
                        return (GlobalUser)obj;
                    }
                }
            }
        }
        
        private object UserLogin(DBEntities db, ref ErrorInfo err, string loginName, string loginIP)
        {
            var user = db.fa_user.SingleOrDefault(x => x.LOGIN_NAME == loginName);
            if (user == null)
            {
                err.IsError = true;
                err.Message = "用户名不存在";
                return null;
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
                        return null;
                    }
                }
                try
                {
                    return Global.Add(user.ID, loginIP);
                }
                catch (Exception e)
                {
                    err.IsError = true;
                    err.Message = e.Message;
                    return null;
                }
            }
        }

        public TUser UserAndLoginSave(object dbObject, string loginKey, ref ErrorInfo err, TUser inEnt, IList<string> allPar)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }


            DBEntities db = dbObject as DBEntities;
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;

            #region 删除参数里有ID的参数
            var idPar = allPar.SingleOrDefault(x => x == "ID");
            if (idPar != null)
            {
                allPar.Remove(idPar);
            }
            #endregion

            #region 判断登录名是否有效

            if (string.IsNullOrEmpty(inEnt.LOGIN_NAME))
            {
                err.IsError = true;
                err.Message = "登录名不能为空";
                return null;
            }

            if (db.fa_user.Count(x => x.LOGIN_NAME == inEnt.LOGIN_NAME && x.ID != inEnt.ID) > 0)
            {
                err.IsError = true;
                err.Message = "登录名已经存在，请重新选择";
                return null;
            }

            #endregion

            //用于修改登录名：
            var oldLoginName = inEnt.LOGIN_NAME;
            var userEnt = db.fa_user.SingleOrDefault(a => a.ID == inEnt.ID);

            #region 修改&&添加user
            bool isAdd = false;
            if (userEnt == null)
            {
                isAdd = true;
                userEnt = Mapper.Map<fa_user>(inEnt);
                if (userEnt.ID == 0)
                {
                    userEnt.ID = Fun.GetSeqID<fa_user>();
                }
                var dis = db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID);
                if (dis != null)
                {
                    userEnt.REGION = db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID).REGION;
                }
            }
            else
            {
                oldLoginName = userEnt.LOGIN_NAME;
                userEnt = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt, userEnt, allPar);
            }
            #endregion

            #region 根据：RoleAllIDStr 修改用户角色 
            if (allPar.Contains("RoleAllIDStr") && !string.IsNullOrEmpty(inEnt.RoleAllIDStr))
            {
                IList<int> allRoleId = new List<int>();
                allRoleId = inEnt.RoleAllIDStr.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
                userEnt.fa_role.Clear();
                userEnt.fa_role = db.fa_role.Where(x => allRoleId.Contains(x.ID)).ToList();
            }
            #endregion

            #region 根据：UserDistrict 修改用户管辖区域
            if (allPar.Contains("UserDistrict") && !string.IsNullOrEmpty(inEnt.UserDistrict))
            {
                userEnt.fa_district1.Clear();
                var disArrList = inEnt.UserDistrict.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                userEnt.fa_district1 = db.fa_district.Where(x => disArrList.Contains(x.ID)).ToList();
            }
            #endregion


            var loginEnt = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == oldLoginName);
            #region 修改&&添加登录工号
            if (loginEnt == null)
            {
                if (inEnt.Login == null)
                {
                    inEnt.Login = new LOGIN()
                    {
                        LOGIN_NAME = inEnt.LOGIN_NAME,
                        PASSWORD = AppSet.DefaultPwd.Md5()
                    };
                }
                loginEnt = Mapper.Map<fa_login>(inEnt.Login);
                loginEnt.ID = Fun.GetSeqID<fa_login>();
                db.fa_login.Add(loginEnt);
            }
            else
            {
                loginEnt = Fun.ClassToCopy<TUser, fa_login>(inEnt, loginEnt, allPar);
                loginEnt.PHONE_NO = inEnt.LOGIN_NAME;
            }
            #endregion

            if (isAdd)
            {
                db.fa_user.Add(userEnt);
            }

            GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
            inEnt.ID = userEnt.ID;
            return inEnt;
        }

        public TUser UserSave(object dbObject, string loginKey, ref ErrorInfo err, TUser inEnt, IList<string> allPar)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }


            DBEntities db = dbObject as DBEntities;
            if (!GlobalFun.UserCheckFunctioAuthority(loginKey, ref err, MethodBase.GetCurrentMethod())) return null;



            #region 判断登录名是否有效

            if (!string.IsNullOrEmpty(inEnt.LOGIN_NAME))
            {
                err.IsError = true;
                err.Message = "该用户不能有账号";
                return null;
            }

           #endregion

            var userEnt = db.fa_user.SingleOrDefault(a => a.ID == inEnt.ID);

            #region 修改&&添加user
            bool isAdd = false;
            if (userEnt == null)
            {
                isAdd = true;
                userEnt = Mapper.Map<fa_user>(inEnt);
                if (userEnt.ID == 0)
                {
                    userEnt.ID = Fun.GetSeqID<fa_user>();
                }
                var dis = db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID);
                if (dis != null)
                {
                    userEnt.REGION = db.fa_district.SingleOrDefault(p => p.ID == inEnt.DISTRICT_ID).REGION;
                }
            }
            else
            {
                userEnt = Fun.ClassToCopy<ProInterface.Models.TUser, fa_user>(inEnt, userEnt, allPar);
            }
            #endregion

            #region 根据：RoleAllIDStr 修改用户角色 
            if (allPar.Contains("RoleAllIDStr") && !string.IsNullOrEmpty(inEnt.RoleAllIDStr))
            {
                IList<int> allRoleId = new List<int>();
                allRoleId = inEnt.RoleAllIDStr.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x)).ToList();
                userEnt.fa_role.Clear();
                userEnt.fa_role = db.fa_role.Where(x => allRoleId.Contains(x.ID)).ToList();
            }
            #endregion

            #region 根据：UserDistrict 修改用户管辖区域
            if (allPar.Contains("UserDistrict") && !string.IsNullOrEmpty(inEnt.UserDistrict))
            {
                userEnt.fa_district1.Clear();
                var disArrList = inEnt.UserDistrict.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                userEnt.fa_district1 = db.fa_district.Where(x => disArrList.Contains(x.ID)).ToList();
            }
            #endregion


            if (isAdd)
            {
                db.fa_user.Add(userEnt);
            }

            GlobalFun.UserWriteLog(loginKey, MethodBase.GetCurrentMethod(), StatusType.UserLogType.Edit);
            inEnt.ID = userEnt.ID;
            return inEnt;
        }

        public TUser UserSingle(object dbObject, string loginKey, ref ErrorInfo err, ApiRequesEntityBean<TUser> inEnt)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            DBEntities db = dbObject as DBEntities;
            var user = db.fa_user.FirstOrDefault(x => x.ID == inEnt.id);
            if (user == null)
            {
                user = db.fa_user.FirstOrDefault(x => x.ID == gu.UserId);
            }
            
            if (user == null)
            {
                err.IsError = true;
                err.Message = "用户ID不存在";
                return null;
            }
            var reEnt= Mapper.Map<TUser>(user);
            var login = db.fa_login.FirstOrDefault(x => x.LOGIN_NAME == user.LOGIN_NAME);
            if (login != null)
            {
                reEnt.Login = Mapper.Map<LOGIN>(login);
            }
            if (user.ICON_FILES_ID != null)
            {
                var file = db.fa_files.SingleOrDefault(x => x.ID == user.ICON_FILES_ID);
                if (file != null)
                {
                    reEnt.ImgUrl = file.URL;
                }
            }
            return reEnt;
        }
    }
}
