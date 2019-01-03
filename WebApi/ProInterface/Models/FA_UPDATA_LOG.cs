
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProInterface.Models
{
    /// <summary>
    /// 修改日志
    /// </summary>
    public class FA_UPDATA_LOG
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public Nullable<DateTime> CREATE_TIME { get; set; }
        /// <summary>
        /// 创建用户的姓名
        /// </summary>
        [StringLength(50)]
        [Display(Name = "创建用户的姓名")]
        public string CREATE_USER_NAME { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        [Display(Name = "创建用户ID")]
        public Nullable<int> CREATE_USER_ID { get; set; }
        /// <summary>
        /// 原内容
        /// </summary>
        [Display(Name = "原内容")]
        public string OLD_CONTENT { get; set; }
        /// <summary>
        /// 新内容
        /// </summary>
        [Display(Name = "新内容")]
        public string NEW_CONTENT { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [StringLength(50)]
        [Display(Name = "表名")]
        public string TABLE_NAME { get; set; }
    }
}
