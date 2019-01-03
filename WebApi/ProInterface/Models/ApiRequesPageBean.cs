using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProInterface.Models
{
   /// <summary>
    /// 请求分布数据
    /// </summary>
    public class ApiRequesPageBean<T> : ApiRequesEntityBean<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiRequesPageBean()
        {
            attachParams = new List<KTV>();
            searchKey = new List<KTV>();
            orderBy = new List<KTV>();
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// 页码大小
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 附加参数
        /// </summary>
        public IList<KTV> attachParams { get; set; }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public IList<KTV> searchKey { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public IList<KTV> orderBy { get; set; }
    }
}
