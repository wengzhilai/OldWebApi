using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class FaUserInfo:FA_USER_INFO
    {
        /// <summary>
        /// 家族
        /// </summary>
        public FA_FAMILY Family { get; set; }
        /// <summary>
        /// 辈分
        /// </summary>
        public FA_ELDER Elder { get; set; }

        /// <summary>
        /// 所有朋友
        /// </summary>
        public IList<USER> FriendList{ get; set; }

        /// <summary>
        /// 所有事件
        /// </summary>
        public IList<FA_USER_EVENT> EventList { get; set; }

        /// <summary>
        /// 父亲
        /// </summary>
        public FA_USER_INFO Father { get; set; }
        /// <summary>
        /// 父亲姓名
        /// </summary>
        public string FatherName { get; set; }
        /// <summary>
        /// 母亲
        /// </summary>
        public FA_USER_INFO Mather { get; set; }
        /// <summary>
        ///  配偶
        /// </summary>
        public FA_USER_INFO Consort { get; set; }

        /// <summary>
        /// 出生日期中国历
        /// </summary>
        public Nullable<DateTime> BirthdayTimeChinese { get; set; }

        /// <summary>
        /// 过逝日期中国历
        /// </summary>
        public Nullable<DateTime> DiedTimeChinese { get; set; }
    }
}
