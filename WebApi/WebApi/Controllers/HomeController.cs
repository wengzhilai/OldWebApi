using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [RoutePrefix("Home")]

    public class HomeController : ApiController
    {

        /// <summary>
        /// 首页测试
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [Route("Index")]
        [HttpPost]
        [HttpGet]
        public string Index(dynamic inEnt)
        //public string Index(TestClass inEnt)
        {
            //TestClass inEnt = new TestClass();
            //inEnt.name = name;
            //inEnt.pwd = pwd;
            return inEnt.name + "_" + inEnt.pwd;
        }
    }

    public class TestClass
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }
    }
}
