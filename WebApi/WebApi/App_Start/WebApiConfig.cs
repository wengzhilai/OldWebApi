using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using WebApi.Areas.HelpPage;
using System.Web;
using Newtonsoft.Json.Converters;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
     
            config.SetDocumentationProvider(new MultiXmlDocumentationProvider(
                HttpContext.Current.Server.MapPath("~/bin/WebApi.XML"), 
                HttpContext.Current.Server.MapPath("~/bin/ProInterface.XML")
                ));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-ddTHH:mm"
                }
                );


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
