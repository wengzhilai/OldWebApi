using ProInterface.Models;
using System.Collections.Generic;

namespace ProInterface
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public interface IUserApi
    {
        /// <summary>
        /// 注册账号
        /// <para>1、添加登录工号 </para>
        /// <para>2、添加用户</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo LoginReg(ref ErrorInfo err, ApiLogingBean inEnt);
        /// <summary>
        /// 注销用户登录状态
        /// <para>清除用户的缓存状态</para>
        /// <para>记录退出日志</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo LoginOut(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);
        /// <summary>
        /// 用户登录
        /// <para>只验证用户账号</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        GlobalUser UserLogin(ref ErrorInfo err, ApiLogingBean inEnt);
        /// <summary>
        /// 重置用户密码
        /// <para>VerifyCode:短信验证码</para>
        /// <para>LoginName:登录名</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo ResetPassword(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);
        /// <summary>
        /// 修改用户密码
        /// <para>entity:旧密码</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo UserEditPwd(ref ErrorInfo err, ApiRequesSaveEntityBean<string> inEnt);

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <param name="allPar"></param>
        /// <returns></returns>
        TUser UserSave(object db, string loginKey, ref ErrorInfo err, TUser inEnt, IList<string> allPar);

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <param name="allPar"></param>
        /// <returns></returns>
        TUser UserAndLoginSave(object db, string loginKey, ref ErrorInfo err, TUser inEnt, IList<string> allPar);

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        TUser UserSingle(object db, string loginKey, ref ErrorInfo err, ApiRequesEntityBean<TUser> inEnt);
    }
}
