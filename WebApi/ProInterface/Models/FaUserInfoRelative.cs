using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface.Models
{
    public class FaUserInfoRelative
    {
        public FaUserInfoRelative() {
            ItemList = new List<FaUserInfoRelativeItem>();
            ElderList = new List<FA_ELDER>();
            RelativeList = new List<KV>();
        }

        /// <summary>
        /// 展示所有用户
        /// </summary>
        public IList<FaUserInfoRelativeItem> ItemList { get; set; }
        /// <summary>
        /// 所有辈分
        /// </summary>
        public IList<FA_ELDER> ElderList { get; set; }
        /// <summary>
        /// 所有关系
        /// </summary>
        public IList<KV> RelativeList { get; set; }
    }
}
