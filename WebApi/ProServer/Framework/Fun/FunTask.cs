using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProInterface;
using ProInterface.Models;

namespace ProServer
{
    public class FunTask
    {
        public static object SubmitTask;

        /// <summary>
        /// 启动一个流程任务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="err"></param>
        /// <param name="gu"></param>
        /// <param name="inEnt">启动内容(FlowID,TaskName,AllFilesStr,UserIdArrStr)</param>
        /// <param name="submitStatu">如果启动时有多个流程需填写此项</param>
        /// <returns></returns>
        public static fa_task_flow StartTask(DBEntities db, ref ErrorInfo err, GlobalUser gu, TNode inEnt, string submitStatu=null)
        {
            
            fa_task_flow reEnt = new fa_task_flow();
            int StartFlownodeId = 9999;
            //流程
            fa_flow flow = db.fa_flow.SingleOrDefault(x => x.ID == inEnt.FlowID);

            if (flow == null)
            {
                err.IsError = true;
                err.Message = "流程不存在";
                return null;
            }

            //流程的入口
            fa_flow_flownode_flow startFlow = new fa_flow_flownode_flow();
            //入口的下一节点
            fa_flow_flownode flowNodeNext = new fa_flow_flownode();
            //流程的入口的下一流程
            fa_flow_flownode_flow flowNodeFlowNext = new fa_flow_flownode_flow();
            //入口的下一节点下一节点
            fa_flow_flownode flowNodeNextNext = new fa_flow_flownode();
            #region 初始化数据
            startFlow = flow.fa_flow_flownode_flow.SingleOrDefault(x => x.FROM_FLOWNODE_ID == StartFlownodeId);
            if (startFlow == null)
            {
                err.IsError = true;
                err.Message = "流程入口错误";
                return null;
            }
            flowNodeNext = db.fa_flow_flownode.SingleOrDefault(x => x.ID == startFlow.TO_FLOWNODE_ID);
            if (flowNodeNext == null)
            {
                err.IsError = true;
                err.Message = "未找到启动后的目标节点";
                return null;
            }
            flowNodeFlowNext = flow.fa_flow_flownode_flow.SingleOrDefault(x => x.FROM_FLOWNODE_ID == startFlow.TO_FLOWNODE_ID && x.STATUS == submitStatu);
            if (flowNodeFlowNext == null)
            {
                flowNodeFlowNext = flow.fa_flow_flownode_flow.SingleOrDefault(x => x.FROM_FLOWNODE_ID == startFlow.TO_FLOWNODE_ID);
            } 
            if (flowNodeFlowNext == null)
            {
                err.IsError = true;
                err.Message = "未找到下一节点";
                return null;
            } 
            flowNodeNextNext = db.fa_flow_flownode.SingleOrDefault(x => x.ID == flowNodeFlowNext.TO_FLOWNODE_ID);
            #endregion


            if (flowNodeFlowNext.ASSIGNER == 1 && string.IsNullOrEmpty(inEnt.UserIdArrStr))
            {
                err.IsError = true;
                err.Message = "必须指处理人";
                return null;
            }

            //每启一个任务生成共三条

            fa_task task = new fa_task();
            #region 初始化任务

            task.ID = Fun.GetSeqID<fa_task>();
            task.FLOW_ID = flow.ID;
            task.TASK_NAME = inEnt.TaskName;
            task.CREATE_TIME = DateTime.Now;
            task.CREATE_USER = gu.UserId;
            task.CREATE_USER_NAME = gu.UserName;
            task.REMARK = inEnt.Remark;
            task.KEY_ID = inEnt.TaskKey;
            task.STATUS = "未完成";
            task.START_TIME = inEnt.StartTime;
            task.END_TIME = inEnt.EndTime;
            task.TASK_NAME = task.TASK_NAME.Replace("@(TASK_ID)", task.ID.ToString());
            task.STATUS_TIME = DateTime.Now;

            //保单有效期为72小时
            if (task.FLOW_ID == 1)
            {
                task.END_TIME = DateTime.Now.AddHours(72);
            } 
            #endregion

            fa_task_flow taskFlow0 = new fa_task_flow();
            #region 初始化启动节点

            taskFlow0.ID = Fun.GetSeqID<fa_task_flow>();
            taskFlow0.DEAL_STATUS = startFlow.STATUS;
            taskFlow0.HANDLE_USER_ID = gu.UserId;
            taskFlow0.FLOWNODE_ID = StartFlownodeId;
            taskFlow0.HANDLE_URL = startFlow.fa_flow_flownode.HANDLE_URL;
            taskFlow0.SHOW_URL = startFlow.fa_flow_flownode.SHOW_URL;
            taskFlow0.LEVEL_ID = 1;
            taskFlow0.IS_HANDLE = 1;
            taskFlow0.NAME = startFlow.fa_flow_flownode.NAME;
            //taskFlow0.NAME = string.Format("{0}处理",db.fa_role.ToList().SingleOrDefault(x=>gu.RoleID.Contains(x.ID)));
            taskFlow0.START_TIME = DateTime.Now;
            taskFlow0.DEAL_TIME = DateTime.Now;
            taskFlow0.ACCEPT_TIME = DateTime.Now;
            taskFlow0.TASK_ID = task.ID;
            taskFlow0.fa_task = task;

            taskFlow0.fa_task_flow_handle.Add(new fa_task_flow_handle
            {
                ID = Fun.GetSeqID<fa_task_flow_handle>(),
                CONTENT = "启动",
                DEAL_TIME = DateTime.Now,
                DEAL_USER_ID = gu.UserId,
                DEAL_USER_NAME = gu.UserName,
                TASK_FLOW_ID = taskFlow0.ID
            }); 
            #endregion

            task.fa_task_flow.Add(taskFlow0);
            //如果下级节点不为空
            if (flowNodeNext != null)
            {
                fa_task_flow taskFlowNext1 = new fa_task_flow();
                #region 初始化创建节点

                taskFlowNext1.ID = Fun.GetSeqID<fa_task_flow>();
                taskFlowNext1.DEAL_STATUS = flowNodeFlowNext.STATUS;
                taskFlowNext1.HANDLE_USER_ID = gu.UserId;
                taskFlowNext1.FLOWNODE_ID = flowNodeNext.ID;
                taskFlowNext1.HANDLE_URL = flowNodeNext.HANDLE_URL;
                taskFlowNext1.SHOW_URL = flowNodeNext.SHOW_URL;
                taskFlowNext1.LEVEL_ID = taskFlow0.LEVEL_ID + 1;
                taskFlowNext1.IS_HANDLE = 1;
                taskFlowNext1.NAME = string.Format("{0}处理", string.Join(",", db.fa_role.ToList().Where(x => gu.RoleID.Contains(x.ID)).Select(x => x.NAME)));
                //taskFlowNext1.NAME = flowNodeNext.NAME;
                taskFlowNext1.fa_task_flow2 = taskFlow0;
                taskFlowNext1.START_TIME = DateTime.Now;
                taskFlowNext1.DEAL_TIME = DateTime.Now;
                taskFlowNext1.ACCEPT_TIME = DateTime.Now;
                taskFlowNext1.TASK_ID = task.ID;
                taskFlowNext1.fa_task = task;

                IList<fa_files> allFile = new List<fa_files>();
                if (!string.IsNullOrEmpty(inEnt.AllFilesStr))
                {
                    var fileIdList = ProInterface.JSON.EncodeToEntity<IList<FILES>>(inEnt.AllFilesStr).Select(x => x.ID);
                    allFile = db.fa_files.Where(x => fileIdList.Contains(x.ID)).ToList();
                }

                taskFlowNext1.fa_task_flow_handle.Add(new fa_task_flow_handle
                {
                    ID = Fun.GetSeqID<fa_task_flow_handle>(),
                    CONTENT = inEnt.Remark,
                    DEAL_TIME = DateTime.Now,
                    DEAL_USER_ID = gu.UserId,
                    DEAL_USER_NAME = gu.UserName,
                    TASK_FLOW_ID = taskFlow0.ID,
                    fa_files = allFile
                });

                #endregion
                if (flowNodeNextNext != null)
                {
                    if (string.IsNullOrEmpty(inEnt.UserIdArrStr)) inEnt.UserIdArrStr = "";
                    IList<int> userIdArr = inEnt.UserIdArrStr.Split(',').Where(x => x.IsInt32()).Select(x => Convert.ToInt32(x)).ToList();
                    var allUser = db.fa_user.Where(x => userIdArr.Contains(x.ID)).ToList();
                    if (allUser.Count() > 0)
                    {
                        foreach (var user in allUser)
                        {
                            fa_task_flow taskFlowNext2 = new fa_task_flow();
                            #region 初始化指定人待处理

                            taskFlowNext2.ID = Fun.GetSeqID<fa_task_flow>();
                            taskFlowNext2.FLOWNODE_ID = flowNodeNextNext.ID;
                            taskFlowNext2.HANDLE_URL = flowNodeNextNext.HANDLE_URL;
                            taskFlowNext2.SHOW_URL = flowNodeNextNext.SHOW_URL;
                            taskFlowNext2.LEVEL_ID = taskFlowNext1.LEVEL_ID + 1;
                            taskFlowNext2.IS_HANDLE = 0;
                            taskFlowNext2.NAME = string.Format("待{0}处理", string.Join(",", user.fa_role.Select(x => x.NAME).ToList()));
                            taskFlowNext2.fa_task_flow2 = taskFlowNext1;
                            taskFlowNext2.fa_task = task;
                            taskFlowNext2.START_TIME = DateTime.Now;
                            //表示无须受理
                            taskFlowNext2.ACCEPT_TIME = DateTime.Now;

                            taskFlowNext2.DEAL_STATUS = "待处理";
                            taskFlowNext2.HANDLE_USER_ID = user.ID;
                            if (flowNodeFlowNext.EXPIRE_HOUR != 0)
                            {
                                taskFlowNext2.EXPIRE_TIME = taskFlowNext2.START_TIME.AddHours(flowNodeFlowNext.EXPIRE_HOUR);
                            } 
                            #endregion
                            taskFlowNext1.fa_task_flow1.Add(taskFlowNext2);
                        } 
                    }
                    else
                    {
                        fa_task_flow taskFlowNext2 = new fa_task_flow();

                        taskFlowNext2.ID = Fun.GetSeqID<fa_task_flow>();
                        taskFlowNext2.FLOWNODE_ID = flowNodeNextNext.ID;
                        taskFlowNext2.HANDLE_URL = flowNodeNextNext.HANDLE_URL;
                        taskFlowNext2.SHOW_URL = flowNodeNextNext.SHOW_URL;
                        taskFlowNext2.LEVEL_ID = taskFlowNext1.LEVEL_ID + 1;
                        taskFlowNext2.IS_HANDLE = 0;
                        taskFlowNext2.DEAL_STATUS = "待处理";
                        //表示无须受理
                        taskFlowNext2.ACCEPT_TIME = DateTime.Now;
                        taskFlowNext2.START_TIME = DateTime.Now;
                        if (flowNodeFlowNext.EXPIRE_HOUR != 0)
                        {
                            taskFlowNext2.EXPIRE_TIME = taskFlowNext2.START_TIME.AddHours(flowNodeFlowNext.EXPIRE_HOUR);
                        }
                        taskFlowNext2.fa_task_flow2 = taskFlowNext1;
                        taskFlowNext2.fa_task = task;

                        var allRoleFlow = db.fa_role.Where(x => x.fa_flow_flownode_flow.Where(y => y.FROM_FLOWNODE_ID == taskFlowNext2.FLOWNODE_ID && y.FLOW_ID == task.FLOW_ID).Count() > 0).ToList();
                        taskFlowNext2.NAME = string.Format("待{0}处理", string.Join(",", allRoleFlow.Select(x => x.NAME)));
                        
                        var server = new Service();
                        IList<int> nowHandleUserIdList = new List<int>();
                        if (string.IsNullOrEmpty(gu.Guid))
                        {
                            nowHandleUserIdList = server.UserGetAllUserById(gu.UserId, ref err, allRoleFlow.Select(x => x.ID).ToList());
                        }
                        else
                        {
                            nowHandleUserIdList = server.UserGetAllUserById(gu.UserId, ref err, allRoleFlow.Select(x => x.ID).ToList(),2);
                        }
                        if (nowHandleUserIdList.Count() == 0)
                        {
                            err.IsError = true;
                            err.Message = string.Format("创建失败，请先配置角色[{0}]下的用户", string.Join(",",allRoleFlow.Select(x=>x.NAME).ToList()));
                            return null;
                        }

                        var allNextFlow = flow.fa_flow_flownode_flow.Where(x => x.FROM_FLOWNODE_ID == flowNodeFlowNext.TO_FLOWNODE_ID && x.HANDLE==1);

                        if (allNextFlow.Count() == 0) //一人处理即可
                        {
                            foreach (var handleU in nowHandleUserIdList)
                            {
                                taskFlowNext2.fa_task_flow_handle_USER.Add(new fa_task_flow_handle_USER { HANDLE_USER_ID = handleU });
                            }
                            taskFlowNext1.fa_task_flow1.Add(taskFlowNext2);
                        }
                        else //所有人必须处理
                        {
                            foreach (var handleU in nowHandleUserIdList)
                            {
                                var tmpTaskFlow = Fun.ClassToCopy<fa_task_flow, fa_task_flow>(taskFlowNext2);
                                tmpTaskFlow.ID = Fun.GetSeqID<fa_task_flow>();
                                tmpTaskFlow.HANDLE_USER_ID = handleU;
                                db.fa_task_flow.Add(tmpTaskFlow);
                            }
                        }
                    }
                }
                reEnt = taskFlowNext1;
                taskFlow0.fa_task_flow1.Add(taskFlowNext1);
            }
            task.KEY = reEnt.ID.ToString();
            db.fa_task.Add(task);
            return reEnt;
        }

        /// <summary>
        /// 启动一个流程任务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="err"></param>
        /// <param name="gu"></param>
        /// <param name="flowId">流程ID</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="allFileStr">上传的文件</param>
        /// <param name="userIdArrStr">指定处理人</param>
        /// <returns></returns>
        public static fa_task_flow StartTask(DBEntities db, ref ErrorInfo err, GlobalUser gu,int flowId,string taskName,string content,string allFileStr, string userIdArrStr,string taskKey="")
        {
            TNode inEnt=new TNode();
            inEnt.FlowID = flowId;
            inEnt.TaskName = taskName;
            inEnt.AllFilesStr = allFileStr;
            inEnt.UserIdArrStr = userIdArrStr;
            inEnt.Remark = content;
            inEnt.TaskKey = taskKey;
            return StartTask(db, ref err, gu, inEnt);
        }

        /// <summary>
        /// 启动任务工单
        /// </summary>
        /// <param name="db"></param>
        /// <param name="err"></param>
        /// <param name="gu"></param>
        /// <param name="taskName"></param>
        /// <param name="allFileStr"></param>
        /// <param name="userIdArrStr"></param>
        /// <param name="roleIdStr"></param>
        /// <returns></returns>
        public static bool StartTaskNoFlow(DBEntities db, ref ErrorInfo err, GlobalUser gu, 
            string taskName,
            string content,
            string allFileStr, 
            string userIdArrStr,
            string roleIdStr)
        {


            if (string.IsNullOrEmpty(userIdArrStr) && string.IsNullOrEmpty(roleIdStr))
            {
                err.IsError = true;
                err.Message = "请选择处理人或角色";
                return false;
            }

            var ent = new fa_task();
            ent.ID = Fun.GetSeqID<fa_task>();
            if (string.IsNullOrEmpty(taskName))
            {
                switch (ProInterface.AppSet.CityId)
                {
                    case 852:
                        ent.TASK_NAME = string.Format("ZY-01-{0}-{1}", DateTime.Now.ToString("yyMMdd"), ent.ID.ToString());
                        break;
                    case 855:
                        ent.TASK_NAME = string.Format("QDN-01-{0}-{1}", DateTime.Now.ToString("yyMMdd"), ent.ID.ToString());
                        break;
                }
            }
            else
            {
                ent.TASK_NAME = taskName.Replace("@(TASK_ID)", ent.ID.ToString());
            }
            ent.CREATE_TIME = DateTime.Now;
            ent.CREATE_USER = gu.UserId;
            ent.CREATE_USER_NAME = gu.UserName;
            ent.STATUS = "待处理";
            ent.REMARK = content;
            ent.STATUS_TIME = DateTime.Now;
            ent.START_TIME = DateTime.Now;
            ent.END_TIME = DateTime.Now;

            var taskFlowStart = new fa_task_flow();
            taskFlowStart.ID = Fun.GetSeqID<fa_task_flow>();
            taskFlowStart.DEAL_STATUS = "开始";
            taskFlowStart.HANDLE_USER_ID = gu.UserId;
            taskFlowStart.LEVEL_ID = 1;
            taskFlowStart.IS_HANDLE = 1;
            taskFlowStart.NAME = "开始";
            taskFlowStart.TASK_ID = ent.ID;
            taskFlowStart.START_TIME = DateTime.Now;
            taskFlowStart.DEAL_TIME = DateTime.Now;
            taskFlowStart.fa_task = ent;
            ent.fa_task_flow.Add(taskFlowStart);

            var taskFlow0 = new fa_task_flow();
            taskFlow0.ID = Fun.GetSeqID<fa_task_flow>();
            taskFlow0.DEAL_STATUS = "创建任务";
            taskFlow0.HANDLE_USER_ID = gu.UserId;
            taskFlow0.LEVEL_ID = 1;
            taskFlow0.IS_HANDLE = 1;
            taskFlow0.NAME = string.Format("{0}创建任务",gu.UserName);
            taskFlow0.TASK_ID = ent.ID;
            taskFlow0.START_TIME = DateTime.Now;
            taskFlow0.DEAL_TIME = DateTime.Now;
            taskFlow0.HANDLE_URL = "~/Task/Handle";
            taskFlow0.SHOW_URL = "~/TaskFlow/Single";
            taskFlow0.fa_task = ent;
            taskFlowStart.fa_task_flow1.Add(taskFlow0);

            taskFlow0.fa_task_flow_handle.Add(new fa_task_flow_handle
            {
                ID = Fun.GetSeqID<fa_task_flow_handle>(),
                CONTENT = content,
                DEAL_TIME = DateTime.Now,
                DEAL_USER_ID = gu.UserId,
                DEAL_USER_NAME = gu.UserName,
                TASK_FLOW_ID = taskFlow0.ID
            });

            ent.fa_task_flow.Add(taskFlow0);

            if (!string.IsNullOrEmpty(userIdArrStr))
            {
                IList<fa_user> allUser = new List<fa_user>();
                if (string.IsNullOrEmpty(userIdArrStr))
                {

                    taskFlow0.ROLE_ID_STR = roleIdStr;
                    IList<int> roleIdArr = roleIdStr.Split(',').Where(x => x.IsInt32()).Select(x => Convert.ToInt32(x)).ToList();
                    var allRoleUser = db.fa_role.Where(x => roleIdArr.Contains(x.ID)).Select(x => x.fa_user).ToList();
                    foreach (var t in allRoleUser)
                    {
                        allUser = allUser.Union(t).ToList();
                    }
                }
                else
                {
                    IList<int> userIdArr = userIdArrStr.Split(',').Where(x => x.IsInt32()).Select(x => Convert.ToInt32(x)).ToList();
                    allUser = db.fa_user.Where(x => userIdArr.Contains(x.ID)).ToList();
                    var allRole = db.fa_role.Where(x => x.fa_user.Where(y => userIdArr.Contains(y.ID)).Count() > 0).ToList();
                    taskFlow0.ROLE_ID_STR = string.Join(",", allRole.Select(x => x.ID).ToList());
                }

                foreach (var user in allUser)
                {
                    fa_task_flow taskFlowNext1 = new fa_task_flow();
                    taskFlowNext1.ID = Fun.GetSeqID<fa_task_flow>();
                    taskFlowNext1.HANDLE_USER_ID = user.ID;
                    taskFlowNext1.LEVEL_ID = taskFlow0.LEVEL_ID + 1;
                    taskFlowNext1.HANDLE_URL = "~/Task/Handle";
                    taskFlowNext1.SHOW_URL = "~/TaskFlow/Single";
                    taskFlowNext1.IS_HANDLE = 0;
                    taskFlowNext1.NAME = string.Format("待{0}处理", string.Join(",", user.fa_role.Select(x => x.NAME)));
                    taskFlowNext1.fa_task_flow2 = taskFlow0;
                    taskFlowNext1.TASK_ID = ent.ID;
                    taskFlowNext1.START_TIME = DateTime.Now;
                    taskFlowNext1.fa_task = ent;
                    taskFlow0.fa_task_flow1.Add(taskFlowNext1);
                }
            }
            db.fa_task.Add(ent);
            return true;
        }

        /// <summary>
        /// 无流程提交
        /// </summary>
        /// <param name="db"></param>
        /// <param name="err"></param>
        /// <param name="gu"></param>
        /// <param name="taskFlowId"></param>
        /// <param name="content"></param>
        /// <param name="allFileStr"></param>
        /// <param name="userIdArrStr"></param>
        /// <param name="roleIdStr"></param>
        /// <param name="IsStage">0表示回复，1表示转派</param>
        /// <param name="butName">转派/回复</param>
        /// <returns></returns>
        public static bool NoFlowSubmit(DBEntities db, ref ErrorInfo err, GlobalUser gu,
            int taskFlowId,
            string content,
            string allFileStr,
            string userIdArrStr,
            string roleIdStr,
            int IsStage,
            string butName)
        {
            if (string.IsNullOrEmpty(content))
            {
                err.IsError = true;
                err.Message = "处理内容不能为空";
                return false;
            }

            var taskFlow = db.fa_task_flow.SingleOrDefault(x => x.ID == taskFlowId);
            if (taskFlow == null)
            {
                err.IsError = true;
                err.Message = "流程不存在";
                return false;
            }

            switch (butName)
            {
                case "转派":
                    #region 转派
                    if (string.IsNullOrEmpty(userIdArrStr) && string.IsNullOrEmpty(roleIdStr))
                    {
                        err.IsError = true;
                        err.Message = "转派时，转派用户或角色不能为空";
                        return false;
                    }
                    taskFlow.IS_HANDLE = 1;
                    taskFlow.DEAL_STATUS = butName;
                    taskFlow.DEAL_TIME = DateTime.Now;
                    taskFlow.fa_task_flow_handle.Add(new fa_task_flow_handle
                    {
                        ID = Fun.GetSeqID<fa_task_flow_handle>(),
                        CONTENT = content,
                        DEAL_TIME = DateTime.Now,
                        DEAL_USER_ID = gu.UserId,
                        DEAL_USER_NAME = gu.UserName,
                        TASK_FLOW_ID = taskFlow.ID
                    });
                    if (string.IsNullOrEmpty(userIdArrStr))
                    {
                        var roleArr = roleIdStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        var roleList = db.fa_role.Where(x => roleArr.Contains(x.ID)).ToList();

                        taskFlow.NAME = string.Format("转派【{0}】", string.Join(",", roleList.Select(x => x.NAME)));

                        fa_task_flow taskFlowNext = new fa_task_flow();
                        taskFlowNext.ID = Fun.GetSeqID<fa_task_flow>();
                        taskFlowNext.PARENT_ID = (taskFlow.EQUAL_ID == null) ? taskFlow.ID : taskFlow.EQUAL_ID;
                        taskFlowNext.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                        taskFlowNext.IS_HANDLE = 0;
                        taskFlowNext.HANDLE_URL = taskFlow.HANDLE_URL;
                        taskFlowNext.SHOW_URL = taskFlow.SHOW_URL;
                        taskFlowNext.NAME = string.Format("【{0}】处理",string.Join(",",roleList.Select(x=>x.NAME)));
                        taskFlowNext.DEAL_STATUS = "待处理";
                        taskFlowNext.fa_task_flow2 = taskFlow;
                        taskFlowNext.TASK_ID = taskFlow.TASK_ID;
                        taskFlowNext.ROLE_ID_STR = string.Join(",", roleList.Select(x => x.ID).ToList());
                        taskFlowNext.START_TIME = DateTime.Now;
                        db.fa_task_flow.Add(taskFlowNext);
                    }
                    else
                    {

                        var userrIdArr = userIdArrStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        var userList = db.fa_user.Where(x => userrIdArr.Contains(x.ID)).ToList();
                        taskFlow.NAME = string.Format("转派【{0}】", string.Join(",", userList.Select(x => x.NAME)));
                        foreach (var user in userList)
                        {
                            fa_task_flow taskFlowNext = new fa_task_flow();
                            taskFlowNext.ID = Fun.GetSeqID<fa_task_flow>();
                            taskFlowNext.PARENT_ID = (taskFlow.EQUAL_ID == null) ? taskFlow.ID : taskFlow.EQUAL_ID;
                            taskFlowNext.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                            taskFlowNext.IS_HANDLE = 0;
                            taskFlowNext.HANDLE_URL = taskFlow.HANDLE_URL;
                            taskFlowNext.SHOW_URL = taskFlow.SHOW_URL;
                            taskFlowNext.NAME = string.Format("【{0}】处理", user.NAME);
                            taskFlowNext.DEAL_STATUS = "待处理";
                            taskFlowNext.fa_task_flow2 = taskFlow;
                            taskFlowNext.TASK_ID = taskFlow.TASK_ID;
                            taskFlowNext.HANDLE_USER_ID = user.ID;
                            taskFlowNext.ROLE_ID_STR = roleIdStr;
                            taskFlowNext.START_TIME = DateTime.Now;
                            db.fa_task_flow.Add(taskFlowNext);
                        }
                    }
                    #endregion
                    break;
                case "回复":
                    taskFlow.fa_task_flow_handle.Add(new fa_task_flow_handle
                    {
                        ID = Fun.GetSeqID<fa_task_flow_handle>(),
                        CONTENT = content,
                        DEAL_TIME = DateTime.Now,
                        DEAL_USER_ID = gu.UserId,
                        DEAL_USER_NAME = gu.UserName,
                        TASK_FLOW_ID = taskFlow.ID
                    });


                    if (IsStage == 0)//回复,不是阶段回复
                    {
                        if (taskFlow.fa_task_flow2 == null)
                        {
                            err.IsError = true;
                            err.Message = "没找到主流程";
                            return false;
                        }

                        if (taskFlow.fa_task_flow2.PARENT_ID == null)
                        {
                            err.IsError = true;
                            err.Message = "已经是第一级了，不能【回复】。只能【归档】";
                            return false;
                        }
                        taskFlow.NAME = string.Format("【{0}】处理", gu.UserName);

                        taskFlow.IS_HANDLE = 1;
                        taskFlow.DEAL_STATUS = butName;
                        taskFlow.DEAL_TIME = DateTime.Now;
                        //表示还有人已经处理
                        if (taskFlow.fa_task_flow2.fa_task_flow1.Where(x => x.IS_HANDLE == 0).Count() == 0)
                        {
                           
                            var taskFlowNext = new fa_task_flow();
                            taskFlowNext.ID = Fun.GetSeqID<fa_task_flow>();
                            taskFlowNext.PARENT_ID = taskFlow.fa_task_flow2.PARENT_ID;
                            taskFlowNext.TASK_ID = taskFlow.fa_task_flow2.TASK_ID;
                            taskFlowNext.LEVEL_ID = taskFlow.fa_task_flow2.LEVEL_ID;
                            taskFlowNext.IS_HANDLE = 0;
                            taskFlowNext.NAME = taskFlow.fa_task_flow2.NAME;
                            taskFlowNext.HANDLE_URL = taskFlow.fa_task_flow2.HANDLE_URL;
                            taskFlowNext.SHOW_URL = taskFlow.fa_task_flow2.SHOW_URL;
                            taskFlowNext.START_TIME = DateTime.Now;
                            var nowUser = db.fa_user.SingleOrDefault(x => x.ID == taskFlow.fa_task_flow2.HANDLE_USER_ID);
                            taskFlowNext.DEAL_STATUS = "待处理";
                            if (nowUser != null)
                            {
                                taskFlowNext.NAME = string.Format("【{0}】处理", nowUser.NAME);
                            }
                            taskFlowNext.HANDLE_USER_ID = taskFlow.fa_task_flow2.HANDLE_USER_ID;
                            taskFlowNext.EQUAL_ID = taskFlow.PARENT_ID;
                            db.fa_task_flow.Add(taskFlowNext);
                        }
                    }
                    else {
                        taskFlow.NAME = string.Format("【{0}】阶段回复", gu.UserName);
                    }
                    break;
                case "归档":
                    taskFlow.IS_HANDLE = 1;
                    taskFlow.DEAL_STATUS = butName;
                    taskFlow.DEAL_TIME = DateTime.Now;

                    fa_task_flow taskFlowEnd = new fa_task_flow();
                        taskFlowEnd.ID = Fun.GetSeqID<fa_task_flow>();
                        taskFlowEnd.PARENT_ID = (taskFlow.EQUAL_ID == null) ? taskFlow.ID : taskFlow.EQUAL_ID;
                        taskFlowEnd.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                        taskFlowEnd.IS_HANDLE = 1;
                        taskFlowEnd.NAME = string.Format("【{0}】归档",gu.UserName);
                        taskFlowEnd.DEAL_STATUS = "归档";
                        taskFlowEnd.TASK_ID = taskFlow.TASK_ID;
                        taskFlowEnd.START_TIME = DateTime.Now;
                        taskFlowEnd.HANDLE_USER_ID = gu.UserId;
                        taskFlowEnd.DEAL_TIME = DateTime.Now;
                        db.fa_task_flow.Add(taskFlowEnd);
                    break;
                default:
                    err.IsError = true;
                    err.Message = "按钮值不存在";
                    return false;
            }
            taskFlow.fa_task.STATUS = butName;
            taskFlow.fa_task.STATUS_TIME = DateTime.Now;

            return true;
        }

        /// <summary>
        /// 案件提交
        /// </summary>
        /// <param name="db"></param>
        /// <param name="err"></param>
        /// <param name="gu"></param>
        /// <param name="taskFlowId">任务流程ID</param>
        /// <param name="content">处理内容</param>
        /// <param name="filesIdArrStr">文件ID串</param>
        /// <param name="userIdArrStr">用户ID串</param>
        /// <param name="roleIdArrStr">角色ID串</param>
        /// <param name="IsStage">是否阶段性处理</param>
        /// <param name="butName">按钮名称</param>
        /// <returns></returns>
        public static bool FlowSubmit(DBEntities db, ref ErrorInfo err, GlobalUser gu,
            int taskFlowId,
            string content,
            string filesIdArrStr,
            string userIdArrStr,
            string roleIdArrStr,
            int IsStage,
            string butName)
        {

            fa_task_flow nowFlow = db.fa_task_flow.SingleOrDefault(x => x.ID == taskFlowId);
            nowFlow.HANDLE_USER_ID = gu.UserId;

            if (butName.Equals("受理"))
            {
                nowFlow.ACCEPT_TIME = DateTime.Now;
                nowFlow.HANDLE_USER_ID = gu.UserId;
                return true;
            }
            else if(nowFlow.HANDLE_USER_ID != null && nowFlow.HANDLE_USER_ID != gu.UserId)
            {
                err.IsError = true;
                err.Message = "该定单已经被其他人受理了";
                return false;
            }


            if (string.IsNullOrEmpty(content))
            {
                if( "接车,定损,驳回".Split(',').Contains(butName))
                {
                    err.IsError = true;
                    err.Message = "处理内容不能为空";
                    return false;
                }
                content = butName;
            }

            var server = new Service();
            fa_task task = nowFlow.fa_task;
            fa_user taskUser = db.fa_user.SingleOrDefault(x => x.ID == task.CREATE_USER);

            task.STATUS = butName;
            task.STATUS_TIME = DateTime.Now;
            var tmp = new fa_task_flow_handle(); ;
            tmp.ID = Fun.GetSeqID<fa_task_flow_handle>();
            tmp.TASK_FLOW_ID = taskFlowId;
            tmp.DEAL_USER_NAME = gu.UserName;
            tmp.DEAL_TIME = DateTime.Now;
            tmp.CONTENT = content;
            tmp.DEAL_USER_ID = gu.UserId;
            if (!string.IsNullOrEmpty(filesIdArrStr))
            {
                var fileIdList = filesIdArrStr.Trim(',').Split(',').Select(x => Convert.ToInt32(x)).ToList();
                tmp.fa_files = db.fa_files.Where(x => fileIdList.Contains(x.ID)).ToList();
            }
            nowFlow.fa_task_flow_handle.Add(tmp);

            if (butName.Equals("驳回"))
            {
                #region 驳回
                var parent = nowFlow.fa_task_flow2;
                nowFlow.IS_HANDLE = 1;
                nowFlow.DEAL_STATUS = butName;
                nowFlow.DEAL_TIME = DateTime.Now;
                nowFlow.NAME = nowFlow.NAME.Substring(1);

                fa_task_flow endFlow = new fa_task_flow();
                endFlow.ID = Fun.GetSeqID<fa_task_flow>();
                endFlow.PARENT_ID = taskFlowId;
                endFlow.TASK_ID = nowFlow.TASK_ID;
                endFlow.LEVEL_ID = nowFlow.LEVEL_ID + 1;
                endFlow.FLOWNODE_ID = parent.FLOWNODE_ID;
                endFlow.IS_HANDLE = 0;
                endFlow.NAME = parent.NAME;
                endFlow.DEAL_STATUS = "待处理";
                endFlow.ACCEPT_TIME = DateTime.Now;
                endFlow.HANDLE_URL = parent.HANDLE_URL;
                endFlow.SHOW_URL = parent.SHOW_URL;
                endFlow.ROLE_ID_STR = parent.ROLE_ID_STR;
                endFlow.HANDLE_USER_ID = parent.HANDLE_USER_ID;
                db.fa_task_flow.Add(endFlow);
                #endregion
            }
            else
            {
                #region 正常回复

                #region 阶段回复
                if (IsStage == 1)//是阶段回复
                {
                    nowFlow.IS_HANDLE = 0;
                    nowFlow.DEAL_STATUS = "阶段回复";
                    nowFlow.DEAL_TIME = DateTime.Now;
                    nowFlow.NAME = string.Format("{0}阶段处理", gu.UserName);
                    return true;
                } 
                #endregion
                //当前有户可操作的节点
                var allFlownodeId = db.fa_flow_flownode_flow.Where(x => x.fa_role.Where(y => gu.RoleID.Contains(y.ID)).Count() > 0).Select(x => x.FROM_FLOWNODE_ID).ToList();
                var nextNode = db.fa_flow_flownode_flow.SingleOrDefault(x => x.FROM_FLOWNODE_ID == nowFlow.FLOWNODE_ID && x.FLOW_ID == task.FLOW_ID && x.STATUS == butName);
                if (nextNode == null)
                {
                    err.IsError = true;
                    err.Message = "没找到下级节点";
                    return false;
                }
                int nextFlownodeId = nextNode.TO_FLOWNODE_ID;

                nowFlow.IS_HANDLE = 1;
                nowFlow.DEAL_STATUS = butName;
                nowFlow.DEAL_TIME = DateTime.Now;
                nowFlow.NAME = nowFlow.NAME.Substring(1);
                switch (nextNode.HANDLE)
                {
                    case 0://一人处理即可
                        foreach (var t in nowFlow.fa_task_flow2.fa_task_flow1.ToList())
                        {
                            if (t.ID != nowFlow.ID)//不是现在的流程
                            {
                                t.DEAL_STATUS = "自动处理";
                                t.NAME = string.Format("{0}自动处理", gu.UserName);
                                t.IS_HANDLE = 1;
                            }
                        }
                        break;
                    case 1://所有人必须处理
                        foreach (var t in nowFlow.fa_task_flow2.fa_task_flow1.ToList())
                        {
                            if (t.ID != nowFlow.ID && t.IS_HANDLE == 0)//不是现在的流程,并且还没有处理
                            {
                                return true;
                            }
                        }
                        break;
                }

                //没指定人，或结束
                if (string.IsNullOrEmpty(userIdArrStr) || nextFlownodeId == 8888 || userIdArrStr.Trim(',') == "")
                {
                    #region 没指定人，或结束
                    fa_task_flow endFlow = new fa_task_flow();
                    endFlow.ID = Fun.GetSeqID<fa_task_flow>();
                    endFlow.PARENT_ID = taskFlowId;
                    endFlow.TASK_ID = nowFlow.TASK_ID;
                    endFlow.LEVEL_ID = nowFlow.LEVEL_ID + 1;
                    endFlow.FLOWNODE_ID = nextFlownodeId;
                    if (nextFlownodeId == 8888)  //表示是结束
                    {
                        #region 结束并发送短信
                        endFlow.IS_HANDLE = 1;
                        endFlow.HANDLE_USER_ID = gu.UserId;
                        endFlow.DEAL_STATUS = "归档";
                        endFlow.DEAL_TIME = DateTime.Now;
                        endFlow.ACCEPT_TIME = DateTime.Now;
                        endFlow.START_TIME = DateTime.Now;
                        endFlow.EXPIRE_TIME = DateTime.Now;
                        endFlow.NAME = string.Format("{0}归档", gu.UserName);
                        db.fa_task_flow.Add(endFlow);
                        task.STATUS = "完成";

                        if (
                            task.FLOW_ID != null &&
                            (new List<int>() { 1, 2, 3, 4 }).Contains(task.FLOW_ID.Value) &&
                            task.KEY.IsInt32()
                            )
                        {
                            int key = Convert.ToInt32(task.KEY);
                            var order = db.YL_ORDER.SingleOrDefault(x => x.ID == key);
                            if (order != null)
                            {
                                order.PAY_STATUS = "完成";
                                order.PAY_STATUS_TIME = DateTime.Now;
                            }
                        }
                        //完成后，发送信息
                        if (taskUser != null)
                        {
                            server.SmsSendOrder(taskUser.LOGIN_NAME, taskUser.NAME, "已经完成", taskUser.ID, Convert.ToInt32(task.KEY),task.FLOW_ID.ToString());
                        }
                        #endregion

                    }
                    else
                    {
                        var allRoleFlow = db.fa_role.Where(x => x.fa_flow_flownode_flow.Where(y => y.FROM_FLOWNODE_ID == nextFlownodeId && y.FLOW_ID == task.FLOW_ID).Count() > 0).ToList();
                        var allRoleIdList = allRoleFlow.Select(x => x.ID).ToList();
                        IList<int> nowTaskAllUser = new List<int>();
                        IList<int> nowHandleUserIdList = new List<int>();
                        var startUserId = nowFlow.fa_task.fa_task_flow.SingleOrDefault(x => x.LEVEL_ID == 1).HANDLE_USER_ID;
                        switch (nextNode.ASSIGNER)
                        {
                            //指定角色
                            case 1:
                                nowHandleUserIdList = server.UserGetAllUserById(startUserId.Value, ref err, allRoleFlow.Select(x => x.ID).ToList());
                                break;
                            //发起人
                            case 3:
                                nowHandleUserIdList = new List<int> { startUserId.Value };
                                break;
                            case 4:
                                //所有已经处理过该工单的用户
                                nowHandleUserIdList = db.fa_task_flow_handle.Where(x=>x.fa_task_flow.TASK_ID==task.ID).Select(x=>x.DEAL_USER_ID).ToList();
                                //所有已经处理过该工单的用户,过滤角色
                                nowHandleUserIdList = db.fa_user.Where(x => nowHandleUserIdList.Contains(x.ID) && x.fa_role.Where(y => allRoleIdList.Contains(y.ID)).Count() > 0).Select(x => x.ID).ToList();
                                break;
                            default:
                                nowHandleUserIdList = server.UserGetAllUserById(startUserId.Value, ref err, allRoleIdList);
                                break;
                        }

                        if (nowHandleUserIdList == null || nowHandleUserIdList.Count() == 0) //上级或没有找到
                        {
                            err.IsError = true;
                            err.Message = "没找到转派后的处理人";
                            return false;
                        }
                        //当是算单时
                        if(butName=="转算单员" && task.FLOW_ID==1)
                        {
                            Dictionary<int, int> dic = new Dictionary<int, int>();
                            foreach (var t in nowHandleUserIdList)
                            {
                                var count = db.fa_task_flow.Where(x => x.IS_HANDLE == 0 &&
                                 (
                                 x.HANDLE_USER_ID == t ||
                                 (x.HANDLE_USER_ID == null && x.fa_task_flow_handle_USER.Where(y => nowHandleUserIdList.Contains(y.HANDLE_USER_ID)).Count() > 0)
                                 )
                                ).Count();
                                dic.Add(t, count);
                            }
                            var nowOrder= dic.OrderBy(x => x.Value).ToList();
                            nowHandleUserIdList =new List<int>() { nowOrder[0].Key };
                        }

                        //判断是否重复添加了

                        var flowList = db.fa_task_flow.Where(x => x.PARENT_ID == taskFlowId && x.TASK_ID == nowFlow.TASK_ID && x.LEVEL_ID == nowFlow.LEVEL_ID + 1 && x.IS_HANDLE == 0).ToList();
                        #region 如果有重复的则进行修改
                        if (flowList.Count() > 0)
                        {
                            foreach (var handleU in nowHandleUserIdList)
                            {
                                var tmpHandleUser = flowList[0].fa_task_flow_handle_USER.SingleOrDefault(x => x.HANDLE_USER_ID == handleU);
                                if (tmpHandleUser == null)
                                {
                                    flowList[0].fa_task_flow_handle_USER.Add(new fa_task_flow_handle_USER { HANDLE_USER_ID = handleU });
                                }
                            }
                            flowList[0].IS_HANDLE = 0;
                            return true;
                        }
                        #endregion

                        var node = db.fa_flow_flownode.Single(x => x.ID == nextFlownodeId);
                        endFlow.IS_HANDLE = 0;
                        endFlow.DEAL_STATUS = "待处理";
                        endFlow.HANDLE_URL = node.HANDLE_URL;
                        endFlow.SHOW_URL = node.SHOW_URL;
                        //如为空，则需要受理
                        endFlow.ACCEPT_TIME = DateTime.Now;
                        endFlow.START_TIME = DateTime.Now;
                        endFlow.ROLE_ID_STR = string.Join(",", allRoleFlow.Select(x => x.ID).ToList());
                        endFlow.NAME = string.Format("待{0}处理", string.Join(",", allRoleFlow.Select(x => x.NAME).ToList()));

                        //一人处理即可，只在fa_task_flow_handle_USER，添加可以操作的人员
                        if (nextNode.HANDLE == 0)
                        {
                            foreach (var handleU in nowHandleUserIdList)
                            {
                                endFlow.fa_task_flow_handle_USER.Add(new fa_task_flow_handle_USER { HANDLE_USER_ID = handleU });
                            }
                            db.fa_task_flow.Add(endFlow);
                        }
                        else //所有人必须处理
                        {
                            foreach (var handleU in nowHandleUserIdList)
                            {
                                var tmpTaskFlow = Fun.ClassToCopy<fa_task_flow, fa_task_flow>(endFlow);
                                tmpTaskFlow.ID = Fun.GetSeqID<fa_task_flow>();
                                tmpTaskFlow.HANDLE_USER_ID = handleU;
                                tmpTaskFlow.fa_task_flow_handle_USER.Add(new fa_task_flow_handle_USER { HANDLE_USER_ID = handleU });
                                db.fa_task_flow.Add(tmpTaskFlow);
                            }
                        }
                        foreach (var handleU in nowHandleUserIdList)
                        {
                            server.SmsSendOrder(null, taskUser.NAME, "待处理", handleU,Convert.ToInt32(task.KEY),task.FLOW_ID.ToString());
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 指定人
                    var allUserId = userIdArrStr.Trim(',').Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    foreach (var t in allUserId)
                    {
                        fa_task_flow endFlow = new fa_task_flow();
                        endFlow.ID = Fun.GetSeqID<fa_task_flow>();
                        endFlow.PARENT_ID = taskFlowId;
                        endFlow.TASK_ID = nowFlow.TASK_ID;
                        endFlow.LEVEL_ID = nowFlow.LEVEL_ID + 1;
                        endFlow.FLOWNODE_ID = nextFlownodeId;
                        var node = db.fa_flow_flownode.Single(x => x.ID == nextFlownodeId);
                        endFlow.IS_HANDLE = 0;
                        endFlow.ACCEPT_TIME = DateTime.Now;
                        endFlow.DEAL_STATUS = "待处理";
                        endFlow.NAME = string.Format("待【{0}】处理", node.NAME);
                        endFlow.HANDLE_URL = node.HANDLE_URL;
                        endFlow.SHOW_URL = node.SHOW_URL;
                        endFlow.ROLE_ID_STR = string.Join(",", nextNode.fa_role.Select(x => x.ID).ToList());
                        endFlow.HANDLE_USER_ID = t;
                        endFlow.START_TIME = DateTime.Now;
                        db.fa_task_flow.Add(endFlow);
                    }
                    #endregion
                }
                #endregion
            }


            if (
                            task.FLOW_ID != null &&
                            (new List<int>() { 1, 2, 3, 4 }).Contains(task.FLOW_ID.Value) &&
                            task.KEY.IsInt32()
                            )
            {
                int key = Convert.ToInt32(task.KEY);
                var order = db.YL_ORDER.SingleOrDefault(x => x.ID == key);
                order.STATUS = butName;
                switch (task.FLOW_ID)
                {
                    case 1:
                        switch (butName)
                        {
                            case "转算单员":
                                order.STATUS = "待投保";
                                break;
                            case "提交付款":
                                order.STATUS = "待投保";
                                break;
                        }
                        break;
                }
                order.STATUS_TIME = DateTime.Now;
            }

            return true;
        }
        

        public static  TTask TaskSingle(string loginKey, ref ErrorInfo err, int id,int flowId=0)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }

            using (DBEntities db = new DBEntities())
            {
                var allFlownodeId = db.fa_flow_flownode_flow.Where(x => x.fa_role.Where(y => gu.RoleID.Contains(y.ID)).Count() > 0).Select(x => x.FROM_FLOWNODE_ID).ToList();

                var task = db.fa_task.SingleOrDefault(x => x.ID == id);
                if (task == null)
                {
                    task = db.fa_task_flow.SingleOrDefault(x => x.ID == flowId).fa_task;
                }

                if (task == null)
                {
                    err.IsError = true;
                    err.Message = "任务不存在";
                    return null;
                }

                var tmp = Fun.ClassToCopy<fa_task, TTask>(task);
                var createUser = db.fa_user.SingleOrDefault(x => x.ID == task.CREATE_USER);
                #region 计算渠道信息
                //if (!string.IsNullOrEmpty(task.KEY))
                //{
                //    var channelId=task.KEY.Split('|')[0];
                //    var channel = db.YL_CHANNEL_INFO.SingleOrDefault(x=>x.ID==channelId);
                //    var channelOnline = db.YL_CHANNEL_COMPUTER.Where(x => x.CHANNEL_ID == channelId && x.STATUS == "在线").Count();
                //    if (channel != null)
                //    {
                //        tmp.ChanneName = channel.CHANNEL_NAME;
                //        tmp.ChannelAddress = channel.CHANNEL_ADDR;
                //        tmp.ChannelOnLine = string.Format("当前{0}台终端在线", channelOnline);
                //    }
                //}
                //else if (createUser.fa_user_INFO != null && createUser.fa_user_INFO.YL_CHANNEL_INFO.Count() > 0)
                //{
                //    var channel = createUser.fa_user_INFO.YL_CHANNEL_INFO.ToList()[0];
                //    tmp.ChanneName = channel.CHANNEL_NAME;
                //    tmp.ChannelAddress = channel.CHANNEL_ADDR;
                //}
                #endregion
                tmp.CreatePhone = createUser.LOGIN_NAME;


                IList<TTaskFlow> AllFlow = new List<TTaskFlow>();
                foreach (var flow in task.fa_task_flow.OrderBy(x => x.ID).ToList())
                {
                    if (flow.FLOWNODE_ID == 8888) continue;
                    if (task.CREATE_USER_NAME.Equals("系统派发"))
                    {
                        if (flow.LEVEL_ID < 3) continue;
                    }
                    else
                    {
                        if (flow.LEVEL_ID < 2) continue;
                    }
                    TTaskFlow nowFlow = Fun.ClassToCopy<fa_task_flow, TTaskFlow>(flow);

                    if (string.IsNullOrEmpty(flow.NAME))
                    {
                        var allHandle = flow.fa_task_flow_handle.ToList();
                        var allHandleAllUser = flow.fa_task_flow_handle_USER.ToList();
                        if (allHandle.Count() > 0)
                        {
                            nowFlow.NAME = allHandle[0].DEAL_USER_NAME + "处理";

                        }
                        else if (allHandleAllUser.Count() > 0)
                        {
                            nowFlow.NAME = "待处理";
                        }
                    }

                    nowFlow.FlowId = (flow.fa_task.FLOW_ID == null) ? 0 : flow.fa_task.FLOW_ID.Value;
                    if (!string.IsNullOrEmpty(flow.ROLE_ID_STR))
                    {
                        var allRoleId = flow.ROLE_ID_STR.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        nowFlow.RoleList = db.fa_role.Where(x => allRoleId.Contains(x.ID)).ToList().Select(x => new KTV { K = x.ID.ToString(), V = x.NAME, child = x.fa_user.ToList().Select(y => new KTV { K = y.ID.ToString(), V = y.NAME }).ToList() }).ToList();
                    }

                    nowFlow.TaskName = flow.fa_task.TASK_NAME;
                    nowFlow.TaskRemark = flow.fa_task.REMARK;
                    if (flow.fa_task_flow2 != null && flow.fa_task_flow2.fa_task_flow_handle.Count() > 0)
                    {
                        var handle = flow.fa_task_flow2.fa_task_flow_handle.ToList()[0];
                        nowFlow.SendUserId = handle.DEAL_USER_ID;
                        nowFlow.SendUserName = handle.DEAL_USER_NAME;
                    }
                    nowFlow.AllHandleContent = string.Join(";", flow.fa_task_flow_handle.Select(x => x.CONTENT).ToList());

                    foreach (var handle in flow.fa_task_flow_handle.ToList())
                    {
                        var tmpHandle = Fun.ClassToCopy<fa_task_flow_handle, TTaskFlowHandle>(handle);
                        var dealUser=db.fa_user.SingleOrDefault(x=>x.ID==tmpHandle.DEAL_USER_ID);
                        foreach (var file in handle.fa_files.ToList())
                        {
                            var tmpFile = Fun.ClassToCopy<fa_files, FILES>(file);
                            tmpHandle.AllFiles .Add(tmpFile);
                            nowFlow.AllHandleFiles.Add(tmpFile);
                        }
                        nowFlow.AllHandle.Add(tmpHandle);
                        nowFlow.DealUserName = handle.DEAL_USER_NAME;
                        if(dealUser!=null)
                        {
                            nowFlow.DealRole = string.Join(",", dealUser.fa_role.ToList().Select(x => x.NAME).ToArray());

                            var ent = dealUser.fa_district;
                            IList<string> idArr = new List<string>();
                            while (ent != null)
                            {
                                idArr.Add(ent.NAME);
                                ent = ent.fa_district2;
                            }
                            idArr = idArr.Reverse().ToList();

                            nowFlow.DealUserDistrictName = string.Join(".", idArr);
                            nowFlow.DealUserPhone = dealUser.LOGIN_NAME;
                            
                        }
                    }
                    
                    AllFlow.Add(nowFlow);
                }
                tmp.AllFlow = AllFlow;
                #region 计算所有按钮


                var nowTaskFlowList = task.fa_task_flow.Where(x => x.IS_HANDLE == 0 && (x.HANDLE_USER_ID == gu.UserId || (x.FLOWNODE_ID != null && allFlownodeId.Contains(x.FLOWNODE_ID.Value)))).ToList();
                if (nowTaskFlowList.Count() > 0)
                {
                    var nowTaskFlow = nowTaskFlowList[0];
                    tmp.NowFlowId = nowTaskFlow.ID;
                    if (nowTaskFlow.FLOWNODE_ID != null) //非任务工单
                    {
                      
                        if (nowTaskFlow.ACCEPT_TIME == null)
                        {
                           // tmp.AllButton = new List<string> { "驳回", "受理" };

                            tmp.AllButton = new List<string> {"受理" };
                        }
                        else
                        {
                            tmp.NowNodeFlowId = nowTaskFlow.FLOWNODE_ID.Value;
                            var allBut = db.fa_flow_flownode_flow.Where(x => 
                            x.FROM_FLOWNODE_ID == nowTaskFlow.FLOWNODE_ID && 
                            x.FLOW_ID == task.FLOW_ID && 
                            x.fa_role.Where(y=>gu.RoleID.Contains(y.ID)).Count()>0
                            ).ToList();
                            tmp.AllButton = allBut.Select(x => x.STATUS).ToList();
                            //if (task.fa_task_flow.Count() > 3 && !nowTaskFlow.fa_task_flow2.DEAL_STATUS.Equals("驳回")) tmp.AllButton.Insert(0, "驳回");
                        }
                    }
                    else {
                        if (nowTaskFlow.fa_task_flow2.PARENT_ID == null)
                        {
                            tmp.AllButton = new List<string>() { "转派", "归档" };
                        }
                        else
                        {
                            tmp.AllButton = new List<string>() { "转派", "回复" };
                        }
                    }
                    
                }
                #endregion

                return tmp;
            }
        }


        public static TTask TaskGetCreateSingle(string loginKey, ref ErrorInfo err, int flowId)
        {
            GlobalUser gu = Global.GetUser(loginKey);
            if (gu == null)
            {
                err.IsError = true;
                err.Message = "登录超时";
                return null;
            }
            TTask reEnt = new TTask();
            using (DBEntities db = new DBEntities())
            {
                var allFlowNode=db.fa_flow_flownode_flow.Where(x=>x.FLOW_ID==flowId).ToList();
                var startNode = allFlowNode.SingleOrDefault(x => x.FROM_FLOWNODE_ID == 9999);
                var allNextFlownode = allFlowNode.Where(x => x.FROM_FLOWNODE_ID == startNode.TO_FLOWNODE_ID&& x.fa_role.Where(y => gu.RoleID.Contains(y.ID)).Count() > 0).ToList();
                if (allNextFlownode.Count() == 0)
                {
                    err.IsError = true;
                    err.Message = "该用户无权创建工单";
                    return null;
                }
                reEnt.AllButton = allNextFlownode.Select(x => x.STATUS).ToList();
            }
            return reEnt;
        }

        /// <summary>
        /// 更新流程修改,用于其它页面修改后，引发的流程必须包括，现在流程ID,和按钮类型
        /// </summary>
        /// <param name="gu"></param>
        /// <param name="err"></param>
        /// <param name="db"></param>
        /// <param name="taskFlowID"></param>
        /// <param name="nowStatus"></param>
        /// <param name="reMark"></param>
        /// <returns></returns>
        public static bool UpDataTaskFlow(GlobalUser gu, ref ErrorInfo err, DBEntities db, TNode node)
        {

            fa_task_flow taskFlow = db.fa_task_flow.SingleOrDefault(x => x.ID == node.ID);
            if (taskFlow == null)
            {
                err.IsError = true;
                err.Message = "程序节点不存在";
                return false;
            }
            //如果已经处理，则跳过
            if (taskFlow.IS_HANDLE == 1)
            {
                return true;
            }
            fa_flow flow = taskFlow.fa_task.fa_flow;
            if (flow != null)
            {

                //终点流程
                IList<fa_flow_flownode_flow> endFlow = flow.fa_flow_flownode_flow.Where(x => x.TO_FLOWNODE_ID == 8888).ToList();
                //当前流程节点
                fa_flow_flownode flowNode = db.fa_flow_flownode.SingleOrDefault(x => x.ID == taskFlow.FLOWNODE_ID);
                if (string.IsNullOrEmpty(node.NowStatus))
                {
                    node.NowStatus = flow.fa_flow_flownode_flow.SingleOrDefault(y => y.FROM_FLOWNODE_ID == flowNode.ID).STATUS;
                }
                //目标节点
                taskFlow.DEAL_STATUS = node.NowStatus;
                taskFlow.HANDLE_USER_ID = gu.UserId;
                taskFlow.IS_HANDLE = 1;

                taskFlow.fa_task_flow_handle.Add(new fa_task_flow_handle
                {
                    ID = Fun.GetSeqID<fa_task_flow_handle>(),
                    CONTENT = node.Remark,
                    DEAL_TIME = DateTime.Now,
                    DEAL_USER_ID = gu.UserId,
                    DEAL_USER_NAME = gu.UserName,
                    TASK_FLOW_ID = taskFlow.ID
                });

                fa_flow_flownode_flow flowNodeFlowNext = flow.fa_flow_flownode_flow.SingleOrDefault(y => y.FROM_FLOWNODE_ID == flowNode.ID && y.STATUS == node.NowStatus);
                if (flowNodeFlowNext == null) return true;
                //如果是结束节
                if (flowNodeFlowNext.TO_FLOWNODE_ID == 8888)
                {
                    if (endFlow.Count == 0)
                    {
                        node.NowStatus = "完成";
                    }
                    else
                    {
                        node.NowStatus = endFlow[0].STATUS;
                    }
                    taskFlow.fa_task.STATUS = node.NowStatus;
                    taskFlow.fa_task.STATUS_TIME = DateTime.Now;
                    return true;
                }


                //下步节点
                fa_flow_flownode flowNodeNext = db.fa_flow_flownode.SingleOrDefault(x => x.ID == flowNodeFlowNext.TO_FLOWNODE_ID); ;
                if (flowNodeNext == null)
                {
                    return false;
                }
                taskFlow.HANDLE_URL = flowNode.HANDLE_URL;
                taskFlow.SHOW_URL = flowNode.SHOW_URL;
                taskFlow.NAME = flowNode.NAME;


                //所有人必须处理
                if (flowNodeFlowNext.HANDLE == 1)
                {
                    var noDealNum = taskFlow.fa_task_flow2.fa_task_flow1.Where(x => x.IS_HANDLE == 0 && x.ID != flow.ID).Count();
                    //表示还有人没有处理，退出
                    if (noDealNum > 0) return true;
                }
                else if (flowNodeFlowNext.HANDLE == 0)
                {
                    taskFlow.fa_task.STATUS = node.NowStatus;
                    taskFlow.fa_task.STATUS_TIME = DateTime.Now;

                    string showUrl = flowNodeNext.SHOW_URL;
                    if (flowNodeNext.HANDLE_URL == "~/Task/Handle")
                    {
                        showUrl = string.Format("{0}?id={1}", taskFlow.SHOW_URL, taskFlow.ID);
                    }

                    switch (flowNodeFlowNext.ASSIGNER)
                    {
                        case 0://指定角色
                            if (flowNodeFlowNext.fa_role.Where(x => gu.RoleID.Contains(x.ID)).Count() == 0 && taskFlow.HANDLE_USER_ID != gu.UserId)
                            {
                                err.IsError = true;
                                err.Message = "该用户无操作权限";
                                return false;
                            }
                            fa_task_flow taskFlowNext = new fa_task_flow();
                            taskFlowNext.TASK_ID = taskFlow.TASK_ID;
                            taskFlowNext.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                            taskFlowNext.FLOWNODE_ID = flowNodeNext.ID;
                            taskFlowNext.HANDLE_URL = flowNodeNext.HANDLE_URL;
                            taskFlowNext.SHOW_URL = showUrl;
                            taskFlowNext.IS_HANDLE = 0;
                            taskFlowNext.NAME = flowNodeNext.NAME;
                            taskFlowNext.START_TIME = DateTime.Now;

                            taskFlow.fa_task_flow1.Add(taskFlowNext);
                            break;
                        case 1://指定人
                            var allUserIdList = node.UserIdArrStr.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                            foreach (var t in allUserIdList)
                            {
                                fa_task_flow temp = new fa_task_flow();
                                temp.TASK_ID = taskFlow.TASK_ID;
                                temp.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                                temp.FLOWNODE_ID = flowNodeNext.ID;
                                temp.HANDLE_URL = flowNodeNext.HANDLE_URL;
                                temp.SHOW_URL = showUrl;
                                temp.HANDLE_USER_ID = t;
                                temp.IS_HANDLE = 0;
                                temp.NAME = flowNodeNext.NAME;
                                taskFlow.fa_task_flow1.Add(temp);
                            }
                            break;
                        case 2://返回发起人
                            taskFlowNext = new fa_task_flow();
                            taskFlowNext.TASK_ID = taskFlow.TASK_ID;
                            taskFlowNext.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                            taskFlowNext.FLOWNODE_ID = flowNodeNext.ID;
                            taskFlowNext.HANDLE_URL = flowNodeNext.HANDLE_URL;
                            taskFlowNext.SHOW_URL = showUrl;
                            taskFlowNext.HANDLE_USER_ID = taskFlow.fa_task_flow2.HANDLE_USER_ID;
                            taskFlowNext.IS_HANDLE = 0;
                            taskFlowNext.NAME = flowNodeNext.NAME;
                            taskFlowNext.START_TIME = DateTime.Now;
                            taskFlow.fa_task_flow1.Add(taskFlowNext);
                            break;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 超级管理员更改主流程
        /// </summary>
        /// <param name="gu"></param>
        /// <param name="err"></param>
        /// <param name="db"></param>
        /// <param name="taskFlowID"></param>
        /// <param name="nowFlowNodeID"></param>
        /// <param name="reMark"></param>
        /// <returns></returns>
        public static bool SupUpDataTaskFlow(GlobalUser gu, ref ErrorInfo err, DBEntities db, int taskFlowID, int toFlowNodeID, string reMark)
        {

            fa_task_flow taskFlow = db.fa_task_flow.SingleOrDefault(x => x.ID == taskFlowID);
            if (taskFlow.FLOWNODE_ID == toFlowNodeID)
            {
                err.IsError = true;
                err.Message = "不能选择当前所在节点";
                return false;
            }
            //如果已经处理，则跳过
            if (taskFlow.IS_HANDLE == 1)
            {
                return true;
            }

            fa_flow flow = taskFlow.fa_task.fa_flow;
            //流程的终点ID
            int endFlowNodeID = 0;
            #region 查找流程的终点ID
            IList<int> toAr = flow.fa_flow_flownode_flow.Select(x => x.TO_FLOWNODE_ID).ToList();
            IList<int> fromAr = flow.fa_flow_flownode_flow.Select(x => x.FROM_FLOWNODE_ID).ToList();
            foreach (var t in toAr)
            {
                if (!fromAr.Contains(t))
                {
                    endFlowNodeID = t;
                }
            }
            #endregion
            //终点流程
            IList<fa_flow_flownode_flow> endFlow = flow.fa_flow_flownode_flow.Where(x => x.TO_FLOWNODE_ID == endFlowNodeID).ToList();

            //fa_flow_flownode flowNode = db.fa_flow_flownode.SingleOrDefault(x => x.ID == taskFlow.FLOWNODE_ID);
            //if (string.IsNullOrEmpty(nowStatus))
            //{
            //    nowStatus = flow.fa_flow_flownode_flow.SingleOrDefault(y => y.FROM_FLOWNODE_ID == flowNode.ID).STATUS;
            //}
            //fa_flow_flownode_flow flowNodeFlowNext = flow.fa_flow_flownode_flow.SingleOrDefault(y => y.FROM_FLOWNODE_ID == flowNode.ID && y.STATUS == nowStatus);
            //if (flowNodeFlowNext.fa_role.Where(x => gu.RoleID.Contains(x.ID)).Count() == 0)
            //{
            //    err.IsError = true;
            //    err.Message = "该用户无操作权限";
            //    return false;
            //}

            //要跳转到的流程节点
            fa_flow_flownode flowNodeNext = null;
            flowNodeNext = db.fa_flow_flownode.SingleOrDefault(x => x.ID == toFlowNodeID);

            if (taskFlow == null)
            {
                err.IsError = true;
                err.Message = "程序节点不存在";
                return false;
            }

            if (taskFlow.IS_HANDLE == 0)
            {
                taskFlow.DEAL_STATUS = "超级管理员处理";
                taskFlow.HANDLE_USER_ID = gu.UserId;
                taskFlow.HANDLE_URL = "~/Task/SupHandle";

                taskFlow.SHOW_URL = "";
                //taskFlow.SHOW_URL = flowNode.SHOW_URL;
                taskFlow.IS_HANDLE = 1;
                taskFlow.NAME = "跳转流程至:" + flowNodeNext.NAME;

                taskFlow.fa_task_flow_handle.Add(new fa_task_flow_handle
                {
                    ID = Fun.GetSeqID<fa_task_flow_handle>(),
                    CONTENT = reMark,
                    DEAL_TIME = DateTime.Now,
                    DEAL_USER_ID = gu.UserId,
                    DEAL_USER_NAME = gu.UserName,
                    TASK_FLOW_ID = taskFlow.ID
                });

            }


            taskFlow.fa_task.STATUS = "跳转流程至:" + flowNodeNext.NAME;
            taskFlow.fa_task.STATUS_TIME = DateTime.Now;

            if (flowNodeNext != null)
            {
                //如果不是结束节点则添加任务
                if (endFlowNodeID != flowNodeNext.ID)
                {
                    fa_task_flow taskFlowNext = new fa_task_flow();
                    taskFlowNext.TASK_ID = taskFlow.TASK_ID;
                    taskFlowNext.LEVEL_ID = taskFlow.LEVEL_ID + 1;
                    taskFlowNext.FLOWNODE_ID = flowNodeNext.ID;
                    taskFlowNext.HANDLE_URL = flowNodeNext.HANDLE_URL;
                    taskFlowNext.SHOW_URL = flowNodeNext.SHOW_URL;
                    taskFlowNext.IS_HANDLE = 0;
                    taskFlowNext.START_TIME = DateTime.Now;
                    taskFlowNext.NAME = flowNodeNext.NAME;
                    taskFlow.fa_task_flow1.Add(taskFlowNext);
                }
                //是结束节点
                else
                {
                    //if (endFlow.Count == 0)
                    //{
                    //    nowStatus = "完成";
                    //}
                    //else
                    //{
                    //    nowStatus = endFlow[0].STATUS;
                    //}
                }
            }

            return true;
        }


        /// <summary>
        /// 设置公共基本信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ent"></param>
        /// <param name="taskID"></param>
        /// <param name="flownodeId"></param>
        public static void SetFlowBaseInfo(DBEntities db, ProInterface.Models.TNode ent, fa_task_flow taskFlow)
        {
            SetFlowBaseInfo(db, ent, taskFlow.FLOWNODE_ID, taskFlow.fa_task.FLOW_ID, taskFlow.TASK_ID);
                            
        }


        public static void SetFlowBaseInfo(DBEntities db, ProInterface.Models.TNode ent, int? flownodeId, int? flowId,int taskId=0)
        {

            ent.AllStatus = new Dictionary<int,string>();
            if (flownodeId != null && flowId != null)
            {
                foreach (var t in db.fa_flow_flownode_flow.Where(x => x.FROM_FLOWNODE_ID == flownodeId && x.FLOW_ID == flowId).ToList())
                {
                    ent.AllStatus.Add(new SelectListItem { Value = t.STATUS, Text = t.STATUS });
                    var allRole = t.fa_role;
                    if (t.ASSIGNER == 1)
                    {
                        if (allRole.Count() == 0)
                        {
                            allRole = db.fa_role.ToList();
                        }
                    }
                    ent.Assigner = t.ASSIGNER;
                    ent.AllRole = allRole.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NAME }).ToList();
                }
            }
            else
            {
                ent.AllStatus.Add(new SelectListItem { Text = "完成", Value = "完成" });
                ent.AllStatus.Add(new SelectListItem { Text = "转派", Value = "转派" });
            }
            var task = db.fa_task.SingleOrDefault(x => x.ID == taskId);
            if (task != null)
            {
                ent.TaskName = task.TASK_NAME;
            }
            var taskFlow=db.fa_task_flow.SingleOrDefault(x=>x.ID==ent.ID);
            if(taskFlow!=null)
            {
                var allFile = new List<FILES>();
                foreach (var t in taskFlow.fa_task_flow_handle.ToList())
                {
                    foreach (var t0 in t.fa_files.ToList())
                    {
                        allFile.Add(Fun.ClassToCopy<fa_files,FILES>(t0));
                    }
                }
                ent.AllFilesStr = JSON.DecodeToStr(allFile);
            }
        }

    }
}
