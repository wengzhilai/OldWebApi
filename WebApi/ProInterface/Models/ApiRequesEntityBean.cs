using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProInterface.Models
{
    /// <summary>
    /// 请求数据
    /// </summary>
    public class ApiRequesEntityBean<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiRequesEntityBean()
        {
            para = new List<KTV>();
        }
        /// <summary>
        /// 参数
        /// </summary>
        public IList<KTV> para { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 登录凭证
        /// </summary>
        public string authToken { get; set; }
        /// <summary>
        /// 参数ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 传入的数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 返回的实体
        /// </summary>
        public T SuccReturn { get; set; }
    }
}
