using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IFamilyApi
    {

        /// <summary>
        /// 获取所有辈份
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt">id:家族ID</param>
        /// <returns></returns>
        ApiPagingDataBean<FA_ELDER> OlderList(ref ErrorInfo err, ApiRequesPageBean<ApiPagingDataBean<FA_ELDER>> inEnt);

        /// <summary>
        /// 保存一个姓的所有辈分
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ErrorInfo OlderSave(ref ErrorInfo err, ApiRequesSaveEntityBean<List<FA_ELDER>> inEnt);

    }
}
