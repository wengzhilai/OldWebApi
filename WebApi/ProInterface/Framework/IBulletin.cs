using ProInterface.Models;
using System.Collections.Generic;


namespace ProInterface
{
    public interface IBulletin : IZ_Bulletin
    {
        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="inEnt">实体类</param>
        /// <returns>添加公告</returns>
        object BulletinSave(string loginKey, ref ErrorInfo err, TBulletin inEnt, IList<string> allPar);

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <returns></returns>
        TBulletin BulletinSingle(string loginKey, ref ErrorInfo err, int? bullID);




        /// <summary>
        /// 获取最新的公告列表
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        IList<TBulletin> BulletinGetNew(string loginKey, ref ErrorInfo err);


        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="keyId">删除公告</param>
        /// <returns></returns>
        bool BulletinDelete(string loginKey, ref ErrorInfo err, int keyId);

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SetTop(string loginKey, ref ErrorInfo err, int id, bool istop);

        /// <summary>
        /// 重要
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SetImport(string loginKey, ref ErrorInfo err, int id, bool isimport);


        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="loginKey">登录凭证</param>
        /// <param name="err">错误信息</param>
        /// <param name="whereLambda">条件lambda表达表</param>
        /// <returns></returns>
        TBulletin BulletinSingleByTitle(string loginKey, ref ErrorInfo err, string title);

        Dictionary<int, string> BulletinType();
        /// <summary>
        /// 公共查询
        /// </summary>
        /// <param name="err"></param>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        ApiPagingDataBean<TBulletin> BulletinList(ref ErrorInfo err, ApiRequesPageBean<ApiPagingDataBean<TBulletin>> inEnt);

        /// <summary>
        /// 获取类型下所有公告
        /// </summary>
        /// <param name="loginKey"></param>
        /// <param name="err"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        Dictionary<int, string> BulletinByTypeCode(string loginKey, ref ErrorInfo err, string typeCode);

    }
}
