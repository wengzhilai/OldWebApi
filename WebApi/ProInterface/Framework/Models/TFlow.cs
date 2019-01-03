using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProInterface.Models
{
    public class TFlow:FLOW
    {
        public TFlow()
        {
            FlowList = new List<TFlowFlownodeFlow>();
            Idxy = new List<XYZ>();
            AllFlownode = new Dictionary<int,string>();
        }
        public IList<TFlowFlownodeFlow> FlowList { get; set; }
        public Dictionary<int,string> AllFlownode { get; set; }
        public IList<XYZ> Idxy { get; set; }
        /// <summary>
        /// 流程节点JSON
        /// </summary>
        public string FlowListStr { get; set; }


    }
}
