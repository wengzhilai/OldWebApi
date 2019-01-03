using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProInterface.Models
{
    /// <summary>
    /// 提交实体
    /// </summary>
    /// <typeparam name="inT">传入的类型</typeparam>
    public class ApiRequesSaveEntityBean<inT>
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiRequesSaveEntityBean()
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
        /// 凭证
        /// </summary>
        public string authToken { get; set; }
        /// <summary>
        /// 需保存的字段
        /// </summary>
        public string saveKeys { get; set; }
        /// <summary>
        /// 提交的实体
        /// </summary>
        public inT entity { get; set; }

        /// <summary>
        /// 返回的实体
        /// </summary>
        public inT SuccReturn { get; set; }
    }
}
