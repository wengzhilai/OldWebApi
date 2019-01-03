
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProInterface.Models
{
    /// <summary>
    /// 事件
    /// </summary>
    public class FA_USER_EVENT
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        /// <summary>
        /// USER_ID
        /// </summary>
        [Display(Name = "USER_ID")]
        public Nullable<int> USER_ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(50)]
        [Display(Name = "标题")]
        public string NAME { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        public Nullable<DateTime> HAPPEN_TIME { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500)]
        [Display(Name = "描述")]
        public string CONTENT { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        [StringLength(500)]
        [Display(Name = "地点")]
        public string ADDRESS { get; set; }

    }
}
