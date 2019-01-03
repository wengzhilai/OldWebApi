using ProInterface.Models;

namespace ProInterface
{
    /// <summary>
    /// 公共类，
    /// 处理上传文件和发送短信
    /// </summary>
    public interface IPublicApi
    {
        /// <summary>
        /// 阳历转阳历
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo GetChineseCalendar(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);
        /// <summary>
        /// 获取文件列表
        /// <para>只能获取自己上传的文件</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns>文件分页对象</returns>
        ApiPagingDataBean<FILES> FileList(ref ErrorInfo err, ApiRequesPageBean<ApiPagingDataBean<FILES>> inEnt);

        /// <summary>
        ///文件上传
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        FILES FileUp(ref ErrorInfo err, ApiRequesEntityBean<FILES> inEnt);

        /// <summary>
        /// 文件删除
        /// <para>只删除数据库和文件实体</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo FileDel(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);

        /// <summary>
        /// 检测登录Key是否存在
        /// <para>只是取存里是否用authToken</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo CheckToken(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);

        /// <summary>
        /// 发送验证码到手机
        /// <para>发送时会在用户的Login表里修改VERIFY_CODE，并在fa_sms_send增加记录</para>
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo SendCode(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);


        /// <summary>
        /// 直接发送短信
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="mobile">手机号码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        bool SmsSendCode(string loginKey, string mobile, string code);

    }
}
