using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface.Models
{
    public class FaUserInfoRelativeItem
    {
        public int Id { get; set; }
        /// <summary>
        /// 辈字排号
        /// </summary>
        public int ElderId { get; set; }
        /// <summary>
        /// 辈字
        /// </summary>
        public string ElderName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父亲ID
        /// </summary>
        public int? FatherId { get; set; }
        /// <summary>
        /// 头像图片
        /// </summary>
        public string IcoUrl { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 坐标X
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// 坐标Y
        /// </summary>
        public int y { get; set; }
    }
}
