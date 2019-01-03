using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserInfoApi
    {
        /// <summary>
        /// 注册账号
        /// <para>1、添加登录工号 </para>
        /// <para>2、添加用户</para>
        /// <para>3、para:是用户的父辈信息</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns>Message 为添加成功的ID</returns>
        ErrorInfo UserInfoReg(ref ErrorInfo err, ApiRequesSaveEntityBean<FaUserInfo> inEnt);

        /// <summary>
        /// 搜索用户
        /// <para>para=>keyWord:关键字</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt">id:家族ID</param>
        /// <returns></returns>
        ApiPagingDataBean<FaUserInfo> UserInfoList(ref ErrorInfo err, ApiRequesPageBean<ApiPagingDataBean<FaUserInfo>> inEnt);


        /// <summary>
        /// 获取单个用户信息,可以指定ID，如不指定，表示获取自己
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FaUserInfo UserInfoSingle(ref ErrorInfo err, ApiRequesEntityBean<FaUserInfo> inEnt);


        /// <summary>
        /// 获取单个用户信息,可以指定ID，如不指定，表示获取自己
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ApiPagingDataBean<FaUserInfo> UserInfoSingleByName(ref ErrorInfo err, ApiRequesEntityBean<FaUserInfo> inEnt);


        /// <summary>
        /// 获取单个用户的父亲信息,可以指定ID，如不指定，表示获取自己父亲的
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FaUserInfo UserInfoFatherSingle(ref ErrorInfo err, ApiRequesEntityBean<FaUserInfo> inEnt);

        /// <summary>
        /// 保存用户父亲信息
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FaUserInfo UserInfoFatherSave(ref ErrorInfo err, ApiRequesSaveEntityBean<FaUserInfo> inEnt);

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FaUserInfo UserInfoSave(ref ErrorInfo err, ApiRequesSaveEntityBean<FaUserInfo> inEnt);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo UserInfoDelete(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);

        /// <summary>
        /// 添加朋友
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FaUserInfo UserInfoAddFriend(ref ErrorInfo err, ApiRequesSaveEntityBean<FaUserInfo> inEnt);

        /// <summary>
        /// 批量添加用儿子
        /// <para>userId:传入的用户</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns>成功添加数</returns>
        string UserInfoAddMultiSon(ref ErrorInfo err, ApiRequesSaveEntityBean<string> inEnt);

        /// <summary>
        /// 获取用户的关系图
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FaUserInfoRelative UserInfoRelative(ref ErrorInfo err, ApiRequesEntityBean<FaUserInfoRelative> inEnt);
    }
}
