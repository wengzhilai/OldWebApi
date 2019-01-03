using ProInterface.Models;

namespace ProInterface
{
    public interface IWeixinApi
    {
        ApiWeiXinJSSDKBean WenXinJSSDKSign(ref ErrorInfo err, ApiRequesEntityBean<ApiWeiXinJSSDKBean> inEnt);
        ErrorInfo WeixinSendMsg(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);
        ErrorInfo WeixinGetOpenid(ref ErrorInfo err, ApiRequesEntityBean<ErrorInfo> inEnt);
        //YL_WEIXIN_USER WeixinGetUser(ref ErrorInfo err, ApiRequesEntityBean inEnt);
    }
}
