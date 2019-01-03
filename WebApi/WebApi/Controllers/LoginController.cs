using System;

using System.Web.Http;
using ProServer;
using ProInterface.Models;
using ProInterface;
using System.Web.Security;
using System.Threading.Tasks;
namespace WebApi.Controllers
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        private ServeWeb api;
        /// <summary>
        /// 构造：用户登录
        /// </summary>
        public LoginController()
        {
            api = new ServeWeb();
        }

        /// <summary>
        /// 注销用户登录状态
        /// <para>清除用户的缓存状态</para>
        /// <para>记录退出日志</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("LoginOut")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> LoginOut(ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.UserApi.LoginOut, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("LoginReg")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> LoginReg(ApiLogingBean inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.UserApi.LoginReg, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }


        /// <summary>
        /// 重置用户密码
        /// <para>VerifyCode:短信验证码</para>
        /// <para>LoginName:登录名</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("ResetPassword")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> ResetPassword(ApiRequesEntityBean<ErrorInfo> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<ErrorInfo>.Func(api.UserApi.ResetPassword, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserEditPwd")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserEditPwd(ApiRequesSaveEntityBean<string> inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;
            dynamic reEnt = await Task.Run(() => Fun<string>.Func(api.UserApi.UserEditPwd, ref err, inEnt));
            if (err.IsError) return err;
            return reEnt;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("UserLogin")]
        [HttpPost]
        [HttpOptions]
        public async Task<dynamic> UserLogin(ApiLogingBean inEnt)
        {
            if (Request.Method.Method == "OPTIONS") return null;
            ErrorInfo err = new ErrorInfo();
            Fun<ErrorInfo>.Valid(inEnt, ModelState, ref err);
            if (err.IsError) return err;

            dynamic reEnt = await Task.Run(()=> Fun<ErrorInfo>.Func(api.UserApi.UserLogin, ref err, inEnt));
            if (err.IsError) return err;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, inEnt.loginName, DateTime.Now,
                            DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", inEnt.loginName, reEnt.Guid),
                            FormsAuthentication.FormsCookiePath);
            //返回登录结果、用户信息、用户验证票据信息
            if (err.IsError) return err;

            reEnt.Ticket = FormsAuthentication.Encrypt(ticket);
            return reEnt;
        }

        



        

    }
}
