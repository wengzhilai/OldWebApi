using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ProInterface.Models
{
    /// <summary>
    /// User扩展
    /// </summary>
    public class TUser : USER
    {
        public TUser()
        {
        }

        public LOGIN Login { get; set; }

        /// <summary>
        /// 所有角色串
        /// </summary>
        public string RoleAllNameStr { get; set; }

        /// <summary>
        /// 区县名称
        /// </summary>
        public string DistrictNameStr { get; set; }
        /// <summary>
        /// 所有角色串
        /// </summary>
        public string RoleAllIDStr { get; set; }

        /// <summary>
        /// 收藏的模块
        /// </summary>
        public string AllModuleIdStr { get; set; }


        /// <summary>
        /// 管辖区域
        /// </summary>
        [StringLength(255)]
        [Display(Name = "管辖区域")]
        [Description(@"如果配置了此项。归属地将无效")]
        public string UserDistrict { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string ImgUrl { get; set; }
    }
}
