using ProInterface.Models;
using System.Collections.Generic;


namespace ProInterface
{
    public interface IFlow
    {
        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        Dictionary<int,string> FlowAllFlownode();

        /// <summary>
        /// 获取所有系统角色
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        Dictionary<int,string> FlowAllRole(string loginKey, ref ErrorInfo err);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        TFlow FlowSingle(string loginKey, ref ErrorInfo err, int? id);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool FlowSave(string loginKey, ref ErrorInfo err, TFlow ent);

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        Dictionary<int,string> FlowAll();

        /// <summary>
        /// 获取用户有权启动的任务
        /// </summary>
        /// <returns></returns>
        Dictionary<int,string> FlowAllMy(string loginKey, ref ErrorInfo err);


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        TFlowFlownodeFlow FlowFristNode(string loginKey, ref ErrorInfo err, int flowId, string status);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="flowId"></param>
        /// <returns></returns>
        TFlowFlownodeFlow FlowGetNodeFlow(string loginKey, ref ErrorInfo err, int flowId, int fromFlownodeId, string statusName);
    }
}
