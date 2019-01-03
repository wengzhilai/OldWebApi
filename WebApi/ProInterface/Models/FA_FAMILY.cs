
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProInterface.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class FA_FAMILY
    {

            /// <summary>
            /// ID
            /// </summary>
            [Required]
            [Display(Name = "ID")]
            public int ID { get; set; }
            /// <summary>
            /// NAME
            /// </summary>
            [Required]
            [StringLength(20)]
            [Display(Name = "NAME")]
            public string NAME { get; set; }
            
       
    }
}
