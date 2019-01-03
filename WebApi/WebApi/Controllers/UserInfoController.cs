using System.Web.Http;
using ProServer;
using ProInterface.Models;
using ProInterface;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
namespace WebApi.Controllers
{

    /// <summary>
    /// 家主要接口
    /// </summary>
    [RoutePrefix("api/UserInfo")]
    public class UserInfoController : ApiController
    {

        private ServeWeb api;
        /// <summary>
        /// 构造：用户登录
        /// </summary>
        public UserInfoController()
        {
            api = new ServeWeb();
        }

        /// <summary>
        /// 注册账号
        /// <para>1、添加登录工号 </para>
        /// <para>2、添加用户</para>
        /// <para>3、para:是用户的父辈信息</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns>Message 为添加成功的ID</returns>
        [Route("UserInfoReg")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoReg(ApiRequesSaveEntityBean<FaUserInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func(api.UserInfoApi.UserInfoReg, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


        /// <summary>
        /// 根据关键字，查询所有用户
        /// <para>para=>keyWord:关键字</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoList")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoList(ApiRequesPageBean<ApiPagingDataBean<FaUserInfo>>  inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ApiPagingDataBean<FaUserInfo>>.Func(api.UserInfoApi.UserInfoList, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 添加朋友
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoAddFriend")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoAddFriend(ApiRequesSaveEntityBean<FaUserInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func<FaUserInfo>(api.UserInfoApi.UserInfoAddFriend, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 获取用户的关系图
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoRelative")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoRelative(ApiRequesPageBean<FaUserInfoRelative> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfoRelative>.Func(api.UserInfoApi.UserInfoRelative, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 保存用户信息,如果login_name为空，表示不创建登录工号
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoSave")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoSave(ApiRequesSaveEntityBean<FaUserInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            

            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func(api.UserInfoApi.UserInfoSave, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


        /// <summary>
        /// 获取单个用户信息,可以指定ID，如不指定，表示获取自己
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoSingle")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoSingle(ApiRequesEntityBean<FaUserInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func(api.UserInfoApi.UserInfoSingle, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoSingleByName")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoSingleByName(ApiRequesEntityBean<FaUserInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func(api.UserInfoApi.UserInfoSingleByName, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


        /// <summary>
        /// 保存用户父亲信息
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoFatherSave")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoFatherSave(ApiRequesSaveEntityBean<FaUserInfo> inEnt)
        {
            if(Request.Method.Method== "OPTIONS") return null ;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func(api.UserInfoApi.UserInfoFatherSave, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 获取单个用户父亲信息,可以指定ID，如不指定，表示获取自己
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoFatherSingle")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoFatherSingle(ApiRequesEntityBean<FaUserInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<FaUserInfo>.Func(api.UserInfoApi.UserInfoFatherSingle, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 批量添加用儿子
        /// <para>userId:传入的用户</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoAddMultiSon")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoAddMultiSon(ApiRequesSaveEntityBean<string> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<string>.Func<string>(api.UserInfoApi.UserInfoAddMultiSon, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 删除用户
        /// <para>id:删除的用户</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserInfoDelete")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserInfoDelete(ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.UserInfoApi.UserInfoDelete, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


    }
}
