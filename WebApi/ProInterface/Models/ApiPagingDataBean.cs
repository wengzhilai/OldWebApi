using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProInterface.Models
{
    /// <summary>
    /// 返回数据列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiPagingDataBean<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiPagingDataBean()
        {
            para = new List<KTV>();
        }
        /// <summary>
        /// 参数
        /// </summary>
        public IList<KTV> para { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// 页面条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public IList<T> data { get; set; }
    }
}
