using ProInterface;
using ProInterface.Models;
using System;
using System.Web.Http.ModelBinding;
using System.Linq;
using System.Collections.Generic;

namespace WebApi
{
    /// <summary>
    /// 公共方法，主要用于数据验证和抓错
    /// </summary>
    public class Fun<outT>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public delegate dynamic MyAction(ref ErrorInfo err, ApiLogingBean inEnt);
        public delegate dynamic MyAction1(ref ErrorInfo err, ApiRequesEntityBean<outT> inEnt);
        public delegate dynamic MyAction2<T>(ref ErrorInfo err, ApiRequesSaveEntityBean<T> inEnt);
        public delegate dynamic MyAction3(ref ErrorInfo err, ApiRequesPageBean<outT> inEnt);
        public delegate dynamic MyAction4(ref ErrorInfo err, ApiPagingDataBean<outT> inEnt);

        /// <summary>
        /// 难输入是否有效
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="modelState"></param>
        /// <param name="err"></param>
        public static void Valid(dynamic inEnt, ModelStateDictionary modelState, ref ErrorInfo err)
        {
            try
            {
                if (inEnt != null && inEnt.saveKeys != null)
                {
                    string[] saveKeyArr = inEnt.saveKeys.Split(',');
                    List<string> saveKeyList = saveKeyArr.ToList();
                    foreach (var t in modelState.Keys.ToList())
                    {
                        if (!saveKeyList.Contains(t))
                        {
                            modelState.Remove(t);
                        }
                    }
                }
            }
            catch { }
            if (inEnt == null) {
                err.IsError = true;
                err.Message = "参数错误";
                return;
            };
            if (!modelState.IsValid)
            {
                err.IsError = true;
                err.Message = "数据验证未通过:\\r\\n错误代码:" + string.Join(",", modelState.Keys);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inFun"></param>
        /// <returns></returns>
        public static dynamic Func(MyAction myAction, ref ErrorInfo err, ApiLogingBean inEnt)
        {
            try
            {
                var r = myAction(ref err, inEnt);
                return r;
            }
            catch(Exception e) {
                err.IsError = true;
                err.Message = e.Message;
                return err;
            }
        }
        public static dynamic Func(MyAction1 myAction, ref ErrorInfo err, ApiRequesEntityBean<outT> inEnt)
        {
            try
            {
                return myAction(ref err, inEnt);
            }
            catch (Exception e)
            {
                err.IsError = true;
                err.Message = e.Message;
                return err;
            }
        }
        public static dynamic Func<T>(MyAction2<T> myAction, ref ErrorInfo err, ApiRequesSaveEntityBean<T> inEnt)
        {
            try
            {
                return myAction(ref err, inEnt);
            }
            catch (Exception e)
            {
                err.IsError = true;
                err.Message = e.Message;
                return err;
            }
        }

        public static dynamic Func(MyAction3 myAction, ref ErrorInfo err, ApiRequesPageBean<outT> inEnt)
        {
            try
            {
                return myAction(ref err, inEnt);
            }
            catch (Exception e)
            {
                err.IsError = true;
                err.Message = e.Message;
                return err;
            }
        }
        public static dynamic Func(MyAction4 myAction, ref ErrorInfo err, ApiPagingDataBean<outT> inEnt)
        {
            try
            {
                return myAction(ref err, inEnt);
            }
            catch (Exception e)
            {
                err.IsError = true;
                err.Message = e.Message;
                return err;
            }
        }
    }
}