using ProInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProInterface
{
    public interface IRoleApi
    {

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <param name="allPar"></param>
        /// <returns></returns>
        bool RoleSave(string loginKey, ref ErrorInfo err, TRole inEnt, IList<string> allPar=null);

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="id">条件lambda表达表</param>
        /// <returns></returns>
        TRole RoleSingle(string loginKey, ref ErrorInfo err, int id);
    }
}
