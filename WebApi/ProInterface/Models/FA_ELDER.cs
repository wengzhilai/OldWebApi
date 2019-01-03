
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProInterface.Models
{
    /// <summary>
    /// 事件
    /// </summary>
    public class FA_ELDER
    {

            /// <summary>
            /// ID
            /// </summary>
            [Required]
            [Display(Name = "ID")]
            public int ID { get; set; }
            /// <summary>
            /// FAMILY_ID
            /// </summary>
            [Display(Name = "FAMILY_ID")]
            public Nullable<int> FAMILY_ID { get; set; }
            /// <summary>
            /// NAME
            /// </summary>
            [Required]
            [StringLength(2)]
            [Display(Name = "NAME")]
            public string NAME { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            [Display(Name = "排序")]
            public Nullable<int> SORT { get; set; }
            
       
    }
}
