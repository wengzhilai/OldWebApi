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
    [RoutePrefix("api/Family")]
    public class FamilyController : ApiController
    {

        private ServeWeb api;
        /// <summary>
        /// 构造：用户登录
        /// </summary>
        public FamilyController()
        {
            api = new ServeWeb();
        }

        /// <summary>
        /// 获取所有辈份
        /// <para>id:家族ID</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("OlderList")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> OlderList(ApiRequesPageBean<ApiPagingDataBean<FA_ELDER>> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ApiPagingDataBean<FA_ELDER>>.Func(api.FamilyApi.OlderList, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }
        /// <summary>
        /// 保存一个姓的所有辈分
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("OlderSave")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> OlderSave(ApiRequesSaveEntityBean<List<FA_ELDER>> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func<List<FA_ELDER>>(api.FamilyApi.OlderSave, ref err, inEnt));
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


    }
}
