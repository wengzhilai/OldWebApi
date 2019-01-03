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

namespace ProServer.ApiWeb
{
    public class PublicApi : IPublicApi
    {
        public ErrorInfo CheckToken(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            GlobalUser gu = Global.GetUser(inEnt.authToken);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "不存在";
                return null;
            }
            else {
                return new ErrorInfo();
            }
        }

        public ErrorInfo GetChineseCalendar(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            DateTime dateTime = new DateTime();
            if (!DateTime.TryParse(inEnt.data.ToString(), out dateTime))
            {
                err.IsError = true;
                err.Message = "参数有误";
                return err;
            }
            var tempPar = inEnt.para.FirstOrDefault(x => x.K == "inType");
            
            try
            {
                if (tempPar == null || tempPar.V == "china")
                {
                    ChineseCalendar cc = new ChineseCalendar(dateTime.Year, dateTime.Month, dateTime.Day, false);
                    err.Message = cc.Date.ToString("yyyy-MM-dd");
                }
                else
                {
                    ChineseCalendar cc = new ChineseCalendar(dateTime);
                    err.Message = cc.ChineseDateString;
                }
            }
            catch {
                err.Message = dateTime.ToString("yyyy-MM-dd");
            }

            return err;
        }

        public ErrorInfo FileDel(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt)
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
                var file = db.fa_files.SingleOrDefault(x => x.ID == inEnt.id);
                if (file == null)
                {
                    err.IsError = true;
                    err.Message = "文件对象不存在";
                    return null;
                }
                
                db.fa_files.Remove(file);
                if (Fun.DBEntitiesCommit(db, ref err))
                {
                    var allPath = Fun.UrlToAllPath(file.URL);
                    if (System.IO.File.Exists(allPath))
                    {
                        System.IO.File.Delete(allPath);
                    }
                    reEnt.IsError = false;
                }
            }
            return reEnt;
        }

        public ApiPagingDataBean<FILES> FileList(ref ErrorInfo err, ApiRequesPageBean<ApiPagingDataBean<FILES>> inEnt)
        {
            GlobalUser gu = Global.GetUser(inEnt.authToken);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            if (inEnt.pageSize == 0) inEnt.pageSize = 10;
            if (inEnt == null)
            {
                err.IsError = true;
                err.Message = "参数有误";
                return null;
            }
            ApiPagingDataBean<FILES> reEnt = new ApiPagingDataBean<FILES>();

            int skip = 0;
            if (inEnt.currentPage > 1)
            {
                skip = (inEnt.currentPage - 1) * inEnt.pageSize;
            }
            using (DBEntities db = new DBEntities())
            {
                var allData = db.fa_files.Where(x => x.USER_ID == gu.UserId).OrderByDescending(x => x.ID).AsEnumerable();

                #region 过虑条件
                if (inEnt.searchKey != null)
                {
                    foreach (var filter in inEnt.searchKey)
                    {
                        allData = Fun.GetListWhere(allData, filter.K, filter.T, filter.V, ref err);
                    }
                }
                #endregion

                #region 排序

                if (allData == null)
                {
                    err.IsError = true;
                    return null;
                }
                foreach (var filter in inEnt.orderBy)
                {
                    allData = Fun.GetListOrder(allData, filter.K, filter.V, ref err);
                }
                #endregion

                var allList = allData.Skip(skip).Take(inEnt.pageSize).ToList();

                reEnt.currentPage = inEnt.currentPage;
                reEnt.pageSize = inEnt.pageSize;
                reEnt.totalCount = allData.Count();
                reEnt.totalPage = reEnt.totalCount / reEnt.pageSize;
                if (reEnt.totalCount % reEnt.pageSize != 0) reEnt.totalPage++;
                IList<FILES> reList = new List<FILES>();
                foreach (var t in allList)
                {
                    var single = Fun.ClassToCopy<fa_files, FILES>(t);
                    reList.Add(single);
                }
                reEnt.data = reList;
            }
            return reEnt;
        }

        public FILES FileUp(ref ErrorInfo err, ApiRequesEntityBean<FILES> inEnt)
        {
            throw new NotImplementedException();
        }

        public ErrorInfo SendCode(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            string phone = "";
            var tmpPar = inEnt.para.FirstOrDefault(x => x.K == "phone");
            if (tmpPar != null)
            {
                phone = tmpPar.V;
            }

            if (string.IsNullOrEmpty(phone))
            {
                err.IsError = true;
                err.Message = "电话号码不能为空";
                return err;
            }
            return SendCode( inEnt.authToken, ref err, phone);
        }


        private ErrorInfo SendCode(string loginKey, ref ErrorInfo err, string phone)
        {
            ErrorInfo reEnt = new ErrorInfo();
            if (string.IsNullOrEmpty(phone))
            {
                err.IsError = true;
                err.Message = "电话号码不能为空";
                return err;
            }

            if (!phone.IsOnlyNumber() || phone.Length != 11)
            {
                err.IsError = true;
                err.Message = "电话号码格式不正确";
                return err;
            }

            using (DBEntities db = new DBEntities())
            {
                var code = PicFun.ValidateMake(4);

                var login = db.fa_login.SingleOrDefault(x => x.LOGIN_NAME == phone);
                if (login != null)
                {
                    login.VERIFY_CODE = code;
                }

                fa_sms_send ent = new fa_sms_send()
                {
                    GUID = Guid.NewGuid().ToString().Replace("-", ""),
                    ADD_TIME = DateTime.Now,
                    CONTENT = code,
                    STAUTS = "成功",
                    PHONE_NO = phone
                };
                if (SmsSendCode(loginKey, phone, code))
                {
                    reEnt.Message = "发送成功";
                }
                else
                {
                    reEnt.IsError = true;
                    reEnt.Message = "短信服务已欠费，请联系管理员";
                }
                db.fa_sms_send.Add(ent);
                Fun.DBEntitiesCommit(db, ref err);
                return reEnt;
            }
        }

        bool IPublicApi.SmsSendCode(string loginKey, string mobile, string code)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        private bool SmsSendCode(string loginKey, string mobile, string code)
        {
            string tpl_value = HttpUtility.UrlEncode(
           HttpUtility.UrlEncode("#code#", Encoding.UTF8) + "=" +
           HttpUtility.UrlEncode(code, Encoding.UTF8), Encoding.UTF8);
            string data_tpl_sms = "apikey=51f88df9eedd2e9565f5f3a9417c45df&mobile=" + mobile + "&tpl_id=1323633&tpl_value=" + tpl_value;
            string questStr = "";
            if (Fun.HttpPostEncoded("https://sms.yunpian.com/v2/sms/tpl_single_send.json", data_tpl_sms, ref questStr))
            {
                return true;
            }
            return false;
        }
    }
}
