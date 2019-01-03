using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface
{
    /// <summary>
    /// 更新数据日志
    /// </summary>
    public interface IUpdataLog<T>
    {
        /// <summary>
        /// 保存新的实体
        /// </summary>
        /// <param name="gu"></param>
        /// <param name="newEnt"></param>
        /// <returns></returns>
        bool UpdataLogSaveCreate(GlobalUser gu, T newEnt);

        /// <summary>
        /// 保存更新实体
        /// </summary>
        /// <param name="gu"></param>
        /// <param name="newEnt"></param>
        /// <returns></returns>
        bool UpdataLogSaveUdate(GlobalUser gu, T newEnt);

        /// <summary>
        /// 删除更新实体
        /// </summary>
        /// <param name="gu"></param>
        /// <returns></returns>
        bool UpdataLogDelete(GlobalUser gu);

    }
}
